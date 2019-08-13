using System;
using System.Collections.Generic;
using System.Text;

namespace CLF.Common.Caching
{
    public static partial class CachingDefaultSettings
    {    
        public static int CacheTime => 60;

        /// <summary>
        /// 特殊受保护的key，很少用到
        /// </summary>
        public static string RedisDataProtectionKey => "CLF.DataProtectionKeys";
    }
}
