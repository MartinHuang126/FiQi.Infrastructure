using System;
using System.Threading.Tasks;
using FiQi.Cache.Abstract;
using StackExchange.Redis;

namespace FiQi.Redis
{
    /// <summary>
    /// FiQiRedis客户端
    /// </summary>
    public interface IFiQiRedisClient : IFiQiCache
    {
        /// <summary>
        /// 获取一个锁，key和value需要一致
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> GetLock<T>(string key, T token, TimeSpan activeTime);

        /// <summary>
        /// 释放一个锁，key和value需要一致
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> ReleaseLock<T>(string key, T token);
    }
}
