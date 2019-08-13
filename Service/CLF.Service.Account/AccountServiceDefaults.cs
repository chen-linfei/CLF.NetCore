using System;
using System.Collections.Generic;
using System.Text;

namespace CLF.Service.Account
{
    public static partial class AccountServiceDefaults 
    {
        public static string GetPermissionListCacheKey => "CLF.GetPermissionListCacheKey-{0}";
        public static string GetPermissionByIdCacheKey => "CLF.GetPermissionByIdCacheKey-{0}";
        public static string GetPermissionListPrefixCacheKey => "CLF.GetPermissionListCacheKey-";
    }
}
