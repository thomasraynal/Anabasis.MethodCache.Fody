using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Anabasis.MethodCache.Test
{
    public class TestBackend : ICachingBackend
    {

        private readonly List<IValueAdapter> _valueAdapters = new List<IValueAdapter>()
        {
            new StreamValueAdapter()
        };

        public void SetValueAdapter<TAdapter>(TAdapter value) where TAdapter : IValueAdapter
        {
            _valueAdapters.Add(value);
        }

        public IMemoryCache Cache = new MemoryCache(new MemoryCacheOptions()
        {
        });

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

        public void SetValue<TItem>(string key, TItem value, long absoluteExpirationRelativeToNowInMilliseconds=0, long slidingExpirationInMilliseconds = 0)
        {
            object storedValue = value;

            var valueAdapter = _valueAdapters.FirstOrDefault(valueAdapter => valueAdapter.ValueAdapterType == typeof(TItem));

            if (null != valueAdapter)
            {
                var typedAdapter = (IValueAdapter<TItem>)valueAdapter;

                storedValue = typedAdapter.GetStoredValue(value);
            }

            Cache.Set(key, storedValue, new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = default == absoluteExpirationRelativeToNowInMilliseconds ? null : TimeSpan.FromMilliseconds(absoluteExpirationRelativeToNowInMilliseconds),
                SlidingExpiration = default == slidingExpirationInMilliseconds ? null : TimeSpan.FromMilliseconds(slidingExpirationInMilliseconds)
            });

        }

        public bool TryGetValue<TItem>(string key, out TItem value)
        {

            var valueAdapter = _valueAdapters.FirstOrDefault(valueAdapter => valueAdapter.ValueAdapterType == typeof(TItem));

            if (null != valueAdapter)
            {
                var hasValue = Cache.TryGetValue(key, out object obj);

                if (!hasValue)
                {
                    value = default;
                    return false;
                }

                var typedAdapter = (IValueAdapter<TItem>)valueAdapter;

                value = typedAdapter.GetExposedValue(obj);

                return true;
            }

            return Cache.TryGetValue(key, out value);
        }

        public Task InvalidateWhenStartWith(string predicate, bool isCaseSensitive = true)
        {
            throw new NotImplementedException();
        }
    }
}
