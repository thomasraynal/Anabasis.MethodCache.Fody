# Anabasis.MethodCache.Fody

A method cache [Fody](https://github.com/Fody/Home/) weaver which emulate [PostSharp Caching](https://doc.postsharp.net/caching) API. 
Cache key builder and cache store are implementations so the weaving is much about fowarding the method context to them, allowing maximum flexibility. It handles Task, ValueTask, Stream and IEnumerable<T> (with a IValueAdapter<T> to be provided) cache values and have an [aspnet DI API](https://github.com/thomasraynal/Anabasis.MethodCache.Fody/blob/master/Anabasis.MethodCache.AspNet.Tests/TestApplicationBuilder.cs). See [the test cover for specific examples](https://github.com/thomasraynal/Anabasis.MethodCache.Fody/tree/master/Anabasis.MethodCache.Test). Default cache store implementation use .NET MemoryCache.


**Anabasis.MethodCache.Fody**

[![NuGet](https://img.shields.io/nuget/v/Anabasis.MethodCache.Fody.svg)](https://www.nuget.org/packages/Anabasis.MethodCache.Fody)

**Anabasis.MethodCache.AspNet**

[![NuGet](https://img.shields.io/nuget/v/Anabasis.MethodCache.AspNet.svg)](https://www.nuget.org/packages/Anabasis.MethodCache.AspNet)


[See the samples for some idea of what the API provide](https://github.com/thomasraynal/Anabasis.MethodCache.Fody/tree/master/Anabasis.MethodCache.Samples). 
  
Another go at method caching weaving using IMemoryCache => [SpatialFocus.MethodCache.Fody](https://github.com/SpatialFocus/MethodCache.Fody)
  
## Usage

See also [Fody usage](https://github.com/Fody/Home/blob/master/pages/usage.md).

### NuGet installation

```powershell
PM> Install-Package Fody
PM> Install-Package Anabasis.MethodCache.Fody
  
### Add to FodyWeavers.xml

Add `<Anabasis.MethodCache/>` to [FodyWeavers.xml](https://github.com/Fody/Home/blob/master/pages/usage.md#add-fodyweaversxml)

```xml
<Weavers>
    <Anabasis.MethodCache/>
</Weavers>
```

## Overview

Before code:

```csharp
public class Sample
{
    [Cache]
    public string SampleTest(int a, string b)
    {
        return a + b;
    }
}
```

What gets compiled

```csharp

public class Sample
{
    [Cache]
    public string SampleTest(int a, string b)
    {
        var cacheKey = CachingServices.KeyBuilder.CreateKey("SampleNamespace.Sample.SampleTest", new[] { "a", "b" }, new object[] { a, b });

        if (CachingServices.Backend.TryGetValue<string>(cacheKey, out var cachedValue))
        {
            return cachedValue;
        }

        var result = a + b;

        CachingServices.Backend.SetValue(cacheKey, result);

        return result;

    }
}
```
