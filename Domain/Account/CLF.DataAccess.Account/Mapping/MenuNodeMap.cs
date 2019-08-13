using CLF.Domain.Core.Mapping;
using CLF.Model.Account;
using CLF.Model.Account.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CLF.DataAccess.Account.Mapping
{
    public class MenuNodeMap: EntityTypeConfiguration<MenuNode>
    {
        public override void Configure(EntityTypeBuilder<MenuNode> builder)
        {
            builder.ToTable(Tables.MenuNode);

            builder.HasKey(p => p.Id);
            builder.Property(p => p.ControllerName).HasMaxLength(128);
            builder.Property(p => p.ActionName).HasMaxLength(128);
            builder.Property(p => p.Description).HasMaxLength(512);
            builder.Property(p => p.BigIcon).HasMaxLength(256);
            builder.Property(p => p.SmallIcon).HasMaxLength(256);

            base.Configure(builder);
        }
    }
}
