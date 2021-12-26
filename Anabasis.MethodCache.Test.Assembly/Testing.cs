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
        public void TestGenerics(TItem a, TItem b)
        {
            CachingServices.Backend.SetValue("sdf", "a", 1L, 2L);

        }
    }
}
