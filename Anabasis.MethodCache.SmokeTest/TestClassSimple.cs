using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anabasis.MethodCache.SmokeTest
{
    public class TestClassSimple
    {

        public string TestNoCacheMethod(int a, int b)
        {
            return $"{a + b}";
        }

        [Cache]
        public string TestValueTypeMethod(int a, int b)
        {
            return $"{a + b}";
        }

        [Cache]
        public string TestReferenceTypeMethod(string a, string b)
        {
            return a + b;
        }

        [Cache]
        public string TestReferenceTypeMethod2(object a, object b)
        {
            return a.ToString() + b.ToString();
        }

    }
}
