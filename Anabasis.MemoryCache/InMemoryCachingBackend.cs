using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Anabasis.MemoryCache
{
    public class InMemoryCachingBackend : ICachingBackend
    {

        public Task Clear(CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public bool ContainsKey(string key)
        {
            throw new System.NotImplementedException();
        }

        public Task Invalidate(string key)
        {
            throw new System.NotImplementedException();
        }

        public Task InvalidateWhenContains(string predicate, bool isCaseSensitive = true)
        {
            throw new System.NotImplementedException();
        }

        public Task InvalidateWhenContains(string[] predicates, bool isCaseSensitive = true)
        {
            throw new System.NotImplementedException();
        }

        public Task InvalidateWhenStartWith(string[] predicates, bool isCaseSensitive = true)
        {
            throw new System.NotImplementedException();
        }

        public void SetValue<TItem>(string key, TItem value)
        {
      
        }

        public bool TryGetValue<TItem>(string key, out TItem value)
        {
            value = default;
            return false;
        }
    }
}
