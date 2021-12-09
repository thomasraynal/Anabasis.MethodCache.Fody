using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Anabasis.MemoryCache.Test
{
    public class Class1
    {


        [Cache]
        public string TestMethod(string a, int b)
        {
            var key = string.Format(a, b);

            var value = a + b;

            CachingServices.DefaultBackend.Set(key, value);

            return value;
        }
        
        public void TestMethod2()
        {

            CachingServices.DefaultBackend.TryGetValue("sfsq", out int value);
        }

    }
}
