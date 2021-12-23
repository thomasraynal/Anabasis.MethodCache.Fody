using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anabasis.MethodCache
{
    public class TestNoKeyAttribute
    {
        [Cache]
        public string TestNoSecondParameter<TItem>(TItem a, [NoKey] TItem b)
        {
            Debug.WriteLine(nameof(TestNoKeyAttribute));

            return $"{a}{b}";
        }

        [Cache]
        public string TestNoFirstParameter<TItem>([NoKey] TItem a, TItem b)
        {
            Debug.WriteLine(nameof(TestNoKeyAttribute));

            return $"{a}{b}";
        }
    }
}
