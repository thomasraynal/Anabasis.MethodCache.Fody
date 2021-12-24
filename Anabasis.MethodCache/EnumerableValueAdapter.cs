using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anabasis.MethodCache
{
    public class EnumerableValueAdapter<TItem> : ValueAdapter<IEnumerable<TItem>>
    {
        public override IEnumerable<TItem> GetExposedValue(object storedValue)
        {
            return (IEnumerable<TItem>)storedValue;
        }

        public override object GetStoredValue(IEnumerable<TItem> value)
        {
            return value.ToArray();
        }
    }
}
