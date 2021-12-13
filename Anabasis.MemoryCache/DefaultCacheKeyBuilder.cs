using System;
using System.Collections.Generic;
using System.Text;

namespace Anabasis.MemoryCache
{
	public class DefaultCacheKeyBuilder : ICacheKeyBuilder
	{
		//public string CreateKey(CacheKeyParameters cacheKeyParameters)
		//{
		//	return
		//		cacheKeyParameters.MethodName +
		//		cacheKeyParameters.MethodParameters
		//			.Select(methodParameter => $"{methodParameter.ParameterName}|{methodParameter.ParameterHashCode}")
		//			.Aggregate((methodParameter1, methodParameter2) => $"{methodParameter1};{methodParameter2}");
		//}

		//public string CreateKey(string methodName)
		//{
		//	return "cacheKey";
		//}

        public void CreateKey(string methodName, params object[] parameters)
        {
			//return "cacheKey";
		}
    }
}
