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

            var cachingServicesTypeDefinition = moduleWeaver.FindTypeDefinition("Anabasis.MemoryCache.CachingServices");
            references.CachingServicesTypeReference = moduleWeaver.ModuleDefinition.ImportReference(cachingServicesTypeDefinition);

            var cacheKeyBuilderTypeDefinition = moduleWeaver.FindTypeDefinition("Anabasis.MemoryCache.ICacheKeyBuilder");
            references.ICacheKeyBuilderTypeReference = moduleWeaver.ModuleDefinition.ImportReference(cacheKeyBuilderTypeDefinition);

            var getCacheKeyBuilderTypeDefinition = cachingServicesTypeDefinition.Properties.Single(propertyDefinition=> propertyDefinition.Name == "KeyBuilder").GetMethod;
            references.GetCacheKeyBuilderTypeReference = moduleWeaver.ModuleDefinition.ImportReference(getCacheKeyBuilderTypeDefinition);

            var createKeyMethod = cacheKeyBuilderTypeDefinition.Methods.Single(m => m.Name == "CreateKey");
            references.CreateKeyMethodReference = moduleWeaver.ModuleDefinition.ImportReference(createKeyMethod);

            var typeType = moduleWeaver.FindTypeDefinition("System.Type");
            var getTypeFromHandle = typeType.Methods
                .First(x => x.Name == "GetTypeFromHandle" &&
                            x.Parameters.Count == 1 &&
                            x.Parameters[0].ParameterType.Name == "RuntimeTypeHandle");

            references.GetTypeFromHandle = moduleWeaver.ModuleDefinition.ImportReference(getTypeFromHandle);


            //references.CreateKeyMethodReference = moduleWeaver.ModuleDefinition.ImportReference(typeof(System.Diagnostics.Debug)
            //                                .GetMethods()
            //                                .First(methodInfo => methodInfo.Name == nameof(System.Diagnostics.Debug.WriteLine) &&
            //                                               methodInfo.GetParameters().Length == 2 &&
            //                                              methodInfo.GetParameters()[0].ParameterType.FullName == typeof(string).FullName &&
            //                                              methodInfo.GetParameters()[1].ParameterType.FullName == typeof(object[]).FullName));


            //  var keyBuilderTypeDefinition = cachingServicesTypeDefinition.Properties.Single(x => createKeyMethodx.Name == "KeyBuilder").GetMethod;


            //   references.CachingServicesTypeReference = moduleWeaver.ModuleDefinition.ImportReference(cachingServicesTypeDefinition);


            //    references.CachingServicesTypeReference.pr

            ////var icacheKeyBuilderType = moduleWeaver.FindTypeDefinition("Anabasis.MemoryCache.ICacheKeyBuilder");
            ////references.ICacheKeyBuilderTypeTypeReference = moduleWeaver.ModuleDefinition.ImportReference(icacheKeyBuilderType);

            //var keyBuilderGetMethodDefinition = cachingServicesTypeDefinition.Properties.Single(x => x.Name == "KeyBuilder").GetMethod;
            //    references.KeyBuilderGetMethodReference = moduleWeaver.ModuleDefinition.ImportReference(keyBuilderGetMethodDefinition);

            //var keyBuilderCreateKeyMethodDefinition = keyBuilderGetMethodDefinition.Resolve().Methods.Single(x => x.Name == "CreateKey");
            //references.KeyBuilderCreateKeyMethodReference = moduleWeaver.ModuleDefinition.ImportReference(keyBuilderCreateKeyMethodDefinition);







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
                                                                       methodInfo.GetParameters().Length == 2 &&
                                                                      methodInfo.GetParameters()[0].ParameterType.FullName == typeof(string).FullName &&
                                                                      methodInfo.GetParameters()[1].ParameterType.FullName == typeof(object[]).FullName));


            return references;
		}

		public TypeReference CacheAttributeType { get; set; }
        public MethodReference StringJoinMethodReference { get; private set; }
        public MethodReference StringFormatMethodReference { get; private set; }
        public MethodReference DebugWriteLineMethodReference { get; private set; }
        public MethodReference CreateKeyMethodReference { get; private set; }
        public ModuleWeaver ModuleWeaver { get; }
        public TypeReference CachingServicesTypeReference { get; private set; }
        public TypeReference ICacheKeyBuilderTypeReference { get; private set; }
        public MethodReference KeyBuilderGetMethodReference { get; private set; }
        public MethodReference KeyBuilderCreateKeyMethodReference { get; private set; }
        public MethodReference GetCacheKeyBuilderTypeReference { get; private set; }
        public MethodReference GetTypeFromHandle { get; private set; }
    }

}

