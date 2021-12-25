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
	public class MethodCacheStaticTests
	{

		private static TestResult TestResult { get; }

		static MethodCacheStaticTests()
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
		public void ShouldTestStaticMethod()
		{
			dynamic instance = TestHelpers.CreateInstance<TestMethodStatic>(MethodCacheStaticTests.TestResult.Assembly, null);

			var result = (string)TestResult.Assembly.GetType("Anabasis.MethodCache.TestMethodStatic")
				.GetMethod("TestStaticMethod")
				.Invoke(instance, new object[] { 1, 2 });


			Assert.NotNull(result);

			var hasValue = CachingServices.Backend.TryGetValue(
				"Anabasis.MethodCache.TestMethodStatic.TestStaticMethod|a|1;b|2",
				out string cacheValue);

			Assert.True(hasValue);
			Assert.AreEqual("3", cacheValue);

		}

        [Test]
        public void ShouldTestStaticClass()
        {
			var result = (string)TestResult.Assembly.GetType("Anabasis.MethodCache.TestClassStatic")
			   .GetMethod("TestStaticClassMethod")
			   .Invoke(null, new object[] { 1, 2 });

            Assert.NotNull(result);

            var hasValue = CachingServices.Backend.TryGetValue(
				"Anabasis.MethodCache.TestClassStatic.TestStaticClassMethod|a|1;b|2",
                out string cacheValue);

            Assert.True(hasValue);
            Assert.AreEqual("3", cacheValue);

        }


    }
}
