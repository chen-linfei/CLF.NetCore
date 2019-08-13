using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CLF.Common.Caching
{
    public interface IStaticCacheManager : ICacheManager
    {
        /// <summary>
        /// 存在缓存项，就从缓存中获取；不存在缓存项，获取数据后再缓存数据
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="acquire">执行缓存的方法</param>
        /// <param name="cacheTime">缓存过期时间</param>
        /// <returns>获取缓存结果</returns>
        Task<T> GetAsync<T>(string key, Func<Task<T>> acquire, int? cacheTime = null);
    }
}
