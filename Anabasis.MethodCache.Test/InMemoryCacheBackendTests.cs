using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anabasis.MethodCache.Test
{
	public class InMemoryCacheBackendTests
	{
		[SetUp]
		public void Setup()
		{
			CachingServices.KeyBuilder = new DefaultCacheKeyBuilder();
			CachingServices.Backend = new InMemoryCachingBackend();
		}

		[Test]
		public async Task ShouldSetValueWithoutExpiration()
		{
			var argNames = new[] { "a", "b" };
			var argValues = new object[] { 1, "b" };

			var cacheKey = CachingServices.KeyBuilder.CreateKey(nameof(ShouldSetValueWithoutExpiration), argNames, argValues);

			Assert.AreEqual("ShouldSetValueWithoutExpiration|a|1;b|b", cacheKey);

			var cachedValue = "result";

			CachingServices.Backend.SetValue(cacheKey, cachedValue);

			var hasResult = CachingServices.Backend.TryGetValue<string>(cacheKey, out var result);

			Assert.True(hasResult);
			Assert.AreEqual(cachedValue, result);

			await Task.Delay(1000);

			hasResult = CachingServices.Backend.TryGetValue(cacheKey, out result);

			Assert.True(hasResult);
			Assert.AreEqual(cachedValue, result);

		}

		[Test]
		public async Task ShouldSetValueWithAbsoluteExpiration()
		{

			var argNames = new[] { "a", "b" };
			var argValues = new object[] { 1, "b" };

			var cacheKey = CachingServices.KeyBuilder.CreateKey(nameof(ShouldSetValueWithoutExpiration), argNames, argValues);

			Assert.AreEqual("ShouldSetValueWithoutExpiration|a|1;b|b", cacheKey);

			var cachedValue = "result";

			CachingServices.Backend.SetValue(cacheKey, cachedValue, absoluteExpirationRelativeToNowInMilliseconds: 500);

			var hasResult = CachingServices.Backend.TryGetValue<string>(cacheKey, out var result);

			Assert.True(hasResult);
			Assert.AreEqual(cachedValue, result);

			await Task.Delay(1000);

			hasResult = CachingServices.Backend.TryGetValue(cacheKey, out result);

			Assert.False(hasResult);

		}


		[Test]
		public async Task ShouldSetValueWithSlidingExpiration()
		{

			var argNames = new[] { "a", "b" };
			var argValues = new object[] { 1, "b" };

			var cacheKey = CachingServices.KeyBuilder.CreateKey(nameof(ShouldSetValueWithoutExpiration), argNames, argValues);

			Assert.AreEqual("ShouldSetValueWithoutExpiration|a|1;b|b", cacheKey);

			var cachedValue = "result";

			CachingServices.Backend.SetValue(cacheKey, cachedValue, slidingExpirationInMilliseconds: 500);

			var hasResult = CachingServices.Backend.TryGetValue<string>(cacheKey, out var result);

			Assert.True(hasResult);
			Assert.AreEqual(cachedValue, result);

			await Task.Delay(1000);

			hasResult = CachingServices.Backend.TryGetValue(cacheKey, out result);

			Assert.False(hasResult);

		}

		[Test]
		public async Task ShouldInvalidate()
		{

			var argNames = new[] { "a", "b" };
			var argValues = new object[] { 1, 2 };

			var cacheKey = CachingServices.KeyBuilder.CreateKey(nameof(ShouldSetValueWithoutExpiration), argNames, argValues);

			Assert.AreEqual("ShouldSetValueWithoutExpiration|a|1;b|2", cacheKey);

			var cachedValue = "result";

			CachingServices.Backend.SetValue(cacheKey, cachedValue);

			var hasResult = CachingServices.Backend.TryGetValue<string>(cacheKey, out var result);

			Assert.True(hasResult);
			Assert.AreEqual(cachedValue, result);

			await CachingServices.Backend.Invalidate(cacheKey);

			hasResult = CachingServices.Backend.TryGetValue(cacheKey, out result);

			Assert.False(hasResult);

		}

		[Test]
		public async Task ShouldClear()
		{

			var argNames = new[] { "a", "b" };
			var argValues = new object[] { 1, 2 };

			var cacheKey = CachingServices.KeyBuilder.CreateKey(nameof(ShouldSetValueWithoutExpiration), argNames, argValues);
			var cacheKey2 = CachingServices.KeyBuilder.CreateKey($"{nameof(ShouldSetValueWithoutExpiration)}2", argNames, argValues);

			Assert.AreEqual("ShouldSetValueWithoutExpiration|a|1;b|2", cacheKey);
			Assert.AreEqual("ShouldSetValueWithoutExpiration2|a|1;b|2", cacheKey2);

			var cachedValue = "result";

			CachingServices.Backend.SetValue(cacheKey, cachedValue);
			CachingServices.Backend.SetValue(cacheKey2, cachedValue);

			var hasResult = CachingServices.Backend.TryGetValue<string>(cacheKey, out var result);

			Assert.True(hasResult);
			Assert.AreEqual(cachedValue, result);

			hasResult = CachingServices.Backend.TryGetValue(cacheKey2, out  result);

			Assert.True(hasResult);
			Assert.AreEqual(cachedValue, result);

			await CachingServices.Backend.Clear();

			hasResult = CachingServices.Backend.TryGetValue(cacheKey, out result);

			Assert.False(hasResult);

			hasResult = CachingServices.Backend.TryGetValue(cacheKey2, out result);

			Assert.False(hasResult);

		}

		[TestCase(false)]
		[TestCase(true)]
		public async Task ShouldInvalidateWhenContains(bool caseSensitive)
		{

			var valueToRemove = "valueToRemove";

			var argNames = new[] { "a", "b" };
			var argValues = new object[] { 1, valueToRemove };

			var cacheKey = CachingServices.KeyBuilder.CreateKey(nameof(ShouldSetValueWithoutExpiration), argNames, argValues);

			Assert.AreEqual("ShouldSetValueWithoutExpiration|a|1;b|valueToRemove", cacheKey);

			var cachedValue = "result";

			CachingServices.Backend.SetValue(cacheKey, cachedValue);

			var hasResult = CachingServices.Backend.TryGetValue<string>(cacheKey, out var result);

			Assert.True(hasResult);
			Assert.AreEqual(cachedValue, result);

			await CachingServices.Backend.InvalidateWhenContains(caseSensitive ? valueToRemove : valueToRemove.ToLower(), caseSensitive);

			hasResult = CachingServices.Backend.TryGetValue(cacheKey, out result);

			Assert.False(hasResult);

		}

		[TestCase(false)]
		[TestCase(true)]
		public async Task ShouldInvalidateWhenStartWith(bool caseSensitive)
		{

			var valueToRemove = "valueToRemove";

			var argNames = new[] { "a", "b" };
			var argValues = new object[] { 1, valueToRemove + "Garbage" };

			var cacheKey = CachingServices.KeyBuilder.CreateKey(nameof(ShouldSetValueWithoutExpiration), argNames, argValues);

			Assert.AreEqual("ShouldSetValueWithoutExpiration|a|1;b|valueToRemoveGarbage", cacheKey);

			var cachedValue = "result";

			CachingServices.Backend.SetValue(cacheKey, cachedValue);

			var hasResult = CachingServices.Backend.TryGetValue<string>(cacheKey, out var result);

			Assert.True(hasResult);
			Assert.AreEqual(cachedValue, result);

			await CachingServices.Backend.InvalidateWhenContains(caseSensitive ? valueToRemove : valueToRemove.ToLower(), caseSensitive);

			hasResult = CachingServices.Backend.TryGetValue(cacheKey, out result);

			Assert.False(hasResult);

		}

	}
}
