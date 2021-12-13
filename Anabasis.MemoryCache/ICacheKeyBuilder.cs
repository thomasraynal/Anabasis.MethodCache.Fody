using System;
using System.Collections.Generic;
using System.Text;

namespace Anabasis.MemoryCache
{
	public interface ICacheKeyBuilder
	{
		string CreateKey(string methodName, params object[] parameters);
	}
}
