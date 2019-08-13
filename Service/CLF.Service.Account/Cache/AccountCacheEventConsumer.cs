using CLF.Common.Caching;
using CLF.Model.Account;
using CLF.Model.Core.Events;
using CLF.Service.Core.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace CLF.Service.Account.Cache
{
    public partial class AccountCacheEventConsumer :
         IConsumer<EntityUpdatedEvent<Permission>>,
           IConsumer<EntityInsertedEvent<Permission>>,
           IConsumer<EntityDeletedEvent<Permission>>
    {
        private readonly IStaticCacheManager _staticCacheManager;

        public AccountCacheEventConsumer(IStaticCacheManager staticCacheManager)
        {
            this._staticCacheManager = staticCacheManager;
        }
        public void HandleEvent(EntityUpdatedEvent<Permission> eventMessage)
        {
            _staticCacheManager.RemoveByPrefix(AccountServiceDefaults.GetPermissionListPrefixCacheKey);
            _staticCacheManager.Remove(AccountServiceDefaults.GetPermissionListCacheKey);
        }

        public void HandleEvent(EntityInsertedEvent<Permission> eventMessage)
        {
            _staticCacheManager.RemoveByPrefix(AccountServiceDefaults.GetPermissionListPrefixCacheKey);
            _staticCacheManager.Remove(AccountServiceDefaults.GetPermissionListCacheKey);
        }

        public void HandleEvent(EntityDeletedEvent<Permission> eventMessage)
        {
            _staticCacheManager.RemoveByPrefix(AccountServiceDefaults.GetPermissionListPrefixCacheKey);
            _staticCacheManager.Remove(AccountServiceDefaults.GetPermissionListCacheKey);
        }
    }
}
