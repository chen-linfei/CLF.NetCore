using System;
using System.Collections.Generic;
using System.Text;

namespace CLF.Common.Caching
{
    public interface ICacheManager : IDisposable
    {
        /// <summary>
        /// 从缓存获取数据，不存在缓存数据获取数据后，再写入缓存
        /// </summary>
        /// <typeparam name="T">返回的的数据类型</typeparam>
        /// <param name="key">缓存的key</param>
        /// <param name="acquire">获取数据，写入缓存的方法</param>
        /// <param name="cacheTime">缓存过期时间</param>
        /// <returns></returns>
        T Get<T>(string key, Func<T> acquire, int? cacheTime = null);
        void Set(string key, object data, int cacheTime);
        bool IsSet(string key);
        void Remove(string key);
        void RemoveByPrefix(string prefix);
        void Clear();
    }
}
