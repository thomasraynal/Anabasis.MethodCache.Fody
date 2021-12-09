using System;
using System.Collections.Generic;
using System.Text;

namespace Anabasis.MemoryCache
{
	public static class CachingServices
	{
		static CachingServices()
		{
			DefaultBackend = new InMemoryCachingBackend();
		}

		public static ICachingBackend DefaultBackend { get; set; }

	}
}
