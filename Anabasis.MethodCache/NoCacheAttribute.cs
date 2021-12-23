using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anabasis.MethodCache
{
	[AttributeUsage(AttributeTargets.Method, Inherited = false)]
	public sealed class NoCacheAttribute : Attribute
	{
		public NoCacheAttribute()
		{
		}

		public double AbsoluteExpirationRelativeToNow { get; set; }
		public double SlidingExpiration { get; set; }
	}
}
