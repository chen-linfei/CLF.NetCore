using CLF.Service.DTO.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CLF.Service.DTO.Account
{
   public class PermissionDTO: TreeNodeDTO<PermissionDTO>
    {
        public PermissionDTO() { }

        [DisplayName("Area名称")]
        public string AreaName { get; set; }

        [DisplayName("Controller名称")]
        public string ControllerName { get; set; }

        [DisplayName("Action名称")]
        public string ActionName { get; set; }

        [DisplayName("描述")]
        public string Description { get; set; }

        [DisplayName("备注")]
        public string Remark { get; set; }

        [DisplayName("排序")]
        public int Index { get; set; }
    }
}
