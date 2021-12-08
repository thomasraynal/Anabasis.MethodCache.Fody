using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anabasis.MemoryCache.Fody
{
	public static class ModuleDefinitionExtension
	{
		public static ICollection<WeavingCandidate> GetWeavingCandidates(this ModuleDefinition moduleDefinition, References references)
		{
			if (moduleDefinition == null)
			{
				throw new ArgumentNullException(nameof(moduleDefinition));
			}

			if (references == null)
			{
				throw new ArgumentNullException(nameof(references));
			}

			var weavingCandidates = new List<WeavingCandidate>();

			foreach (var typeDefinition in moduleDefinition.Types)
			{
				var typeCandidates = typeDefinition.Methods.Where(methodDefinition => methodDefinition.HasCacheAttribute(references))
														   .Select(methodDefinition => new WeavingCandidate(typeDefinition, methodDefinition))
														   .ToArray();

				weavingCandidates.AddRange(typeCandidates);

			}

			return weavingCandidates;
		}
	}
}
