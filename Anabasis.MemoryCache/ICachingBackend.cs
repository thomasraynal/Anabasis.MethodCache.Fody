using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Anabasis.MemoryCache
{
	public interface ICachingBackend
	{
		Task Clear(CancellationToken cancellationToken = default);
		bool ContainsKey(string key);
		bool TryGetValue<TItem>(string key, out TItem value);
		void Set<TItem>(string key, TItem value);
		Task Invalidate(string key);
		Task InvalidateWhenContains(string predicate, bool isCaseSensitive = true);
		Task InvalidateWhenContains(string[] predicates, bool isCaseSensitive = true);
		Task InvalidateWhenStartWith(string[] predicates, bool isCaseSensitive = true);
	}
}
