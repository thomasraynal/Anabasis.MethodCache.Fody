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
	public class MethodCacheNoKeyTests
	{

		private static TestResult TestResult { get; }

		static MethodCacheNoKeyTests()
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
		}

		[Test]
		public void ShouldNotUseSecondParameter()
		{
			dynamic instance = TestHelpers.CreateInstance<TestNoKeyAttribute>(MethodCacheNoKeyTests.TestResult.Assembly, null);

			var result = instance.TestNoSecondParameter("1", "2");

			Assert.NotNull(result);

			var hasValue = CachingServices.Backend.TryGetValue(
				"Anabasis.MethodCache.TestNoKeyAttribute.TestNoSecondParameter<TItem>|a|1",
				out string cacheValue);

			Assert.True(hasValue);
			Assert.AreEqual("12", cacheValue);
		}

		[Test]
		public void ShouldNotUseFirstParameter()
		{
			dynamic instance = TestHelpers.CreateInstance<TestNoKeyAttribute>(MethodCacheNoKeyTests.TestResult.Assembly, null);

			var result = instance.TestNoFirstParameter(new object(), new object());

			Assert.NotNull(result);

			var hasValue = CachingServices.Backend.TryGetValue(
				"Anabasis.MethodCache.TestNoKeyAttribute.TestNoFirstParameter<TItem>|b|System.Object",
				out string cacheValue);

			Assert.True(hasValue);
			Assert.AreEqual("System.ObjectSystem.Object", cacheValue);

		}
	}
}
