using System;

namespace Anabasis.MethodCache
{
    public interface IValueAdapter
    {
        Type ValueAdapterType { get; }
    }

    public interface IValueAdapter<TValue> : IValueAdapter
    {
        TValue GetExposedValue(object storedValue);
        object GetStoredValue(TValue value);
    }
}