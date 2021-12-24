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
        public IEnumerable<string> TestGenerics(TItem a, TItem b)
        {

            yield return "a";
            yield return "B";

            //CachingServices.Backend.SetValue("sdf", result);

        }
    }
}
