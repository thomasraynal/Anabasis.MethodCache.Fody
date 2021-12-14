using System;

namespace Anabasis.MemoryCache
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
	public sealed class CacheAttribute : Attribute
	{
		public CacheAttribute()
		{
		}

		public double AbsoluteExpirationRelativeToNow { get; set; }

		public double SlidingExpiration { get; set; }
	}
}
