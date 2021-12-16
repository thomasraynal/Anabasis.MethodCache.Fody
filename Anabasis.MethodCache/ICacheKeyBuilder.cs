using System;
using System.Collections.Generic;
using System.Text;

namespace Anabasis.MethodCache
{
	public interface ICacheKeyBuilder
	{
		string CreateKey(string methodName, params object[] parameters);
	}
}
