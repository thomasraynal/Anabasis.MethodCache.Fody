using Anabasis.MemoryCache.Fody;
using Fody;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Anabasis.MemoryCache.Test
{

	[NonParallelizable]
	public class MemoryCacheBasicTests
	{

        private static TestResult TestResult { get; }

		static MemoryCacheBasicTests()
		{
			var weavingTask = new ModuleWeaver();

            TestResult =
				weavingTask.ExecuteTestRun("Anabasis.MemoryCache.Test.Assembly.dll", ignoreCodes: new[] { "0x80131869" }, runPeVerify: false);
		}

		[SetUp]
		public void Setup()
        {
			CachingServices.KeyBuilder = new TestCacheKeyBuilder();
			CachingServices.Backend = new TestBackend();
		}

		[Test]
		public void ShouldTestGenerics()
		{
			dynamic instance = TestHelpers.CreateInstance<TestClassGenererics<int>>(MemoryCacheBasicTests.TestResult.Assembly, null);

			var result = instance.TestGenerics(1, 2);

			Assert.NotNull(result);

			var hasValue = CachingServices.Backend.TryGetValue(
				"Anabasis.MemoryCache.Test.TestClassGenererics<TItem>.TestGenerics|Int32|1;Int32|2",
				out string cacheValue);

			Assert.True(hasValue);
			Assert.AreEqual("12", cacheValue);
		}

		[Test]
		public void ShouldTestTestMethodGenerics()
		{
			dynamic instance = TestHelpers.CreateInstance<TestClassGenererics<int>>(MemoryCacheBasicTests.TestResult.Assembly, null);

			var result = instance.TestMethodGenerics(1, 2);

			Assert.NotNull(result);

			var hasValue = CachingServices.Backend.TryGetValue(
				"Anabasis.MemoryCache.Test.TestClassGenererics<TItem>.TestMethodGenerics<TItem2>|Int32|1;Int32|2",
				out string cacheValue);

			Assert.True(hasValue);
			Assert.AreEqual("12", cacheValue);

		}

		[Test]
		public void ShouldTestMethodGenerics2()
		{
			dynamic instance = TestHelpers.CreateInstance<TestClassGenererics<int>>(MemoryCacheBasicTests.TestResult.Assembly, null);

			var result = instance.TestMethodGenerics2(1, 2);

			Assert.NotNull(result);

			var hasValue = CachingServices.Backend.TryGetValue(
				"Anabasis.MemoryCache.Test.TestClassGenererics<TItem>.TestMethodGenerics2<TItem2>|Int32|1;Int32|2",
				out string cacheValue);

			Assert.True(hasValue);
			Assert.AreEqual("12", cacheValue);

		}

		[Test]
		public void ShouldTestMethodGenericsWithReferenceType()
		{
			dynamic instance = TestHelpers.CreateInstance<TestClassGenererics<string>>(MemoryCacheBasicTests.TestResult.Assembly, null);

			var result = instance.TestMethodGenerics2(2, "dfs");

			Assert.NotNull(result);

			var hasValue = CachingServices.Backend.TryGetValue(
				"Anabasis.MemoryCache.Test.TestClassGenererics<TItem>.TestMethodGenerics2<TItem2>|Int32|2;String|dfs",
				out string cacheValue);

			Assert.True(hasValue);
			Assert.AreEqual("2dfs", cacheValue);

		}

	}

}
