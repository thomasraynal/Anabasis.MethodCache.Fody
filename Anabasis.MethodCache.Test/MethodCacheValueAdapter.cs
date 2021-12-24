using Anabasis.MethodCache.Fody;
using Fody;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anabasis.MethodCache.Test
{
	[NonParallelizable]
    public class MethodCacheValueAdapter
	{
		private static TestResult TestResult { get; }

		static MethodCacheValueAdapter()
		{
			var weavingTask = new ModuleWeaver();

			TestResult =
				weavingTask.ExecuteTestRun("Anabasis.MethodCache.Test.Assembly.dll", ignoreCodes: new[] { "0x80131869" }, runPeVerify: false);
		}

		[SetUp]
		public void Setup()
		{
			CachingServices.KeyBuilder = new TestCacheKeyBuilder();
			CachingServices.Backend = new TestBackend();

			CachingServices.Backend.SetValueAdapter(new EnumerableValueAdapter<string>());
		}

		[Test]
		public void ShouldTestEnumerableValueAdapter()
		{
			dynamic instance = TestHelpers.CreateInstance<TestValueAdapter>(MethodCacheValueAdapter.TestResult.Assembly, null);

			var result = instance.TestValueAdapterEnumerable("1", "2");

			Assert.NotNull(result);

			var hasValue = CachingServices.Backend.TryGetValue(
				"Anabasis.MethodCache.TestValueAdapter.TestValueAdapterEnumerable|a|1;b|2",
				out IEnumerable<string> cacheValue);

			Assert.True(hasValue);
			Assert.AreEqual("ab", cacheValue.Aggregate((a,b)=> $"{a}{b}"));
		}

		[Test]
		public void ShouldTestStreamValueAdapter()
		{

			dynamic instance = TestHelpers.CreateInstance<TestValueAdapter>(MethodCacheValueAdapter.TestResult.Assembly, null);

			var result = (Stream)instance.TestValueAdapterStream("1", "2");

			result.Dispose();

			Assert.NotNull(result);

			var hasValue = CachingServices.Backend.TryGetValue(
				"Anabasis.MethodCache.TestValueAdapter.TestValueAdapterStream|a|1;b|2",
				out Stream cacheValue);
		
			Assert.True(hasValue);

            using var memoryStream = new MemoryStream();

            cacheValue.CopyTo(memoryStream);
            Assert.AreEqual("test", Encoding.UTF8.GetString(memoryStream.ToArray()));

        }
	}
}
