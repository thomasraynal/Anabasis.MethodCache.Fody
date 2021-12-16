using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anabasis.MethodCache
{
	public class DefaultCacheKeyBuilder : ICacheKeyBuilder
	{
		private string GetParameterCacheKey(object parameter)
        {
			if (null == parameter) return "null";

			var type = parameter.GetType();

			return $"{type.Name}|{parameter}";
        }

        public string CreateKey(string methodName, params object[] parameters)
        {
			return
				methodName + "|" +
				parameters
					.Select(methodParameter => GetParameterCacheKey(methodParameter))
					.Aggregate((methodParameter1, methodParameter2) => $"{methodParameter1};{methodParameter2}");
		}
    }
}
