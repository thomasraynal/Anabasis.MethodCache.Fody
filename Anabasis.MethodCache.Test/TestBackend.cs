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
            if (Cache.ContainsKey(key))
            {
                value = (TItem)Cache[key];
                return true;
            }

            value = default;
            return false;
        }
    }
}
