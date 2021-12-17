using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Anabasis.MethodCache.Test
{
    public class TestClassSimple
    {
      
        public string TestNoCacheMethod(int a, int b)
        {
            Debug.WriteLine(nameof(TestNoCacheMethod));

            return $"{a + b}";
        }

        [Cache]
        public string TestValueTypeMethod(int a, int b)
        {
            Debug.WriteLine(nameof(TestValueTypeMethod));

            return $"{a + b}";
        }

        [Cache]
        public string TestReferenceTypeMethod(string a, string b)
        {
            Debug.WriteLine(nameof(TestReferenceTypeMethod));

            return a + b;
        }

        [Cache]
        public string TestReferenceTypeMethod2(object a, object b)
        {
            Debug.WriteLine(nameof(TestReferenceTypeMethod));

            return a.ToString() + b.ToString();
        }

    }
}
