using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anabasis.MethodCache
{
	public class DefaultCacheKeyBuilder : ICacheKeyBuilder
	{

		private const string NullStringValue = "null";

		private string GetParameterCacheKey(string parameterName, object parameterValue)
		{
			if (null == parameterValue)
			{
				return $"{parameterName}|{NullStringValue}";
			}

			string parameterValueAsString;

			if (parameterValue is IMemoryCacheFormattable)
			{

				var stringBuilder = new StringBuilder();

				(parameterValue as IMemoryCacheFormattable).Format(stringBuilder);

				parameterValueAsString = stringBuilder.ToString();

			}
			else
			{
				parameterValueAsString = parameterValue.ToString();
			}

			return $"{parameterName}|{ parameterValueAsString}";

		}

		public string CreateKey(string methodName, string[] argumentNames = null, object[] argumentValues = null)
		{

			var key = methodName;

			if (null == argumentNames) return key;

			key += "|";

			if (null == argumentNames || argumentNames.Length == 0)
				return key.TrimEnd('|');

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
