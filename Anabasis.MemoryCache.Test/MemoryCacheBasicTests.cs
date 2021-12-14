using Anabasis.MemoryCache.Fody;
using Fody;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Anabasis.MemoryCache.Test
{
	public static class TestHelpers
	{
		public static dynamic CreateInstance<T>(Assembly assembly, object parameter)
		{
			if (null == parameter)
				return Activator.CreateInstance(CreateType<T>(assembly));

			return Activator.CreateInstance(CreateType<T>(assembly), parameter);
		}

		private static Type CreateType<T>(Assembly assembly)
		{
			return assembly.GetType(typeof(T).FullName ?? string.Empty);
		}
	}

	[NonParallelizable]
	public class MemoryCacheBasicTests
	{
        class TestBackend : ICachingBackend
        {

			public Dictionary<string, object> Cache = new();

            public Task Clear(CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task Invalidate(string key)
            {
                throw new NotImplementedException();
            }

            public Task InvalidateWhenContains(string predicate, bool isCaseSensitive = true)
            {
                throw new NotImplementedException();
            }

            public Task InvalidateWhenContains(string[] predicates, bool isCaseSensitive = true)
            {
                throw new NotImplementedException();
            }

            public Task InvalidateWhenStartWith(string[] predicates, bool isCaseSensitive = true)
            {
                throw new NotImplementedException();
            }

            public void SetValue<TItem>(string key, TItem value)
            {
				Cache.Add(key, value);
			}

            public bool TryGetValue<TItem>(string key, out TItem value)
            {
				value = default;
				return false;
            }
        }


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
			CachingServices.Backend = new TestBackend();
		}

		[Test]
		public void ShouldTestReferenceTypeMethod()
		{
			dynamic instance = TestHelpers.CreateInstance<TestClassSimple>(MemoryCacheBasicTests.TestResult.Assembly, null);

			var result = instance.TestReferenceTypeMethod("1", "2");

			Assert.NotNull(result);
	
		}

		[Test]
		public void ShouldTestReferenceTypeMethod2()
		{
			dynamic instance = TestHelpers.CreateInstance<TestClassSimple>(MemoryCacheBasicTests.TestResult.Assembly, null);

			var result = instance.TestReferenceTypeMethod2(new object(), new object());

			Assert.NotNull(result);

		}

		[Test]
		public void ShouldTestValueTypeMethod()
		{
			dynamic instance = TestHelpers.CreateInstance<TestClassSimple>(MemoryCacheBasicTests.TestResult.Assembly, null);

			var result = instance.TestValueTypeMethod(1, 2);

			Assert.NotNull(result);

		}

		[Test]
		public void ShouldTestNoCacheMethode()
		{
			dynamic instance = TestHelpers.CreateInstance<TestClassSimple>(MemoryCacheBasicTests.TestResult.Assembly, null);

			var result = instance.TestNoCacheMethod(1, 2);

			Assert.NotNull(result);

		}

	}

}
