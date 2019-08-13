using CLF.Model.Account.Constants;
using CLF.Model.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CLF.Model.Account
{
    public  class MenuNode: TreeNode<MenuNode>
    {
        public MenuNode() { }

        /// <summary>
        /// 控制器名称
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// Action名称
        /// </summary>
        public string ActionName { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 小图标
        /// </summary>
        public string SmallIcon { get; set; }

        /// <summary>
        /// 大图标
        /// </summary>
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
