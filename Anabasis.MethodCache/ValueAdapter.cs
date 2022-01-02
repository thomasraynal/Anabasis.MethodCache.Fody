using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anabasis.MethodCache
{
    public abstract class ValueAdapter<TValue> : IValueAdapter<TValue>
    {

        public virtual bool CanHandle(Type type)
        {
            return typeof(TValue) == type;
        }

        public abstract TValue GetExposedValue(object storedValue);

        public abstract object GetStoredValue(TValue value);
    }
}
