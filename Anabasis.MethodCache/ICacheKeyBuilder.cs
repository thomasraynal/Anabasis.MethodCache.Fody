using System;
using System.Collections.Generic;
using System.Text;

namespace Anabasis.MethodCache
{
	public interface ICacheKeyBuilder
	{
		string CreateKey(string methodName, string[] argumentNames = null, object[] argumentValues = null);
	}
}
