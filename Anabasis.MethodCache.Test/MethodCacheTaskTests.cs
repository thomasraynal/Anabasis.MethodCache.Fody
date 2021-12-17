using Anabasis.MethodCache.Fody;
using Fody;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Anabasis.MethodCache.Test
{

	[NonParallelizable]
	public class MethodCacheTaskTests
	{

        private static TestResult TestResult { get; }

		static MethodCacheTaskTests()
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
			dynamic instance = TestHelpers.CreateInstance<TestClassTask>(MethodCacheTaskTests.TestResult.Assembly, null);

			var result = await instance.TestValueTypeMethod(1, 2);

			Assert.NotNull(result);

			var hasValue = CachingServices.Backend.TryGetValue(
				"Anabasis.MethodCache.Test.TestClassTask.TestValueTypeMethod|Int32|1;Int32|2",
				out Task<string> cacheValue);

			Assert.True(hasValue);
			Assert.AreEqual("3", await cacheValue);
		}

		[Test]
		public async Task  ShouldTestReferenceTypeMethod2()
		{
			dynamic instance = TestHelpers.CreateInstance<TestClassTask>(MethodCacheTaskTests.TestResult.Assembly, null);

			var result = await instance.TestReferenceTypeMethod2(new object(), new object());

			Assert.NotNull(result);

			var hasValue = CachingServices.Backend.TryGetValue(
				"Anabasis.MethodCache.Test.TestClassTask.TestReferenceTypeMethod2|Object|System.Object;Object|System.Object",
				out Task<string> cacheValue);

			Assert.True(hasValue);
			Assert.AreEqual("System.ObjectSystem.Object", await cacheValue);

		}

		[Test]
		public async Task ShouldTestTestReferenceTypeMethod()
		{
			dynamic instance = TestHelpers.CreateInstance<TestClassTask>(MethodCacheTaskTests.TestResult.Assembly, null);

			var result = await instance.TestReferenceTypeMethod("1", "2");

			Assert.NotNull(result);

			var hasValue = CachingServices.Backend.TryGetValue(
				"Anabasis.MethodCache.Test.TestClassTask.TestReferenceTypeMethod|String|1;String|2",
				out Task<string> cacheValue);

			Assert.True(hasValue);
			Assert.AreEqual("12", await cacheValue);

		}

		[Test]
		public async Task ShouldTestGenericsMethod()
		{
			dynamic instance = TestHelpers.CreateInstance<TestClassTask>(MethodCacheTaskTests.TestResult.Assembly, null);

			var result = await instance.TestGenerics<int>(1, 2);

			Assert.NotNull(result);

			var hasValue = CachingServices.Backend.TryGetValue(
				"SomeKey",
				out Task<string> cacheValue);

			Assert.False(hasValue);
			Assert.Null(cacheValue);

		}

		[Test]
		public async Task ShouldTestNoReturnValue()
		{
			dynamic instance = TestHelpers.CreateInstance<TestClassTask>(MethodCacheTaskTests.TestResult.Assembly, null);

			await instance.TestNoReturnValue(1, 2);

			var hasValue = CachingServices.Backend.TryGetValue(
				"Anabasis.MethodCache.Test.TestClassTask.TestValueTypeMethod|Int32|1;Int32|2",
				out Task<string> cacheValue);

			Assert.False(hasValue);
			Assert.Null(cacheValue);

		}

	}

}
