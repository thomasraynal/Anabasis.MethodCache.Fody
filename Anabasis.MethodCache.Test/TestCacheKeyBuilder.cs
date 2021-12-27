using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anabasis.MethodCache.Test
{
	public class TestCacheKeyBuilder : ICacheKeyBuilder
	{

		private const string NullStringValue = "null";

		private string GetParameterCacheKey(string parameterName, object parameterValue)
		{
			if (null == parameterValue)
			{
				return $"{parameterName}|{NullStringValue}";
			}

            string parameterValueAsString;

            if (parameterValue is IFormattable)
			{

				var stringBuilder = new StringBuilder();

				(parameterValue as IFormattable).Format(stringBuilder);

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