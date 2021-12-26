using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anabasis.MethodCache.Fody
{
	//https://github.com/SpatialFocus/MethodCache.Fody/blob/master/src/SpatialFocus.MethodCache.Fody/Extensions/ModuleDefinitionExtension.cs
	public static class ModuleDefinitionExtension
	{
		public static ICollection<WeavingCandidate> GetWeavingCandidates(this ModuleDefinition moduleDefinition, References references)
		{
			var weavingCandidates = new List<WeavingCandidate>();

			foreach (var typeDefinition in moduleDefinition.Types)
			{
				var typeCandidates = typeDefinition.Methods.Where(methodDefinition => methodDefinition.HasCacheAttribute(references) && methodDefinition.IsEligibleForWeaving(references))
														   .Select(methodDefinition => new WeavingCandidate(typeDefinition, methodDefinition, references))
														   .ToArray();

				weavingCandidates.AddRange(typeCandidates);

			}

			return weavingCandidates;
		}
	}
}
