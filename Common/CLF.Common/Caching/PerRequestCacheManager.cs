using CLF.Common.ComponentModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace CLF.Common.Caching
{
    /// <summary>
    ///HTTP请求缓存
    /// </summary>
    public partial class PerRequestCacheManager : ICacheManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ReaderWriterLockSlim _locker;

        public PerRequestCacheManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            _locker = new ReaderWriterLockSlim();
        }

        protected virtual IDictionary<object, object> GetItems()
        {
            return _httpContextAccessor.HttpContext?.Items;
        }

        #region Methods

        public virtual T Get<T>(string key, Func<T> acquire, int? cacheTime = null)
        {
            IDictionary<object, object> items;

            using (new ReaderWriteLockDisposable(_locker, ReaderWriteLockType.Read))
            {
                items = GetItems();
                if (items == null)
                    return acquire();

                if (items[key] != null)
                    return (T)items[key];
            }

            var result = acquire();

            if (result == null || (cacheTime ?? CachingDefaultSettings.CacheTime) <= 0)
                return result;

            using (new ReaderWriteLockDisposable(_locker))
            {
                items[key] = result;
            }

            return result;
        }

        public virtual void Set(string key, object data, int cacheTime)
        {
            if (data == null)
                return;

            using (new ReaderWriteLockDisposable(_locker))
            {
                var items = GetItems();
                if (items == null)
                    return;

                items[key] = data;
            }
        }

        public virtual bool IsSet(string key)
        {
            using (new ReaderWriteLockDisposable(_locker, ReaderWriteLockType.Read))
            {
                var items = GetItems();
                return items?[key] != null;
            }
        }

        public virtual void Remove(string key)
        {
            using (new ReaderWriteLockDisposable(_locker))
            {
                var items = GetItems();
                items?.Remove(key);
            }
        }

        public virtual void RemoveByPrefix(string prefix)
        {
            using (new ReaderWriteLockDisposable(_locker, ReaderWriteLockType.UpgradeableRead))
            {
                var items = GetItems();
                if (items == null)
                    return;

                var regex = new Regex(prefix,
                    RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
                var matchesKeys = items.Keys.Select(p => p.ToString()).Where(key => regex.IsMatch(key)).ToList();

                if (!matchesKeys.Any())
                    return;

                using (new ReaderWriteLockDisposable(_locker))
                {
                    foreach (var key in matchesKeys)
                    {
                        items.Remove(key);
                    }
                }
            }
        }

        public virtual void Clear()
        {
            using (new ReaderWriteLockDisposable(_locker))
            {
                var items = GetItems();
                items?.Clear();
            }
        }

        public virtual void Dispose()
        {

        }

        #endregion
    }
}
