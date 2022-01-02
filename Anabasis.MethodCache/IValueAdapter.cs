using System;

namespace Anabasis.MethodCache
{
    public interface IValueAdapter
    {
        bool CanHandle(Type type);
    }

    public interface IValueAdapter<TValue> : IValueAdapter
    {
        TValue GetExposedValue(object storedValue);
        object GetStoredValue(TValue value);
    }
}