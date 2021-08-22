using System;
using System.Threading.Tasks;
using FiQi.Cache.Abstract;
using Microsoft.Extensions.Caching.Memory;

namespace FiQi.Cache.MemoryCache
{
    public class FiQiMemoryCache : IFiQiCache
    {
        readonly IMemoryCache _cache;
        public FiQiMemoryCache(
            IMemoryCache memoryCache
            )
        {
            _cache = memoryCache;
        }

        public Task<T> GetAsync<T>(string key)
        {
            return Task.FromResult(_cache.Get<T>(key));
        }

        public Task<bool> IsExist(string key)
        {
            return Task.FromResult(_cache.TryGetValue(key, out _));
        }

        public Task<bool> RemoveAsync(string key)
        {
            _cache.Remove(key);
            return Task.FromResult(!_cache.TryGetValue(key, out _));
        }

        public Task<bool> SetAsync<T>(string key, T value, TimeSpan timeSpan)
        {
            _cache.Set(key, value, timeSpan);
            return Task.FromResult(_cache.TryGetValue(key, out T _));
        }

        public Task<bool> SetAsync<T>(string key, T value)
        {
            _cache.Set(key, value);
            return Task.FromResult(_cache.TryGetValue(key, out T _));
        }
    }
}
