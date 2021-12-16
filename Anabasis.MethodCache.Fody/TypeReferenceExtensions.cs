using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anabasis.MethodCache.Fody
{
    public static class TypeReferenceExtensions
    {
        public static bool IsTask(this TypeReference typeReference)
        {
            return typeReference.Name == "Task`1";
        }
    }
}
