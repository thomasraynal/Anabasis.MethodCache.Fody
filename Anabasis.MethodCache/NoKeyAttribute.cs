using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anabasis.MethodCache
{
	[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
	public sealed class NoKeyAttribute : Attribute
	{
		public NoKeyAttribute()
		{
		}

		public double AbsoluteExpirationRelativeToNow { get; set; }
		public double SlidingExpiration { get; set; }
	}

}
