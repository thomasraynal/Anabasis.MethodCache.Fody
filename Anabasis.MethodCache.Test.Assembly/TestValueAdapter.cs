using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anabasis.MethodCache
{
    public class TestValueAdapter
    {
        [Cache]
        public IEnumerable<string> TestValueAdapterEnumerable(string a, string b)
        {
            Debug.WriteLine(nameof(TestValueAdapterEnumerable));

            yield return "a";
            yield return "b"; 
        }

        [Cache]
        public Stream TestValueAdapterStream(string a, string b)
        {
            Debug.WriteLine(nameof(TestValueAdapterStream));

            return new MemoryStream(Encoding.UTF8.GetBytes("test"));
        }

    }
}
