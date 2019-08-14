using CLF.Common.Exceptions;
using CLF.Domain.Core.IRepository;
using CLF.Model.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace CLF.Domain.Core.EFRepository
{
    /// <summary>  
    ///     单元操作实现  
    /// </summary>  
    public abstract class EFUnitOfWorkContextBase : IUnitOfWorkContext
    {
        /// <summary>  
        /// 获取 当前使用的数据访问上下文对象  
        /// </summary>  
        public abstract DbContext Context { get; }

        /// <summary>  
        ///     获取 当前单元操作是否已被提交  
        /// </summary>  
        public bool IsCommitted { get; private set; }

        /// <summary>  
        ///     提交当前单元操作的结果  
        /// </summary>  
        /// <returns></returns>  
        public int Commit()
        {
            if (IsCommitted)
            {
                return 0;
            }
            try
            {
                int result = Context.SaveChanges();
                IsCommitted = true;
                return result;
            }
            catch (Exception ex)
            {
                var entities = from entity in Context.ChangeTracker.Entries()
                               where entity.State == EntityState.Added ||
                               entity.State == EntityState.Modified ||
                               entity.State == EntityState.Deleted
                               select entity.Entity;

                var validationResults = new List<ValidationResult>();
                foreach (var entity in entities)
                {
                    var validationContext = new ValidationContext(entity);
                    Validator.TryValidateObject(entity, validationContext, validationResults);
                }
                EntityValidationException validationException = new EntityValidationException(validationResults.Select(x => new ValidationException(x, null, null)));

                StringBuilder sbErrors = new StringBuilder();
                foreach (var err in validationException.ValidationErrors)
                {
                    var validationResult = err.ValidationResult;
                    sbErrors.Append("Error: " + validationResult.MemberNames.First() + ", Message: " + validationResult.ErrorMessage + Environment.NewLine);
                }
                throw PublicHelper.ThrowDataAccessException("提交数据验证时发生异常：" + sbErrors, ex);
            }
        }

        /// <summary>  
        ///     把当前单元操作回滚成未提交状态  
        /// </summary>  
        public void Rollback()
        {
            IsCommitted = false;
        }

        public void Dispose()
        {
            Context.Dispose();
        }

        /// <summary>  
        ///   为指定的类型返回 System.Data.Entity.DbSet，这将允许对上下文中的给定实体执行 CRUD 操作。  
        /// </summary>  
        /// <typeparam name="TEntity"> 应为其返回一个集的实体类型。 </typeparam>  
        /// <returns> 给定实体类型的 System.Data.Entity.DbSet 实例。 </returns>  
        public DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return Context.Set<TEntity>();
        }

        /// <summary>  
        ///     注册一个新的对象到仓储上下文中  
        /// </summary>  
        /// <typeparam name="TEntity"> 要注册的类型 </typeparam>  
        /// <param name="entity"> 要注册的对象 </param>  
        public void RegisterNew<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            EntityState state = Context.Entry(entity).State;
            if (state == EntityState.Detached)
            {
                Context.Entry(entity).State = EntityState.Added;
            }
            IsCommitted = false;
        }

        /// <summary>  
        ///     批量注册多个新的对象到仓储上下文中  
        /// </summary>  
        /// <typeparam name="TEntity"> 要注册的类型 </typeparam>  
        /// <param name="entities"> 要注册的对象集合 </param>  
        public void RegisterNew<TEntity>(IEnumerable<TEntity> entities) where TEntity : BaseEntity
        {
            try
            {
                Context.ChangeTracker.AutoDetectChangesEnabled = false;
                foreach (TEntity entity in entities)
                {
                    RegisterNew(entity);
                }
            }
            finally
            {
                Context.ChangeTracker.AutoDetectChangesEnabled = true;
            }
        }

        /// <summary>  
        ///     注册一个更改的对象到仓储上下文中  
        /// </summary>  
        /// <typeparam name="TEntity"> 要注册的类型 </typeparam>  
        /// <param name="properties">需部分更新的属性值名称</param>
        /// <param name="entity"> 要注册的对象 </param>  
        public void RegisterModified<TEntity>(TEntity entity, string[] properties = null) where TEntity : BaseEntity
        {
            Context.Entry(entity).State = EntityState.Detached;
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                Context.Set<TEntity>().Attach(entity);
            }

            if (properties != null)
            {
                foreach (var property in properties)
                {
                    Context.Entry(entity).Property(property).IsModified = true;
                }
            }
            else
            {
                Context.Entry(entity).State = EntityState.Modified;
            }

            if (entity is Entity)
            {
                Context.Entry(entity).Property("CreatedBy").IsModified = false;
                Context.Entry(entity).Property("CreatedDate").IsModified = false;
            }

            IsCommitted = false;
        }
    

        /// <summary>  
        ///     注册多个更改的对象到仓储上下文中  
        /// </summary>  
        /// <typeparam name="TEntity"> 要注册的类型 </typeparam>  
        /// <param name="entities"> 要注册的对象集合 </param>  
        /// <param name="properties">需部分更新的属性值名称</param>
        public void RegisterModified<TEntity>(IEnumerable<TEntity> entities, string[] properties = null) where TEntity : BaseEntity
        {
            try
            {
                Context.ChangeTracker.AutoDetectChangesEnabled = false;
                foreach (TEntity entity in entities)
                {
                    RegisterModified(entity);
                }
            }
            finally
            {
                Context.ChangeTracker.AutoDetectChangesEnabled = true;
            }
        }

        /// <summary>  
        ///   注册一个删除的对象到仓储上下文中  
        /// </summary>  
        /// <typeparam name="TEntity"> 要注册的类型 </typeparam>  
        /// <param name="entity"> 要注册的对象 </param>  
        public void RegisterDeleted<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            Context.Entry(entity).State = EntityState.Deleted;
            IsCommitted = false;
        }

        /// <summary>  
        ///   批量注册多个删除的对象到仓储上下文中  
        /// </summary>  
        /// <typeparam name="TEntity"> 要注册的类型 </typeparam>  
        /// <param name="entities"> 要注册的对象集合 </param>  
        public void RegisterDeleted<TEntity>(IEnumerable<TEntity> entities) where TEntity : BaseEntity
        {
            try
            {
                Context.ChangeTracker.AutoDetectChangesEnabled = false;
                foreach (TEntity entity in entities)
                {
                    RegisterDeleted(entity);
                }
            }
            finally
            {
                Context.ChangeTracker.AutoDetectChangesEnabled = true;
            }
        }

        /// <summary>  
        ///   注册一个删除的对象到仓储上下文中  
        /// </summary>  
        /// <typeparam name="TEntity"> 要注册的类型 </typeparam>  
        /// <param name="entity"> 要注册的对象 </param>  
        public void RegisterSoftDeleted<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            Context.Entry(entity).State = EntityState.Detached;
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                Context.Set<TEntity>().Attach(entity);
            }

            if (entity is Entity)
            {
                Context.Entry(entity).Property("IsDeleted").IsModified = true;
            }

            IsCommitted = false;
        }
    }
}
