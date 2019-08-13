using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace CLF.Service.DTO.Core
{
   public abstract class EntityDTO:BaseEntityDTO
    {
        protected EntityDTO()
        {
            IsDeleted = false;
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
        }

        [DefaultValue(false)]
        [DataMember]
        public bool IsDeleted { get; set; }

        [Display(Name = "创建人")]
        [DataMember]
        public string CreatedBy { get; set; }
        [Display(Name = "创建时间")]
        [DataMember]
        public DateTime CreatedDate { get; set; }
        [Display(Name = "修改人")]
        [DataMember]
        public string ModifiedBy { get; set; }
        [Display(Name = "修改时间")]
        [DataMember]
        public DateTime ModifiedDate { get; set; }

        public string CreatedDateStr
        {
            get { return CreatedDate.ToString("yyyy-MM-dd HH:mm:ss"); }
        }

        public string ModifiedDateStr
        {
            get { return ModifiedDate.ToString("yyyy-MM-dd HH:mm:ss"); }
        }
    }
}
