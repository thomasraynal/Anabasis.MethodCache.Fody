using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anabasis.MethodCache
{
    public class StreamValueAdapter : ValueAdapter<Stream>
    {

        public override Stream GetExposedValue(object storedValue)
        {
            return new MemoryStream(storedValue as byte[]);
        }

        public override object GetStoredValue(Stream value)
        {
            using (var memoryStream = new MemoryStream())
            {
                value.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
