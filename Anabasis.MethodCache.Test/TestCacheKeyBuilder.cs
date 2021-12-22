using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anabasis.MethodCache.Test
{
	public class TestCacheKeyBuilder : ICacheKeyBuilder
	{
		private string GetParameterCacheKey(string parameterName, object parameterValue)
		{
			return $"{parameterName}|{parameterValue ?? "null"}";
		}

		public string CreateKey(string methodName, string[] argumentNames = null, object[] argumentValues = null)
		{

			var key = methodName;

			if (null == argumentNames) return key;

			key += "|";

			var valueIndex = 0;

			foreach (var argumentName in argumentNames)
			{
				var argumentValue = argumentValues[valueIndex];

				key += GetParameterCacheKey(argumentName, argumentValue);
				key += ";";
				valueIndex++;
			}

			return key.TrimEnd(';');

		}
	}
}