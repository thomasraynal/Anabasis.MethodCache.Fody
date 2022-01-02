using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anabasis.MethodCache.Test
{
    public class Sample
    {
        [Cache]
        public string SampleTest(int a, string b)
        {
            var cacheKey = CachingServices.KeyBuilder.CreateKey(nameof(SampleTest), new[] { "a", "b" }, new object[] { a, b });

            if (CachingServices.Backend.TryGetValue<string>(cacheKey, out var cachedValue))
            {
                return cachedValue;
            }

            var result = a + b;

            CachingServices.Backend.SetValue(cacheKey, result);

            return result;

        }
    }
}
