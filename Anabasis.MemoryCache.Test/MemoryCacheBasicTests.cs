using Anabasis.MemoryCache.Fody;
using Fody;
using NUnit.Framework;
using System;
using System.Reflection;

namespace Anabasis.MemoryCache.Test
{
	public static class TestHelpers
	{
		public static dynamic CreateInstance<T>(Assembly assembly, object parameter)
		{
			return assembly != null ? Activator.CreateInstance(TestHelpers.CreateType<T>(assembly), parameter) : null;
		}

		private static Type CreateType<T>(Assembly assembly)
		{
			return assembly.GetType(typeof(T).FullName ?? string.Empty);
		}
	}

	public class MemoryCacheBasicTests
	{
		private static TestResult TestResult { get; }

		static MemoryCacheBasicTests()
		{
			var weavingTask = new ModuleWeaver();

			MemoryCacheBasicTests.TestResult =
				weavingTask.ExecuteTestRun("Anabasis.MemoryCache.Test.Assembly.dll", ignoreCodes: new[] { "0x80131869" }, runPeVerify: false);
		}

		[Test]
		public void BasicTest1CreateAndGet()
		{
			

			dynamic instance = TestHelpers.CreateInstance<Class1>(MemoryCacheBasicTests.TestResult.Assembly, null);

		}

	}

}
