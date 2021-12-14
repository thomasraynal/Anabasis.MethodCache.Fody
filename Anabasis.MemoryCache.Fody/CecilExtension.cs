using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Text;

namespace Anabasis.MemoryCache.Fody
{
	//https://github.com/SpatialFocus/MethodCache.Fody/blob/master/src/SpatialFocus.MethodCache.Fody/Extensions/CecilExtension.cs
	public static class CecilExtension
	{
		public static MethodReference MakeGeneric(this MethodReference method, params TypeReference[] arguments)
		{
			if (method == null)
			{
				throw new ArgumentNullException(nameof(method));
			}

			if (method.GenericParameters.Count != arguments.Length)
			{
				throw new ArgumentException("Invalid number of generic type arguments supplied");
			}

			if (arguments.Length == 0)
			{
				return method;
			}

			var genericTypeReference = new GenericInstanceMethod(method);

			foreach (TypeReference argument in arguments)
			{
				genericTypeReference.GenericArguments.Add(argument);
			}

			return genericTypeReference;
		}
	}
}
