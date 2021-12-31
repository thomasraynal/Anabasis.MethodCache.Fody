using System.Diagnostics;
using System.Threading.Tasks;

namespace Anabasis.MethodCache.Test
{
    public class TestClassValueTask
    {


        [Cache]
        public async ValueTask TestNoReturnValue(int a, int b)
        {
            Debug.WriteLine(nameof(TestNoReturnValue));

            await Task.Delay(10);
        }

        [Cache]
        public async ValueTask<string> TestValueTypeMethod(int a, int b)
        {
            Debug.WriteLine(nameof(TestValueTypeMethod));

            await Task.Delay(10);

            return $"{a + b}";
        }

        [Cache]
        public async ValueTask<string> TestReferenceTypeMethod(string a, string b)
        {
            Debug.WriteLine(nameof(TestReferenceTypeMethod));

            await Task.Delay(10);

            return a + b;
        }

        [Cache]
        public async ValueTask<string> TestReferenceTypeMethod2(object a, object b)
        {
            Debug.WriteLine(nameof(TestReferenceTypeMethod));

            await Task.Delay(10);

            return a.ToString() + b.ToString();
        }

        [Cache]
        public async ValueTask<string> TestGenerics<TItem>(TItem a, TItem b)
        {
            Debug.WriteLine(nameof(TestGenerics));

            await Task.Delay(10);

            return $"{a}{b}";
        }
    }
}
