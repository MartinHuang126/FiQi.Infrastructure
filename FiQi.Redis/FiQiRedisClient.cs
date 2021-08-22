using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using FiQi.Util.Json;

namespace FiQi.Redis
{
    public class FiQiRedisClient : IFiQiRedisClient
    {
        readonly FiQiRedisOptions _redisoptions;
        ConcurrentDictionary<string, ConnectionMultiplexer> _connectDic;

        public FiQiRedisClient(
            IOptions<FiQiRedisOptions> redisoptions
            )
        {
            _redisoptions = redisoptions.Value;

            _connectDic = new ConcurrentDictionary<string, ConnectionMultiplexer>();

            var connect = ConnectionMultiplexer.Connect(new ConfigurationOptions()
            {
                EndPoints = { { _redisoptions.Address } },
                Password = _redisoptions.Password,
                DefaultDatabase = _redisoptions.DefaultDatabase,
                ChannelPrefix = new RedisChannel(_redisoptions.Environment, RedisChannel.PatternMode.Auto)
            });

            _connectDic[_redisoptions.DatabaseName] = connect;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var value = await _connectDic[_redisoptions.DatabaseName].GetDatabase().StringGetAsync(AddPrefix(key));
            if (value.IsNull)
            {
                return default;
            }
            return FiQiJson.DeSerialize<T>(value);
        }

        public async Task<bool> GetLock<T>(string key, T token, TimeSpan activeTime)
        {
            return await _connectDic[_redisoptions.DatabaseName].GetDatabase().LockTakeAsync(AddPrefix(key), FiQiJson.Serialize(token), activeTime);
        }

        public Task<bool> IsExist(string key)
        {
            return Task.FromResult(GetDatabase(_redisoptions.DatabaseName).KeyExists(AddPrefix(key)));
        }

        public async Task<bool> ReleaseLock<T>(string key, T token)
        {
            return await GetDatabase(_redisoptions.DatabaseName).LockReleaseAsync(AddPrefix(key), FiQiJson.Serialize(token));
        }

        public Task<bool> RemoveAsync(string key)
        {
            return Task.FromResult(GetDatabase(_redisoptions.DatabaseName).KeyDelete(AddPrefix(key)));
        }

        public async Task<bool> SetAsync<T>(string key, T value)
        {
            return await GetDatabase(_redisoptions.DatabaseName).StringSetAsync(AddPrefix(key), FiQiJson.Serialize(value));
        }

        public async Task<bool> SetAsync<T>(string key, T value, TimeSpan activeTime)
        {
            return await GetDatabase(_redisoptions.DatabaseName).StringSetAsync(AddPrefix(key), FiQiJson.Serialize(value), activeTime);
        }

        private IDatabase GetDatabase(string dbName)
        {
            return _connectDic[dbName].GetDatabase();
        }

        private string AddPrefix(string key)
        {
            return _redisoptions.Environment + key;
        }
    }
}
