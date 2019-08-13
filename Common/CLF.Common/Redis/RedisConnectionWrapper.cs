using CLF.Common.Caching;
using CLF.Common.Configuration;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace CLF.Common.Redis
{
    public class RedisConnectionWrapper : IRedisConnectionWrapper, ILocker
    {
        private readonly AppConfig _config;

        private readonly object _lock = new object();
        private volatile ConnectionMultiplexer _connection;
        private readonly Lazy<string> _connectionString;
        private volatile RedLockFactory _redisLockFactory;

        public RedisConnectionWrapper(AppConfig config)
        {
            _config = config;
            _connectionString = new Lazy<string>(GetConnectionString);
            _redisLockFactory = CreateRedisLockFactory();
        }

        #region Utilities

        /// <summary>
        /// 获取redis服务器链接
        /// </summary>
        /// <returns></returns>
        protected string GetConnectionString()
        {
            return _config.RedisConnectionString;
        }

        /// <summary>
        /// 获取redis数据库链接
        /// </summary>
        /// <returns></returns>
        protected ConnectionMultiplexer GetConnection()
        {
            if (_connection != null && _connection.IsConnected) return _connection;

            lock (_lock)
            {
                if (_connection != null && _connection.IsConnected) return _connection;

                _connection?.Dispose();

                _connection = ConnectionMultiplexer.Connect(_connectionString.Value);
            }

            return _connection;
        }

        /// <summary>
        /// 创建redis分布式锁工厂方法
        /// </summary>
        /// <returns></returns>
        protected RedLockFactory CreateRedisLockFactory()
        {
            var configurationOptions = ConfigurationOptions.Parse(_connectionString.Value);
            var redLockEndPoints = GetEndPoints().Select(endPoint => new RedLockEndPoint
            {
                EndPoint = endPoint,
                Password = configurationOptions.Password,
                Ssl = configurationOptions.Ssl,
                RedisDatabase = configurationOptions.DefaultDatabase,
                ConfigCheckSeconds = configurationOptions.ConfigCheckSeconds,
                ConnectionTimeout = configurationOptions.ConnectTimeout,
                SyncTimeout = configurationOptions.SyncTimeout
            }).ToList();

            return RedLockFactory.Create(redLockEndPoints);
        }

        #endregion

        #region Methods

        /// <summary>
        /// 获取redis数据库
        /// </summary>
        /// <param name="db">数据库Id（0-15）</param>
        /// <returns></returns>
        public IDatabase GetDatabase(int db)
        {
            return GetConnection().GetDatabase(db);
        }

        /// <summary>
        /// 获取redis数据库服务器
        /// </summary>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public IServer GetServer(EndPoint endPoint)
        {
            return GetConnection().GetServer(endPoint);
        }

        public EndPoint[] GetEndPoints()
        {
            return GetConnection().GetEndPoints();
        }

        /// <summary>
        /// 清空redis数据库
        /// </summary>
        /// <param name="db">数据库Id</param>
        public void FlushDatabase(RedisDatabaseNumber db)
        {
            var endPoints = GetEndPoints();

            foreach (var endPoint in endPoints)
            {
                GetServer(endPoint).FlushDatabase((int)db);
            }
        }

        /// <summary>
        /// redis分布式锁
        /// </summary>
        /// <param name="resource">一般是缓存的Key</param>
        /// <param name="expirationTime">锁的自动过期时间</param>
        /// <param name="action">被锁住的方法</param>
        /// <returns>锁住方法成功，返回true；否则返回false</returns>
        public bool PerformActionWithLock(string resource, TimeSpan expirationTime, Action action)
        {
            using (var redisLock = _redisLockFactory.CreateLock(resource, expirationTime))
            {
                //没有获取到锁
                if (!redisLock.IsAcquired)
                    return false;

                action();

                return true;
            }
        }


        public void Dispose()
        {
            _connection?.Dispose();
            _redisLockFactory?.Dispose();
        }

        #endregion
    }
}
