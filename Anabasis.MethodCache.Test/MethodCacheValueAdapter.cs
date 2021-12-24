using Anabasis.MethodCache.Fody;
using Fody;
using NUnit.Framework;
using System;
using System.Collections.Generic;
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
				"Anabasis.MethodCache.TestNoKeyAttribute.TestNoSecondParameter<TItem>|a|1",
				out string cacheValue);

			Assert.True(hasValue);
			Assert.AreEqual("12", cacheValue);
		}

		[Test]
		public void ShouldTesttreamValueAdapter()
		{

			dynamic instance = TestHelpers.CreateInstance<TestValueAdapter>(MethodCacheValueAdapter.TestResult.Assembly, null);

			var result = instance.TestValueAdapterStream("1", "2");

			Assert.NotNull(result);

			var hasValue = CachingServices.Backend.TryGetValue(
				"Anabasis.MethodCache.TestNoKeyAttribute.TestNoFirstParameter<TItem>|b|System.Object",
				out string cacheValue);

			Assert.True(hasValue);
			Assert.AreEqual("System.ObjectSystem.Object", cacheValue);

		}
	}
}
