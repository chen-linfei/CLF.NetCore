using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CLF.Model.Core.Data
{
    public abstract class Entity : BaseEntity
    {
        protected Entity()
        {
            IsDeleted = false;
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
        }

        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [Display(Name = "创建人")]
        public string CreatedBy { get; set; }

        [Display(Name = "创建时间")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "修改人")]
        public string ModifiedBy { get; set; }

        [Display(Name = "修改时间")]
        public DateTime ModifiedDate { get; set; }
    }

}
