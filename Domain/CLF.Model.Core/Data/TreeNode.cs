using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CLF.Model.Core.Data
{
  public abstract  class TreeNode<T>:Entity where T :BaseEntity
    {
        public TreeNode() { }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public int? ParentId { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        [MaxLength(128)]
        public string TreeCode { get; set; }
        public bool Leaf { get; set; }
        public int Level { get; set; }

        [ForeignKey("ParentId")]
        public virtual List<T> ChildNodes { get; set; }
        public virtual T ParentNode { get; set; }
    }
}
