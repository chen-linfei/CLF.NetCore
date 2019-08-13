using CLF.Service.DTO.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace CLF.Service.DTO.Account
{
  public  class MenuDTO:TreeNodeDTO<MenuDTO>
    {
        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Index { get; set; }

        public string SmallIcon { get; set; }

        public string BigIcon { get; set; }

        /// <summary>
        /// 访问地址
        /// </summary>
        public string URL
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ControllerName) || string.IsNullOrWhiteSpace(ActionName))
                {
                    return null;
                }
                return $"/{ControllerName.Trim()}/{ActionName.Trim()}";
            }
        }
    }
}
