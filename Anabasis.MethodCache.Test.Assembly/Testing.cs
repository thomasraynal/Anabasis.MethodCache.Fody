using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anabasis.MethodCache.Test
{
    public class Testing<TItem>
    {
        [Cache]
        public string TestGenerics(TItem a, TItem b)
        {
            var ff = CachingServices.KeyBuilder.CreateKey("sdf", a, b);

            return $"{a}{b}";
        }
    }
}
