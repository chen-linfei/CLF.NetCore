using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CLF.Web.Mvc.Constants
{
    public class ActionMethods
    {
        public const string Index = "Index";
        public const string List = "List";
        public const string Create = "Create";
        public const string Details = "Details";
        public const string Delete = "Delete";
        public const string Edit = "Edit";
        public const string CreateOrUpdate = "CreateOrUpdate";
        public const string Search = "Search";
        public const string Form = "Form";
        
        public sealed class Permission
        {
            public const string PermissionForm = "PermissionForm";
            public const string LoadPermissions = "LoadPermissions";
            
        }
    }
}
