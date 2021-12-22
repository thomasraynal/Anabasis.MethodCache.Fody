using Fody;
using System;
using System.Collections.Generic;

namespace Anabasis.MethodCache.Fody
{
    public partial class ModuleWeaver : BaseModuleWeaver
    {
        public override void Execute()
        {
            var references = Fody.References.Init(this);

            var weavingCandidates = ModuleDefinition.GetWeavingCandidates(references);
    
            foreach(var weavingCandidate in weavingCandidates)
            {
                MethodCache.WeaveMethod(ModuleDefinition, weavingCandidate.MethodDefinition, references);
            }

        }

        public override IEnumerable<string> GetAssembliesForScanning()
        {
            yield return "netstandard";
            yield return "mscorlib";
            yield return "System.Threading.Tasks.Extensions";
            yield return "Anabasis.MethodCache";
        }
    }
}
