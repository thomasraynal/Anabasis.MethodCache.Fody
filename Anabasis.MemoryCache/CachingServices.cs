using System;
using System.Collections.Generic;
using System.Text;

namespace Anabasis.MemoryCache
{
	public static class CachingServices
	{
		static CachingServices()
		{
			KeyBuilder = new DefaultCacheKeyBuilder();
			Backend = new InMemoryCachingBackend();
		}

		public static ICacheKeyBuilder KeyBuilder { get; set; }
		public static ICachingBackend Backend { get; set; }

	}
}
