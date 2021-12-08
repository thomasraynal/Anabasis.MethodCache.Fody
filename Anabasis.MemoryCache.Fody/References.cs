using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Text;

namespace Anabasis.MemoryCache.Fody
{
	public class References
	{
		protected References(ModuleWeaver moduleWeaver)
		{
			ModuleWeaver = moduleWeaver;
		}

		public static References Init(ModuleWeaver moduleWeaver)
		{
			var references = new References(moduleWeaver);

			var cacheAttributeType = moduleWeaver.FindTypeDefinition("Anabasis.MemoryCache.CacheAttribute");
			references.CacheAttributeType = moduleWeaver.ModuleDefinition.ImportReference(cacheAttributeType);

			return references;
		}

		public TypeReference CacheAttributeType { get; set; }

		public ModuleWeaver ModuleWeaver { get; }
    }

}

