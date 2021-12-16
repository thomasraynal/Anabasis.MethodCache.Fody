using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anabasis.MethodCache.Fody
{
	//https://github.com/SpatialFocus/MethodCache.Fody/blob/master/src/SpatialFocus.MethodCache.Fody/Extensions/CecilExtension.cs
	public static class CecilExtension
	{
		public static MethodReference MakeGeneric(this MethodReference method, References references, params TypeReference[] arguments)
		{

			var genericTypeReference = new GenericInstanceMethod(method);

			foreach (var argument in arguments)
			{
				genericTypeReference.GenericArguments.Add(argument);
			}

			return genericTypeReference;
		}
	}
}
