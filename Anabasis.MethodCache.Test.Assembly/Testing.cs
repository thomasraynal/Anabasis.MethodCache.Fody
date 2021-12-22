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

            var argumentNames = new string[] { "a", "b" };
            var argumentValues = new object[] { a, b };

            var ff = CachingServices.KeyBuilder.CreateKey("sdf", argumentNames, argumentValues);
            
            var result = $"{a}{b}";

            CachingServices.Backend.SetValue("sdf", result);

            return ff;
        }
    }
}
