using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CLF.Domain.Core.Mapping
{
    public partial interface IMappingConfiguration
    {
        /// <summary>
        /// 配置Map文件
        /// </summary>
        /// <param name="modelBuilder"></param>
        void ApplyConfiguration(ModelBuilder modelBuilder);
    }
}
