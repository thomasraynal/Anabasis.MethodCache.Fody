using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anabasis.MethodCache.Fody
{
	//https://github.com/kswoll/sexy-proxy/blob/a485b620e76df70058fc51951c91c505cb5efee4/SexyProxy.Fody/CecilExtensions.cs#L324
	//https://github.com/SpatialFocus/MethodCache.Fody/blob/master/src/SpatialFocus.MethodCache.Fody/Extensions/CecilExtension.cs
	public static class CecilExtension
	{
		public static bool IsStateMachine(this TypeDefinition typeDefinition)
		{
			return typeDefinition.IsIAsyncStateMachine() &&
				typeDefinition.IsCompilerGenerated();
		}

		public static bool IsCompilerGenerated(this TypeDefinition typeDefinition)
		{
			return typeDefinition.CustomAttributes.Any(x => x.Constructor.DeclaringType.Name == "CompilerGeneratedAttribute");
		}

		public static bool IsIAsyncStateMachine(this TypeDefinition typeDefinition)
		{
			return typeDefinition.Interfaces.Any(x => x.InterfaceType.Name == "IAsyncStateMachine");
		}


		public static bool CompareTo(this TypeReference type, TypeReference compareTo)
		{
			return type.FullName == compareTo.FullName;
		}

		public static bool IsTaskT(this TypeReference type, References references)
		{
			var current = type;
			while (current != null)
			{
				if (current is GenericInstanceType && ((GenericInstanceType)current).Resolve().GetElementType().CompareTo(references.TaskGenericTypeReference))
					return true;
				current = current.Resolve().BaseType;
			}
			return false;
		}

		public static TypeReference GetTaskType(this TypeReference type, References references)
		{
			var current = type;
			while (current != null)
			{
				if (current is GenericInstanceType && ((GenericInstanceType)current).Resolve().GetElementType().CompareTo(references.TaskGenericTypeReference))
					return ((GenericInstanceType)current).GenericArguments.Single();
				current = current.Resolve().BaseType;
			}
			throw new Exception("Type " + type.FullName + " is not an instance of Task<T>");
		}


		public static MethodReference MakeGeneric(this MethodReference method, params TypeReference[] arguments)
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
