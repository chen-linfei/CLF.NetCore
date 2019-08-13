using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CLF.Service.DTO.Core
{
    public abstract class TreeNodeDTO<T> : EntityDTO where T : BaseEntityDTO
    {
        public TreeNodeDTO()
        {
            this.children = new List<T>();
        }
        public int Id { get; set; }
        public int? ParentId { get; set; }

        [DisplayName("名称")]
        public string Name { get; set; }

        [DisplayName("标识编码")]
        public string TreeCode { get; set; }

        /// <summary>
        /// 是否叶节点
        /// </summary>
        public bool Leaf { get; set; }

        public int Level { get; set; }

        public List<T> children { get; set; }
    }
}
