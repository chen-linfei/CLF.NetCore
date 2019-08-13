using CLF.Service.DTO.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CLF.Web.Mvc.Models.Account
{
    public class PermissionModel
    {
        public PermissionModel()
        {
            this.Permissions = new List<PermissionDTO>();
        }
        public List<PermissionDTO> Permissions { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
    }
}
