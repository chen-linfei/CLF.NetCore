using System;
using System.Collections.Generic;
using System.Text;

namespace CLF.Common.Caching
{
    public interface ILocker
    {
        /// <summary>
        /// 分布式缓存
        /// </summary>
        /// <param name="source">缓存的key</param>
        /// <param name="expirationTime">缓存过期时间</param>
        /// <param name="action">需要锁住的方法</param>
        /// <returns></returns>
        bool PerformActionWithLock(string source, TimeSpan expirationTime, Action action);
    }
}
