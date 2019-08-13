using CLF.Domain.Core.Mapping;
using CLF.Model.Account;
using CLF.Model.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace CLF.DataAccess.Account
{
    public class AccountContext : DbContext
    {
        public AccountContext(DbContextOptions<AccountContext> options):base(options)
        {
        }

        public DbSet<MenuNode> MenuNodes { get; set; }
        public DbSet<Permission>Permissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //动态创建模型配置
            var typeConfigurations = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type =>(type.BaseType?.IsGenericType ?? false)
                    && (type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>)));

            foreach (var typeConfiguration in typeConfigurations)
            {
                var configuration = (IMappingConfiguration)Activator.CreateInstance(typeConfiguration);
                configuration.ApplyConfiguration(modelBuilder);
            }

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            IEnumerable<EntityEntry> entries = ChangeTracker.Entries();

            foreach (var entry in entries)
            {
                if (!(entry.Entity is Entity))
                {
                    continue;
                }

                Entity entity = (Entity)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedBy = string.IsNullOrWhiteSpace(entity.CreatedBy) ? Thread.CurrentPrincipal?.Identity.Name : entity.CreatedBy;
                    entity.CreatedDate = entity.CreatedDate == DateTime.MinValue ? DateTime.Now : entity.CreatedDate;
                    entity.ModifiedBy = string.IsNullOrWhiteSpace(entity.ModifiedBy) ? Thread.CurrentPrincipal?.Identity.Name : entity.ModifiedBy;
                    entity.ModifiedDate = entity.ModifiedDate == DateTime.MinValue ? DateTime.Now : entity.ModifiedDate;
                }

                if (entry.State == EntityState.Modified)
                {
                    entity.ModifiedBy = string.IsNullOrWhiteSpace(entity.ModifiedBy) ? Thread.CurrentPrincipal?.Identity.Name : entity.ModifiedBy;
                    entity.ModifiedDate = entity.ModifiedDate == DateTime.MinValue ? DateTime.Now : entity.ModifiedDate;
                }
            }

            return base.SaveChanges();
        }
    }
}
