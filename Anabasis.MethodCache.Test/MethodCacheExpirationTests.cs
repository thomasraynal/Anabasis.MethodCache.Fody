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
	public class MethodCacheExpirationTests
	{

		private static TestResult TestResult { get; }

		static MethodCacheExpirationTests()
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
		public async Task ShouldTestAbsoluteExpiration()
		{
			dynamic instance = TestHelpers.CreateInstance<TestCacheExpiration>(MethodCacheExpirationTests.TestResult.Assembly, null);

			var result = instance.TestAbsoluteExpiration(1, 2);

			Assert.NotNull(result);

			var hasValue = CachingServices.Backend.TryGetValue(
				"Anabasis.MethodCache.TestCacheExpiration.TestAbsoluteExpiration|a|1;b|2",
				out string cacheValue);

			Assert.True(hasValue);
			Assert.AreEqual("3", cacheValue);

			await Task.Delay(1200);

			hasValue = CachingServices.Backend.TryGetValue(
				"Anabasis.MethodCache.TestCacheExpiration.TestAbsoluteExpiration|a|1;b|2",
				out string _);

			Assert.False(hasValue);

		}

		[Test]
		public async Task ShouldTestSlidingExpiration()
		{
			dynamic instance = TestHelpers.CreateInstance<TestCacheExpiration>(MethodCacheExpirationTests.TestResult.Assembly, null);

			var result = instance.TestSlidingExpiration(1, 2);

			Assert.NotNull(result);

			var hasValue = CachingServices.Backend.TryGetValue(
				"Anabasis.MethodCache.TestCacheExpiration.TestSlidingExpiration|a|1;b|2",
				out string cacheValue);

			Assert.True(hasValue);
			Assert.AreEqual("3", cacheValue);

			await Task.Delay(1200);

			hasValue = CachingServices.Backend.TryGetValue(
				"Anabasis.MethodCache.TestCacheExpiration.TestSlidingExpiration|a|1;b|2",
				out string _);

			Assert.False(hasValue);

		}
	}
}
