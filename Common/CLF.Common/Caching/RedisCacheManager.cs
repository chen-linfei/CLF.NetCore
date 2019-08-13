using CLF.Common.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using StackExchange.Redis;
using CLF.Common.Configuration;
using System.Net;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;

namespace CLF.Common.Caching
{
    public partial class RedisCacheManager : IStaticCacheManager
    {
        private readonly ICacheManager _perRequestCacheManager;
        private readonly IRedisConnectionWrapper _connectionWrapper;
        private readonly IDatabase _db;

        public RedisCacheManager(ICacheManager perRequestCacheManager,
            IRedisConnectionWrapper connectionWrapper,
            AppConfig config)
        {
            if (string.IsNullOrEmpty(config.RedisConnectionString))
                throw new Exception("Redis connection string is empty");

            _perRequestCacheManager = perRequestCacheManager;

            _connectionWrapper = connectionWrapper;

            _db = _connectionWrapper.GetDatabase(config.RedisDatabaseId ?? (int)RedisDatabaseNumber.Cache);
        }


        #region Utilities

        protected virtual IEnumerable<RedisKey> GetKeys(EndPoint endPoint, string prefix = null)
        {
            var server = _connectionWrapper.GetServer(endPoint);
            var keys = server.Keys(_db.Database, string.IsNullOrEmpty(prefix) ? null : $"{prefix}*");
            keys = keys.Where(key => !key.ToString().Equals(CachingDefaultSettings.RedisDataProtectionKey, StringComparison.OrdinalIgnoreCase));
            return keys;
        }

        protected virtual async Task<T> GetAsync<T>(string key)
        {
            //如果本地缓存中没有缓存项，就连接redis，以提高性能
            if (_perRequestCacheManager.IsSet(key))
                return _perRequestCacheManager.Get(key, () => default(T), 0);

            //从redis中获取缓存数据
            var serializedItem = await _db.StringGetAsync(key);
            if (!serializedItem.HasValue)
                return default(T);

            var item = JsonConvert.DeserializeObject<T>(serializedItem);
            if (item == null)
                return default(T);

            //写入本地缓存
            _perRequestCacheManager.Set(key, item, 0);

            return item;
        }

        
        protected virtual async Task SetAsync(string key, object data, int cacheTime)
        {
            if (data == null)
                return;

            var expiresIn = TimeSpan.FromMinutes(cacheTime);
            var serializedItem = JsonConvert.SerializeObject(data);
            await _db.StringSetAsync(key, serializedItem, expiresIn);
        }

        protected virtual async Task<bool> IsSetAsync(string key)
        {
            //先读本地缓存，没有再读redis
            if (_perRequestCacheManager.IsSet(key))
                return true;

            return await _db.KeyExistsAsync(key);
        }

        #endregion

        #region Methods

        /// <summary>
        /// 获取缓存数据，不存缓存数据先获取数据再写入缓存
        /// </summary>
        public async Task<T> GetAsync<T>(string key, Func<Task<T>> acquire, int? cacheTime = null)
        {
            if (await IsSetAsync(key))
                return await GetAsync<T>(key);

            var result = await acquire();

            if ((cacheTime ?? CachingDefaultSettings.CacheTime) > 0)
                await SetAsync(key, result, cacheTime ?? CachingDefaultSettings.CacheTime);

            return result;
        }


        public virtual T Get<T>(string key)
        {
            if (_perRequestCacheManager.IsSet(key))
                return _perRequestCacheManager.Get(key, () => default(T), 0);

            var serializedItem = _db.StringGet(key);
            if (!serializedItem.HasValue)
                return default(T);

            //序列化数据
            var item = JsonConvert.DeserializeObject<T>(serializedItem);
            if (item == null)
                return default(T);

            _perRequestCacheManager.Set(key, item, 0);

            return item;
        }


        public virtual T Get<T>(string key, Func<T> acquire, int? cacheTime = null)
        {
            if (IsSet(key))
                return Get<T>(key);

            var result = acquire();

            if ((cacheTime ?? CachingDefaultSettings.CacheTime) > 0)
                Set(key, result, cacheTime ?? CachingDefaultSettings.CacheTime);

            return result;
        }

        public virtual void Set(string key, object data, int cacheTime)
        {
            if (data == null)
                return;

            var expiresIn = TimeSpan.FromMinutes(cacheTime);
            var serializedItem = JsonConvert.SerializeObject(data);
            _db.StringSet(key, serializedItem, expiresIn);
        }

        public virtual bool IsSet(string key)
        {
            if (_perRequestCacheManager.IsSet(key))
                return true;

            return _db.KeyExists(key);
        }

        public virtual void Remove(string key)
        {
            if (key.Equals(CachingDefaultSettings.RedisDataProtectionKey, StringComparison.OrdinalIgnoreCase))
                return;

            _db.KeyDelete(key);
            _perRequestCacheManager.Remove(key);
        }


        public virtual void RemoveByPrefix(string prefix)
        {
            _perRequestCacheManager.RemoveByPrefix(prefix);

            foreach (var endPoint in _connectionWrapper.GetEndPoints())
            {
                var keys = GetKeys(endPoint, prefix);

                _db.KeyDelete(keys.ToArray());
            }
        }

        public virtual void Clear()
        {
            foreach (var endPoint in _connectionWrapper.GetEndPoints())
            {
                var keys = GetKeys(endPoint).ToArray();
                foreach (var redisKey in keys)
                {
                    _perRequestCacheManager.Remove(redisKey.ToString());
                }
                _db.KeyDelete(keys);
            }
        }

        public virtual void Dispose()
        {
            //if (_connectionWrapper != null)
            //    _connectionWrapper.Dispose();
        }

        #endregion
    }
}
