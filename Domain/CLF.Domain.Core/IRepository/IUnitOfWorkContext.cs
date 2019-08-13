using CLF.Model.Core.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;


namespace CLF.Domain.Core.IRepository
{
    /// <summary>  
    /// 数据单元操作接口  
    /// </summary>  
    public interface IUnitOfWorkContext : IUnitOfWork, IDisposable
    {
        DbContext Context { get; }
        /// <summary>  
        ///   为指定的类型返回 System.Data.Entity.DbSet，这将允许对上下文中的给定实体执行 CRUD 操作。  
        /// </summary>  
        /// <typeparam name="TEntity"> 应为其返回一个集的实体类型。 </typeparam>  
        /// <returns> 给定实体类型的 System.Data.Entity.DbSet 实例。 </returns>  
        DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity;

        /// <summary>  
        ///   注册一个新的对象到仓储上下文中  
        /// </summary>  
        /// <typeparam name="TEntity"> 要注册的类型 </typeparam>  
        /// <param name="entity"> 要注册的对象 </param>  
        void RegisterNew<TEntity>(TEntity entity) where TEntity : BaseEntity;

        /// <summary>  
        ///   批量注册多个新的对象到仓储上下文中  
        /// </summary>  
        /// <typeparam name="TEntity"> 要注册的类型 </typeparam>  
        /// <param name="entities"> 要注册的对象集合 </param>  
        void RegisterNew<TEntity>(IEnumerable<TEntity> entities) where TEntity : BaseEntity;

        /// <summary>  
        ///   注册一个更改的对象到仓储上下文中  
        /// </summary>  
        /// <typeparam name="TEntity"> 要注册的类型 </typeparam>  
        /// <param name="entity"> 要注册的对象 </param>
        /// <param name="properties">要更新对象的属性列表</param>  
        void RegisterModified<TEntity>(TEntity entity, string[] properties = null) where TEntity : BaseEntity;

        /// <summary>  
        ///   注册一个更改的对象到仓储上下文中  
        /// </summary>  
        /// <typeparam name="TEntity"> 要注册的类型 </typeparam>  
        /// <param name="entities">要注册的对象</param>
        /// <param name="properties">要更新对象的属性列表</param>  
        void RegisterModified<TEntity>(IEnumerable<TEntity> entities, string[] properties = null) where TEntity : BaseEntity;

        /// <summary>  
        ///   注册一个删除的对象到仓储上下文中  
        /// </summary>  
        /// <typeparam name="TEntity"> 要注册的类型 </typeparam>  
        /// <param name="entity"> 要注册的对象 </param>  
        void RegisterDeleted<TEntity>(TEntity entity) where TEntity : BaseEntity;

        /// <summary>  
        ///   批量注册多个删除的对象到仓储上下文中  
        /// </summary>  
        /// <typeparam name="TEntity"> 要注册的类型 </typeparam>  
        /// <param name="entities"> 要注册的对象集合 </param>  
        void RegisterDeleted<TEntity>(IEnumerable<TEntity> entities) where TEntity : BaseEntity;

        /// <summary>  
        ///   注册一个删除的对象到仓储上下文中  
        /// </summary>  
        /// <typeparam name="TEntity"> 要注册的类型 </typeparam>  
        /// <param name="entity"> 要注册的对象 </param>  
        void RegisterSoftDeleted<TEntity>(TEntity entity) where TEntity : BaseEntity;

    }
}
