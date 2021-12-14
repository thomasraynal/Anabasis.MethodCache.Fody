using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Anabasis.MemoryCache.Test
{
    public class Class1
    {
   
        public string TestMethodNotask(string a, string b)
        {

            var key = CachingServices.KeyBuilder.CreateKey("methodName", a, b);

            //if (CachingServices.Backend.TryGetValue(key, out string cacheValue))
            //{
            //    return cacheValue;
            //}


            //var key = "methodCache";

            //Debug.WriteLine(key);

            var value = a + b;

            CachingServices.Backend.SetValue(key, value);



            return a + b;
        }

        [Cache]
        public string TestMethod(string a, string b)
        {

            //var key = CachingServices.KeyBuilder.CreateKey("methodName", a, b);

            //if (CachingServices.Backend.TryGetValue(key, out string cacheValue))
            //{
            //    return cacheValue;
            //}


            //var value = a + b;

            //CachingServices.Backend.SetValue(key, value);

            return a + b;
        }
        
        public void TestMethod2()
        {

          

            CachingServices.Backend.TryGetValue("sfsq", out int value);
        }

    }
}
