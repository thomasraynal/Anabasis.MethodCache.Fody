using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Anabasis.MethodCache
{
    //https://stackoverflow.com/a/64291008
    public static class MemoryCacheExtensions
    {
        private static readonly Func<MemoryCache, object> GetEntriesCollection = Delegate.CreateDelegate(
            typeof(Func<MemoryCache, object>),
            typeof(MemoryCache).GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance).GetGetMethod(true),
            throwOnBindFailure: true) as Func<MemoryCache, object>;

        public static IEnumerable GetKeys(this IMemoryCache memoryCache) =>
            ((IDictionary)GetEntriesCollection((MemoryCache)memoryCache)).Keys;

        public static IEnumerable<T> GetKeys<T>(this IMemoryCache memoryCache) =>
            GetKeys(memoryCache).OfType<T>();
    }

    internal class DummyLogger : ILogger
    {
        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return false;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
        }
    }

    internal class DummyLoggerFactory : ILoggerFactory
    {
        public void AddProvider(ILoggerProvider provider)
        {
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new DummyLogger();
        }

        public void Dispose()
        {
        }
    }


    public class InMemoryCachingBackend : ICachingBackend
    {

        private readonly List<IValueAdapter> _valueAdapters = new List<IValueAdapter>()
        {
            new StreamValueAdapter()
        };

        public void SetValueAdapter<TAdapter>(TAdapter value) where TAdapter : IValueAdapter
        {
            _valueAdapters.Add(value);
        }

        private readonly ILoggerFactory _loggerFactory;
        private readonly MemoryCacheOptions _memoryCacheOptions;

        protected IMemoryCache Cache { get; private set; }

        public InMemoryCachingBackend(MemoryCacheOptions memoryCacheOptions = null, ILoggerFactory loggerFactory = null)
        {
            _loggerFactory = loggerFactory?? new DummyLoggerFactory();
            _memoryCacheOptions = memoryCacheOptions ?? new MemoryCacheOptions();

            Cache = new MemoryCache(_memoryCacheOptions, _loggerFactory);
        }

        public Task Clear(CancellationToken cancellationToken = default)
        {
            Cache.Dispose();

            Cache = new MemoryCache(_memoryCacheOptions, _loggerFactory);

            return Task.CompletedTask;
        }

        public Task Invalidate(string key)
        {
            Cache.Remove(key);

            return Task.CompletedTask;
        }

        public Task InvalidateWhenContains(string predicate, bool isCaseSensitive = true)
        {
            var entriesToRemove = new List<string>();

            foreach (string entry in Cache.GetKeys())
            {
          
                if (isCaseSensitive)
                {
                    if (entry.Contains(predicate))
                    {
                        entriesToRemove.Add(entry);
                    }
                }
                else
                {
                    if (entry.ToLowerInvariant().Contains(predicate.ToLowerInvariant()))
                    {
                        entriesToRemove.Add(entry);
                    }
                }

            }

            foreach(var entryToRemove in entriesToRemove)
            {
                Cache.Remove(entryToRemove);
            }

            return Task.CompletedTask;

        }

        public Task InvalidateWhenContains(string[] predicates, bool isCaseSensitive = true)
        {
            return Task.WhenAll(predicates.Select(predicate => InvalidateWhenContains(predicate, isCaseSensitive)).ToArray());

        }
        public Task InvalidateWhenStartWith(string[] predicates, bool isCaseSensitive = true)
        {
            return Task.WhenAll(predicates.Select(predicate => InvalidateWhenContains(predicate, isCaseSensitive)).ToArray());
        }

        public Task InvalidateWhenStartWith(string predicate, bool isCaseSensitive = true)
        {
            var entriesToRemove = new List<string>();

            foreach (string entry in Cache.GetKeys())
            {

                if (isCaseSensitive)
                {
                    if (entry.StartsWith(predicate))
                    {
                        entriesToRemove.Add(entry);
                    }
                }
                else
                {
                    if (entry.ToLowerInvariant().StartsWith(predicate.ToLowerInvariant()))
                    {
                        entriesToRemove.Add(entry);
                    }
                }

            }

            foreach (var entryToRemove in entriesToRemove)
            {
                Cache.Remove(entryToRemove);
            }

            return Task.CompletedTask;
        }

        public void SetValue<TItem>(string key, TItem value, long absoluteExpirationRelativeToNowInMilliseconds = 0, long slidingExpirationInMilliseconds = 0)
        {
            object storedValue = value;

            var valueAdapter = _valueAdapters.FirstOrDefault(valueAdapter => valueAdapter.ValueAdapterType == typeof(TItem));

            if (null != valueAdapter)
            {
                var typedAdapter = (IValueAdapter<TItem>)valueAdapter;

                storedValue = typedAdapter.GetStoredValue(value);
            }

            Cache.Set(key, storedValue, new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = default == absoluteExpirationRelativeToNowInMilliseconds ? null : TimeSpan.FromMilliseconds(absoluteExpirationRelativeToNowInMilliseconds),
                SlidingExpiration = default == slidingExpirationInMilliseconds ? null : TimeSpan.FromMilliseconds(slidingExpirationInMilliseconds)
            });

        }

        public bool TryGetValue<TItem>(string key, out TItem value)
        {

            var valueAdapter = _valueAdapters.FirstOrDefault(valueAdapter => valueAdapter.ValueAdapterType == typeof(TItem));

            if (null != valueAdapter)
            {
                var hasValue = Cache.TryGetValue(key, out object obj);

                if (!hasValue)
                {
                    value = default;
                    return false;
                }

                var typedAdapter = (IValueAdapter<TItem>)valueAdapter;

                value = typedAdapter.GetExposedValue(obj);

                return true;
            }

            return Cache.TryGetValue(key, out value);
        }

    }
}

