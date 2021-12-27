using Anabasis.MethodCache.Fody;
using Anabasis.MethodCache.Test.Common;
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
	public class MethodCacheIFormattableTests
	{

		private static TestResult TestResult { get; }

		static MethodCacheIFormattableTests()
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
		public void ShouldTestAbsoluteExpiration()
		{
			dynamic instance = TestHelpers.CreateInstance<TestIFormattable>(MethodCacheIFormattableTests.TestResult.Assembly, null);

			var result = instance.TestingIFormattable(1, new Item(2));

			Assert.NotNull(result);

			var hasValue = CachingServices.Backend.TryGetValue(
				"Anabasis.MethodCache.TestIFormattable.TestingIFormattable|a|1;b|2",
				out string cacheValue);

			Assert.True(hasValue);
			Assert.AreEqual("1Anabasis.MethodCache.Test.Common.Item", cacheValue);

		}

	}
}
