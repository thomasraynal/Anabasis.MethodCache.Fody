using Anabasis.MemoryCache.Fody;
using Fody;
using NUnit.Framework;
using System;
using System.Reflection;
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
		public async Task BasicTest1CreateAndGet()
		{
			

			dynamic instance = TestHelpers.CreateInstance<Class1>(MemoryCacheBasicTests.TestResult.Assembly, null);

			var gg = instance.TestMethod("qsfqsfqfqsfqfs", "kkkkk");

		}

	}

}
