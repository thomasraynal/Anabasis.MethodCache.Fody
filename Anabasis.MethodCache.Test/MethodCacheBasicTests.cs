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
	public class MethodCacheGenericTests
	{

        private static TestResult TestResult { get; }

		static MethodCacheGenericTests()
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
		public void ShouldTestReferenceTypeMethod()
		{
			dynamic instance = TestHelpers.CreateInstance<TestClassSimple>(MethodCacheGenericTests.TestResult.Assembly, null);

			var result = instance.TestReferenceTypeMethod("1", "2");

			Assert.NotNull(result);

			var hasValue = CachingServices.Backend.TryGetValue(
				"Anabasis.MethodCache.Test.TestClassSimple.TestReferenceTypeMethod|String|1;String|2",
				out string cacheValue);

			Assert.True(hasValue);
			Assert.AreEqual("12", cacheValue);
		}

		[Test]
		public void ShouldTestReferenceTypeMethod2()
		{
			dynamic instance = TestHelpers.CreateInstance<TestClassSimple>(MethodCacheGenericTests.TestResult.Assembly, null);

			var result = instance.TestReferenceTypeMethod2(new object(), new object());

			Assert.NotNull(result);

			var hasValue = CachingServices.Backend.TryGetValue(
				"Anabasis.MethodCache.Test.TestClassSimple.TestReferenceTypeMethod2|Object|System.Object;Object|System.Object",
				out string cacheValue);

			Assert.True(hasValue);
			Assert.AreEqual("System.ObjectSystem.Object", cacheValue);

		}

		[Test]
		public void ShouldTestValueTypeMethod()
		{
			dynamic instance = TestHelpers.CreateInstance<TestClassSimple>(MethodCacheGenericTests.TestResult.Assembly, null);

			var result = instance.TestValueTypeMethod(1, 2);

			Assert.NotNull(result);

			var hasValue = CachingServices.Backend.TryGetValue(
				"Anabasis.MethodCache.Test.TestClassSimple.TestValueTypeMethod|Int32|1;Int32|2",
				out string cacheValue);

			Assert.True(hasValue);
			Assert.AreEqual("3", cacheValue);

		}

		[Test]
		public void ShouldTestNoCacheMethode()
		{
			dynamic instance = TestHelpers.CreateInstance<TestClassSimple>(MethodCacheGenericTests.TestResult.Assembly, null);

			var result = instance.TestNoCacheMethod(1, 2);

			Assert.NotNull(result);

			var hasValue = CachingServices.Backend.TryGetValue(
				"Anabasis.MethodCache.Test.TestClassSimple.TestValueTypeMethod|Int32|1;Int32|2",
				out string cacheValue);

			Assert.False(hasValue);
			Assert.Null(cacheValue);

		}

	}

}
