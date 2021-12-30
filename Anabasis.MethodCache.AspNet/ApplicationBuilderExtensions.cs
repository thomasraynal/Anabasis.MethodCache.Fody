using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Anabasis.MethodCache.AspNet
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseMethodCache(this IApplicationBuilder applicationBuilder, params IValueAdapter[] valueAdapters)
        {
            var cacheKeyBuilder = applicationBuilder.ApplicationServices.GetService<ICacheKeyBuilder>();
            var cachingBackend = applicationBuilder.ApplicationServices.GetService<ICachingBackend>();
            
            if (null != cachingBackend)
                CachingServices.Backend = cachingBackend;

            if (null != cacheKeyBuilder)
                CachingServices.KeyBuilder = cacheKeyBuilder;

            foreach(var valueAdapter in valueAdapters)
            {
                CachingServices.Backend.SetValueAdapter(valueAdapter);
            }

            return applicationBuilder;
        }
    }
}
