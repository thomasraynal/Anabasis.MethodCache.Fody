using System;

namespace Anabasis.MethodCache
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
	public sealed class CacheAttribute : Attribute
	{
		public CacheAttribute()
		{
		}

		public long AbsoluteExpirationRelativeToNowInMilliseconds { get; set; }
		public long SlidingExpirationInMilliseconds { get; set; }
	}
}
