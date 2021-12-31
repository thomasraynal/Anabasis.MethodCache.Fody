using System;

namespace Anabasis.MethodCache
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
	public sealed class CacheAttribute : Attribute
	{
		public const long Second = 1000 ;

		public const long Minute = Second * 60;

		public const long Hour = Minute * 60;

		public const long Day = Hour * 24;

		public const long Year = Day * 360;

		public CacheAttribute()
		{
		}

		public long AbsoluteExpirationRelativeToNowInMilliseconds { get; set; }
		public long SlidingExpirationInMilliseconds { get; set; }
	}
}
