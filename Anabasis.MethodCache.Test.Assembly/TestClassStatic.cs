using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anabasis.MethodCache
{
    public static class TestClassStatic
    {

        [Cache]
        public static string TestStaticClassMethod(int a, int b)
        {
            Debug.WriteLine(nameof(TestStaticClassMethod));

            return $"{a + b}";
        }
    }

    public class TestMethodStatic
    {

        [Cache]
        public static string TestStaticMethod(int a, int b)
        {
            Debug.WriteLine(nameof(TestStaticMethod));

            return $"{a + b}";
        }

    }
}
