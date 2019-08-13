using System;
using System.Collections.Generic;
using System.Text;

namespace CLF.Common.Infrastructure.Singleton
{
    public class BaseSingleton
    {
        /// <summary>
        /// 保证AllSingletons只实例化一次
        /// </summary>
        static BaseSingleton()
        {
            AllSingletons = new Dictionary<Type, object>();
        }
        public static IDictionary<Type, object> AllSingletons { get; }
    }
}
