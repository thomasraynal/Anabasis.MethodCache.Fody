using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Text;

namespace Anabasis.MethodCache.Fody
{
	public class WeavingCandidate
	{
		public WeavingCandidate(TypeDefinition classDefinition, MethodDefinition methodDefinition, References references)
		{
			ClassDefinition = classDefinition;
			MethodDefinition = methodDefinition;
			CacheAttribute = methodDefinition.GetCacheAttribute(references);
		}

		public CustomAttribute CacheAttribute { get; }

		public TypeDefinition ClassDefinition { get; }

		public MethodDefinition MethodDefinition { get; }
	}
}
