using Mono.Cecil;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Anabasis.MethodCache.Fody
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

            var cachingServicesTypeDefinition = moduleWeaver.FindTypeDefinition("Anabasis.MethodCache.CachingServices");
            references.CachingServicesTypeReference = moduleWeaver.ModuleDefinition.ImportReference(cachingServicesTypeDefinition);

            var cacheKeyBuilderTypeDefinition = moduleWeaver.FindTypeDefinition("Anabasis.MethodCache.ICacheKeyBuilder");
            references.ICacheKeyBuilderTypeReference = moduleWeaver.ModuleDefinition.ImportReference(cacheKeyBuilderTypeDefinition);

            var cachingBackendTypeDefinition = moduleWeaver.FindTypeDefinition("Anabasis.MethodCache.ICachingBackend");
            references.ICachingBackendTypeReference = moduleWeaver.ModuleDefinition.ImportReference(cachingBackendTypeDefinition);

            var getCacheKeyBuilderTypeDefinition = cachingServicesTypeDefinition.Properties.Single(propertyDefinition=> propertyDefinition.Name == "KeyBuilder").GetMethod;
            references.GetCacheKeyBuilderTypeReference = moduleWeaver.ModuleDefinition.ImportReference(getCacheKeyBuilderTypeDefinition);

            var getBackendTypeDefinition = cachingServicesTypeDefinition.Properties.Single(propertyDefinition => propertyDefinition.Name == "Backend").GetMethod;
            references.GetBackendTypeReference = moduleWeaver.ModuleDefinition.ImportReference(getBackendTypeDefinition);

            var createKeyMethodDefinition = cacheKeyBuilderTypeDefinition.Methods.Single(methodDefinition => methodDefinition.Name == "CreateKey");
            references.CreateKeyMethodReference = moduleWeaver.ModuleDefinition.ImportReference(createKeyMethodDefinition);

            var tryGetValueMethodDefinition = cachingBackendTypeDefinition.Methods.Single(methodDefinition => methodDefinition.Name == "TryGetValue");
            references.TryGetValueMethodReference = moduleWeaver.ModuleDefinition.ImportReference(tryGetValueMethodDefinition);

            var setValueMethodDefinition = cachingBackendTypeDefinition.Methods.Single(methodDefinition => methodDefinition.Name == "SetValue");
            references.SetValueMethodReference = moduleWeaver.ModuleDefinition.ImportReference(setValueMethodDefinition);

            var cacheAttributeType = moduleWeaver.FindTypeDefinition("Anabasis.MethodCache.CacheAttribute");
			references.CacheAttributeType = moduleWeaver.ModuleDefinition.ImportReference(cacheAttributeType);

            references.DebugWriteLineStringMethodReference = moduleWeaver.ModuleDefinition.ImportReference(typeof(System.Diagnostics.Debug)
                                            .GetMethods()
                                            .First(methodInfo => methodInfo.Name == nameof(System.Diagnostics.Debug.WriteLine) &&
                                                                 methodInfo.GetParameters().Length == 1 &&
                                                                 methodInfo.GetParameters()[0].ParameterType.FullName == typeof(string).FullName));

            var compilerGeneratedAttributeType = moduleWeaver.FindTypeDefinition(typeof(CompilerGeneratedAttribute).FullName);
            references.CompilerGeneratedAttributeType = moduleWeaver.ModuleDefinition.ImportReference(compilerGeneratedAttributeType);

            var taskTypeReference = moduleWeaver.FindTypeDefinition(typeof(Task).FullName);
            references.TaskTypeReference = moduleWeaver.ModuleDefinition.ImportReference(taskTypeReference);

            return references;
		}

        public TypeReference CompilerGeneratedAttributeType { get; set; }
        public TypeReference CacheAttributeType { get; set; }
        public MethodReference DebugWriteLineStringMethodReference { get; private set; }
        public MethodReference CreateKeyMethodReference { get; private set; }
        public ModuleWeaver ModuleWeaver { get; }
        public TypeReference CachingServicesTypeReference { get; private set; }
        public TypeReference ICacheKeyBuilderTypeReference { get; private set; }
        public MethodReference GetCacheKeyBuilderTypeReference { get; private set; }
        public TypeReference ICachingBackendTypeReference { get; private set; }
        public MethodReference TryGetValueMethodReference { get; private set; }
        public MethodReference GetTryGetValue(TypeReference type) => ModuleWeaver.ModuleDefinition.ImportReference(TryGetValueMethodReference.MakeGeneric(type));
        public MethodReference GetSetValue(TypeReference type) => ModuleWeaver.ModuleDefinition.ImportReference(SetValueMethodReference.MakeGeneric(type));
        public MethodReference GetBackendTypeReference { get; private set; }
        public MethodReference SetValueMethodReference { get; private set; }
        public TypeReference TaskTypeReference { get; private set; }
    }

}

