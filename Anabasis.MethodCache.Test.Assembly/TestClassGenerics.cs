using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Anabasis.MethodCache.Test
{
    public class TestClassGenererics<TItem>
    {
        [Cache]
        public string TestGenerics(TItem a, TItem b)
        {
            Debug.WriteLine(nameof(TestGenerics));

            return $"{a}{b}";
        }

        [Cache]
        public string TestMethodGenerics<TItem2>(TItem2 a, TItem2 b)
        {
            Debug.WriteLine(nameof(TestMethodGenerics));

            return $"{a}{b}";
        }

        [Cache]
        public string TestMethodGenerics2<TItem2>(TItem2 a, TItem b)
        {
            Debug.WriteLine(nameof(TestMethodGenerics2));

            return $"{a}{b}";
        }

    }
}
