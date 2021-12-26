using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anabasis.MethodCache
{
    public class TestCacheExpiration
    {
        [Cache(AbsoluteExpirationRelativeToNowInMilliseconds = 1000)]
        public string TestAbsoluteExpiration(int a, int b)
        {
            return $"{a + b}";
        }

        [Cache(SlidingExpirationInMilliseconds = 1000)]
        public string TestSlidingExpiration(int a, int b)
        {
            return $"{a + b}";
        }

    }
}
