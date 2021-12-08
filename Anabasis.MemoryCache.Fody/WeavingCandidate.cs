using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Text;

namespace Anabasis.MemoryCache.Fody
{
	public class WeavingCandidate
	{
		public WeavingCandidate(TypeDefinition classDefinition, MethodDefinition methodDefinition)
		{
			ClassDefinition = classDefinition;
			MethodDefinition = methodDefinition;
		}

		public TypeDefinition ClassDefinition { get; }

		public MethodDefinition MethodDefinition { get; }
	}
}
