using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

            references.StringJoinMethodReference = moduleWeaver.ModuleDefinition.ImportReference(moduleWeaver.FindTypeDefinition(typeof(string).FullName)
                                                            .Methods.First(methodDefinition => methodDefinition.Name == "Join" &&
                                                                           methodDefinition.Parameters.Count == 2 && 
                                                                           methodDefinition.Parameters[0].ParameterType.FullName == typeof(string).FullName && 
                                                                           methodDefinition.Parameters[1].ParameterType.FullName == typeof(object[]).FullName));

            references.StringFormatMethodReference = moduleWeaver.ModuleDefinition.ImportReference(moduleWeaver.FindTypeDefinition(typeof(string).FullName)
                                                        .Methods.First(methodDefinition => methodDefinition.Name == "Format" &&
                                                                       methodDefinition.Parameters.Count == 2 &&
                                                                       methodDefinition.Parameters[0].ParameterType.FullName == typeof(string).FullName &&
                                                                       methodDefinition.Parameters[1].ParameterType.FullName == typeof(object[]).FullName));

            references.DebugWriteLineMethodReference = moduleWeaver.ModuleDefinition.ImportReference(typeof(System.Diagnostics.Debug)
                                                        .GetMethods()
                                                        .First(methodInfo => methodInfo.Name == nameof(System.Diagnostics.Debug.WriteLine) &&
                                                                       methodInfo.GetParameters().Length == 1 &&
                                                                      methodInfo.GetParameters()[0].ParameterType.FullName == typeof(string).FullName));
                                                                      //methodInfo.GetParameters()[1].ParameterType.FullName == typeof(object[]).FullName));


            return references;
		}

		public TypeReference CacheAttributeType { get; set; }
        public MethodReference StringJoinMethodReference { get; private set; }
        public MethodReference StringFormatMethodReference { get; private set; }
        public MethodReference DebugWriteLineMethodReference { get; private set; }
        public ModuleWeaver ModuleWeaver { get; }
    }

}

