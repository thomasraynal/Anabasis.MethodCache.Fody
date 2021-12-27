using Anabasis.MethodCache.Test.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anabasis.MethodCache
{

    public class TestIFormattable
    {
        [Cache]
        public string TestingIFormattable(int a, Item b)
        {
            return $"{a}{b}";
        }

    }
}
