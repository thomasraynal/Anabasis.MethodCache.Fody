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
	public class MethodCacheValueTaskTests
	{

		private static TestResult TestResult { get; }

		static MethodCacheValueTaskTests()
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
		public async Task ShouldTestReferenceTypeMethod()
		{
			dynamic instance = TestHelpers.CreateInstance<TestClassValueTask>(MethodCacheValueTaskTests.TestResult.Assembly, null);

			var result = await instance.TestValueTypeMethod(1, 2);

			Assert.NotNull(result);

			var hasValue = CachingServices.Backend.TryGetValue(
				"Anabasis.MethodCache.Test.TestClassValueTask.TestValueTypeMethod|a|1;b|2",
				out ValueTask<string> cacheValue);

			Assert.True(hasValue);
			Assert.AreEqual("3", await cacheValue);
		}

        [Test]
        public async Task ShouldTestReferenceTypeMethod2()
        {
            dynamic instance = TestHelpers.CreateInstance<TestClassValueTask>(MethodCacheValueTaskTests.TestResult.Assembly, null);

            var result = await instance.TestReferenceTypeMethod2(new object(), new object());

            Assert.NotNull(result);

            var hasValue = CachingServices.Backend.TryGetValue(
                "Anabasis.MethodCache.Test.TestClassValueTask.TestReferenceTypeMethod2|a|System.Object;b|System.Object",
                out ValueTask<string> cacheValue);

            Assert.True(hasValue);
            Assert.AreEqual("System.ObjectSystem.Object", await cacheValue);

        }

        [Test]
        public async Task ShouldTestTestReferenceTypeMethod()
        {
            dynamic instance = TestHelpers.CreateInstance<TestClassValueTask>(MethodCacheValueTaskTests.TestResult.Assembly, null);

            var result = await instance.TestReferenceTypeMethod("1", "2");

            Assert.NotNull(result);

            var hasValue = CachingServices.Backend.TryGetValue(
                "Anabasis.MethodCache.Test.TestClassValueTask.TestReferenceTypeMethod|a|1;b|2",
                out ValueTask<string> cacheValue);

            Assert.True(hasValue);
            Assert.AreEqual("12", await cacheValue);

        }

        [Test]
        public async Task ShouldTestGenericsMethod()
        {
            dynamic instance = TestHelpers.CreateInstance<TestClassValueTask>(MethodCacheValueTaskTests.TestResult.Assembly, null);

            var result = await instance.TestGenerics<int>(1, 2);

            Assert.NotNull(result);

            var hasValue = CachingServices.Backend.TryGetValue(
                "SomeKey",
                out ValueTask<string> cacheValue);

            Assert.False(hasValue);
            Assert.AreEqual(null, await cacheValue);

        }

        [Test]
        public async Task ShouldTestNoReturnValue()
        {
            dynamic instance = TestHelpers.CreateInstance<TestClassValueTask>(MethodCacheValueTaskTests.TestResult.Assembly, null);

            await instance.TestNoReturnValue(1, 2);

            var hasValue = CachingServices.Backend.TryGetValue(
                "Anabasis.MethodCache.Test.TestClassValueTask.TestValueTypeMethod|a|1;b|2",
                out ValueTask<string> cacheValue);

            Assert.False(hasValue);
            Assert.AreEqual(null, await cacheValue);

        }

    }
}
