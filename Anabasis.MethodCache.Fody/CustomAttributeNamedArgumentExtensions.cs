using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anabasis.MethodCache.Fody
{
    public static class CustomAttributeNamedArgumentExtensions
    {
        public static bool IsNull(this CustomAttributeNamedArgument customAttributeNamedArgument)
        {
            return customAttributeNamedArgument.Name == null;
        }
    }
}
