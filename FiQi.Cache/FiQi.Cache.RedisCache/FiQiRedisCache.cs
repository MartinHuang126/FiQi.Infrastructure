using System;
using System.Threading.Tasks;
using FiQi.Cache.Abstract;
using FiQi.Redis;

namespace FiQi.Cache.FiQiRedisCache
{
    public class FiQiRedisCache : IFiQiCache
    {
        readonly IFiQiRedisClient _cache;
        public FiQiRedisCache(
            IFiQiRedisClient redisClient
            )
        {
            _cache = redisClient;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            return await _cache.GetAsync<T>(key);
        }

        public async Task<bool> IsExist(string key)
        {
            return await _cache.IsExist(key);
        }

        public async Task<bool> RemoveAsync(string key)
        {
            return await _cache.RemoveAsync(key);
        }

        public async Task<bool> SetAsync<T>(string key, T value, TimeSpan timeSpan)
        {
            return await _cache.SetAsync(key, value, timeSpan);
        }

        public async Task<bool> SetAsync<T>(string key, T value)
        {
            return await _cache.SetAsync(key, value);
        }
    }
}
