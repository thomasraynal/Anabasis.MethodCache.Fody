using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anabasis.MethodCache.Test
{
    public class TestClassTask
    {

        //[Cache]
        //public async Task TestNoReturnValue(int a, int b)
        //{
        //    Debug.WriteLine(nameof(TestValueTypeMethod));

        //    await Task.Delay(10);
        //}

        [Cache]
        public async Task<string> TestValueTypeMethod(int a, int b)
        {
            Debug.WriteLine(nameof(TestValueTypeMethod));

            await Task.Delay(10);

            return $"{a + b}";
        }

        [Cache]
        public async Task<string> TestReferenceTypeMethod(string a, string b)
        {
            Debug.WriteLine(nameof(TestReferenceTypeMethod));

            await Task.Delay(10);

            return a + b;
        }

        [Cache]
        public async Task<string> TestReferenceTypeMethod2(object a, object b)
        {
            Debug.WriteLine(nameof(TestReferenceTypeMethod));

            await Task.Delay(10);

            return a.ToString() + b.ToString();
        }

        [Cache]
        public async Task<string> TestGenerics<TItem>(TItem a, TItem b)
        {
            Debug.WriteLine(nameof(TestGenerics));

            await Task.Delay(10);

            return $"{a}{b}";
        }
    }
}
