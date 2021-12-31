using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Anabasis.MethodCache.AspNet.Tests
{
    public class TestApplicationBuilder
    {
        class DefaultStartup
        {
            public void ConfigureServices(IServiceCollection services)
            {
            }
            public void Configure(IApplicationBuilder app)
            {
                app.UseMethodCache();
            }

        }

        class CustomStartup
        {
            public void ConfigureServices(IServiceCollection services)
            {
                services.AddSingleton<ICacheKeyBuilder, TestCacheKeyBuilder>();
                services.AddSingleton<ICachingBackend, TestCachingBackend>();
            }

            public void Configure(IApplicationBuilder app)
            {
                app.UseMethodCache();
            }

        }

        class TestCacheKeyBuilder : ICacheKeyBuilder
        {
            public string CreateKey(string methodName, string[] argumentNames = null, object[] argumentValues = null)
            {
                throw new NotImplementedException();
            }
        }
        class TestCachingBackend : ICachingBackend
        {
            public Task Clear(CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task Invalidate(string key)
            {
                throw new NotImplementedException();
            }

            public Task InvalidateWhenContains(string predicate, bool isCaseSensitive = true)
            {
                throw new NotImplementedException();
            }

            public Task InvalidateWhenContains(string[] predicates, bool isCaseSensitive = true)
            {
                throw new NotImplementedException();
            }

            public Task InvalidateWhenStartWith(string[] predicates, bool isCaseSensitive = true)
            {
                throw new NotImplementedException();
            }

            public Task InvalidateWhenStartWith(string predicate, bool isCaseSensitive = true)
            {
                throw new NotImplementedException();
            }

            public void SetValue<TItem>(string key, TItem value, long absoluteExpirationRelativeToNowInMilliseconds, long slidingExpirationInMilliseconds)
            {
                throw new NotImplementedException();
            }

            public void SetValueAdapter<TAdapter>(TAdapter value) where TAdapter : IValueAdapter
            {
                throw new NotImplementedException();
            }

            public bool TryGetValue<TItem>(string key, out TItem value)
            {
                throw new NotImplementedException();
            }
        }


        [SetUp]
        public void Setup()
        {
            CachingServices.KeyBuilder = new DefaultCacheKeyBuilder();
            CachingServices.Backend = new InMemoryCachingBackend();
        }

        [Test]
        public void ShouldBuildWithDefaultSetup()
        {

            var builder = new WebHostBuilder()
                .UseStartup<DefaultStartup>();

            var _ = new TestServer(builder);

            Assert.NotNull(CachingServices.Backend);
            Assert.AreEqual(typeof(InMemoryCachingBackend), CachingServices.Backend.GetType());

            Assert.NotNull(CachingServices.KeyBuilder);
            Assert.AreEqual(typeof(DefaultCacheKeyBuilder), CachingServices.KeyBuilder.GetType());

        }

        [Test]
        public void ShouldBuildWithCustomSetup()
        {

            var builder = new WebHostBuilder()
                            .UseStartup<CustomStartup>();

            var _ = new TestServer(builder);

            Assert.NotNull(CachingServices.Backend);
            Assert.AreEqual(typeof(TestCachingBackend), CachingServices.Backend.GetType());

            Assert.NotNull(CachingServices.KeyBuilder);
            Assert.AreEqual(typeof(TestCacheKeyBuilder), CachingServices.KeyBuilder.GetType());
        }

    }
}
