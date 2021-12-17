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
        public async Task<string> TestGenerics(TItem a, TItem b)
        {
            await Task.Delay(500);

            Task<string> t = new Task<string>(()=> "");

            var ff = CachingServices.KeyBuilder.CreateKey("sdf", a, b);

            var result = $"{a}{b}";

            CachingServices.Backend.SetValue("sdf", result);

            return ff;
        }
    }
}
