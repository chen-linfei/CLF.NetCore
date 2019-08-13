using CLF.Domain.Core.IRepository;
using CLF.Model.Core.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using CLF.Common.Extensions;
using CLF.Common.Exceptions;

namespace CLF.Domain.Core.EFRepository
{
    /// <summary>
    ///     EntityFramework仓储操作基类
    /// </summary>
    /// <typeparam name="T">动态实体类型</typeparam>
    public abstract class EFRepositoryBase<T> : IDbRepository<T> where T : BaseEntity
    {
        protected EFRepositoryBase(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        #region 属性

        /// <summary>
        ///     获取 仓储上下文的实例
        /// </summary>
        public IUnitOfWork UnitOfWork { get; private set; }

        /// <summary>
        ///     获取或设置 EntityFramework的数据仓储上下文
        /// </summary>
        protected IUnitOfWorkContext EFContext
        {
            get
            {
                if (UnitOfWork is IUnitOfWorkContext)
                {
                    return UnitOfWork as IUnitOfWorkContext;
                }
                throw new DataAccessException(string.Format("数据仓储上下文对象类型不正确，应为IRepositoryContext，实际为 {0}", UnitOfWork.GetType().Name));
            }
        }

        /// <summary>
        ///     获取 当前实体的查询数据集
        /// </summary>
        public virtual IQueryable<T> Entities
        {
            get { return EFContext.Set<T>(); }
        }

        #endregion

        #region 公共方法

        #region Search
        public virtual T FindById(params object[] keyValues)
        {
            var result = EFContext.Set<T>().Find(keyValues);
            return result;
        }
        public virtual T FindByFilter(Expression<Func<T, bool>> predicate)
        {
            return EFContext.Set<T>().AsNoTracking().FirstOrDefault(predicate);
        }

        public virtual IList<T> FindAll()
        {
            return EFContext.Set<T>().AsNoTracking().ToList();
        }
        public virtual IList<T> FindAll<K>(Expression<Func<T, K>> keySelector, bool ascending = true)
        {
            return @ascending
                ? EFContext.Set<T>().OrderBy(keySelector).AsNoTracking().ToList()
                : EFContext.Set<T>().OrderByDescending(keySelector).AsNoTracking().ToList();
        }

        public virtual IList<T> Find(Expression<Func<T, bool>> predicate)
        {
            return EFContext.Set<T>().Where(predicate).AsNoTracking().ToList();
        }
        public virtual IList<T> Find<K>(Expression<Func<T, bool>> predicate, Expression<Func<T, K>> keySelector, bool ascending = true)
        {
            return @ascending
                ? EFContext.Set<T>().Where(predicate).OrderBy(keySelector).AsNoTracking().ToList()
                : EFContext.Set<T>().Where(predicate).OrderByDescending(keySelector).AsNoTracking().ToList();
        }

        public virtual IList<T> FindPagenatedList<K>(int pageIndex, int pageSize, Expression<Func<T, bool>> predicate,
            Expression<Func<T, K>> keySelector, bool ascending = true)
        {
            return @ascending
                ? EFContext.Set<T>().Where(predicate).OrderBy(keySelector).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking().ToList()
                : EFContext.Set<T>().Where(predicate).OrderByDescending(keySelector).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking().ToList();
        }
        public virtual IList<T> FindPagenatedList<K>(int pageIndex, int pageSize, Expression<Func<T, bool>> predicate,
           string sortProperty, bool ascending = true)
        {
            var q = EFContext.Set<T>().Where(predicate).AsNoTracking();
            return q.SingleOrderBy(sortProperty, ascending).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        public virtual Tuple<int, IList<T>> FindPagenatedListWithCount<K>(int pageIndex, int pageSize,
            Expression<Func<T, bool>> predicate, string sortProperty, bool ascending = true)
        {
            var q = EFContext.Set<T>().Where(predicate).AsNoTracking();
            int recordCount = q.Count();
            var data = q.SingleOrderBy(sortProperty, ascending).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new Tuple<int, IList<T>>(recordCount, data);
        }
        public virtual Tuple<int, IList<T>> FindPagenatedListWithCount<K>(int pageIndex, int pageSize,
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, K>> keySelector, bool ascending = true)
        {
            var databaseItems = EFContext.Set<T>().Where(predicate).AsNoTracking();
            int recordCount = databaseItems.Count();

            return @ascending
                ? new Tuple<int, IList<T>>(recordCount, databaseItems.OrderBy(keySelector).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList())
                : new Tuple<int, IList<T>>(recordCount, databaseItems.OrderByDescending(keySelector).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList());
        }

        #endregion

        #region Add
        /// <summary>  
        ///     插入实体记录  
        /// </summary>  
        /// <param name="entity"> 实体对象 </param>  
        /// <param name="isSave"> 是否执行保存 </param>  
        /// <returns> 操作影响的行数 </returns>  
        public virtual bool Add(T entity, bool isSave = true)
        {
            EFContext.RegisterNew(entity);
            return isSave ? EFContext.Commit() > 0 : false;
        }

        /// <summary>  
        ///     批量插入实体记录集合  
        /// </summary>  
        /// <param name="entities"> 实体记录集合 </param>  
        /// <param name="isSave"> 是否执行保存 </param>  
        /// <returns> 操作影响的行数 </returns>  
        public virtual int Add(IEnumerable<T> entities, bool isSave = true)
        {
            EFContext.RegisterNew(entities);
            return isSave ? EFContext.Commit() : 0;
        }

        #endregion

        #region hard remove the entity

        /// <summary>  
        ///     删除指定编号的记录  
        /// </summary>  
        /// <param name="id"> 实体记录编号 </param>  
        /// <param name="isSave"> 是否执行保存 </param>  
        /// <returns> 操作影响的行数 </returns>  
        public virtual bool Remove(object id, bool isSave = true)
        {
            T entity = EFContext.Set<T>().Find(id);
            return entity != null ? Remove(entity, isSave) : false;
        }
        /// <summary>  
        ///     删除实体记录  
        /// </summary>  
        /// <param name="entity"> 实体对象 </param>  
        /// <param name="isSave"> 是否执行保存 </param>  
        /// <returns> 操作影响的行数 </returns>  
        public virtual bool Remove(T entity, bool isSave = true)
        {
            EFContext.RegisterDeleted(entity);
            return isSave && EFContext.Commit() > 0;
        }
        /// <summary>  
        ///     删除实体记录集合  
        /// </summary>  
        /// <param name="entities"> 实体记录集合 </param>  
        /// <param name="isSave"> 是否执行保存 </param>  
        /// <returns> 操作影响的行数 </returns>  
        public virtual int Remove(IEnumerable<T> entities, bool isSave = true)
        {
            EFContext.RegisterDeleted(entities);
            return isSave ? EFContext.Commit() : 0;
        }
        /// <summary>  
        ///     删除所有符合特定表达式的数据  
        /// </summary>  
        /// <param name="predicate"> 查询条件谓语表达式 </param>  
        /// <param name="isSave"> 是否执行保存 </param>  
        /// <returns> 操作影响的行数 </returns>  
        public virtual int Remove(Expression<Func<T, bool>> predicate, bool isSave = true)
        {
            var entities = EFContext.Set<T>().Where(predicate).ToList();
            return entities.Count > 0 ? Remove(entities, isSave) : 0;
        }
        public virtual bool RemoveAll()
        {
            var entities = EFContext.Set<T>().ToList();
            if (entities.Count > 0)
            {
                Remove(entities, false);
                return EFContext.Commit() > 0;
            }

            return true;
        }

        #endregion

        #region soft remove the entity

        public virtual bool SoftRemove(object id, bool isSave = true)
        {
            T entity = EFContext.Set<T>().Find(id);
            return entity != null ? SoftRemove(entity, isSave) : false;
        }
        public virtual bool SoftRemove(T entity, bool isSave = true)
        {
            if (!(entity is Entity))
            {
                throw new ArgumentException("T must be an Entity type that is derived from the Entity class in the namespace Com.Domain.Core.");
            }

            var aEntity = entity as Entity;
            aEntity.IsDeleted = true;

            EFContext.RegisterSoftDeleted(entity);
            return isSave ? EFContext.Commit() > 0 : false;
        }
        public virtual int SoftRemove(IEnumerable<T> entities, bool isSave = true)
        {
            foreach (T entity in entities)
            {
                if (!(entity is Entity))
                {
                    throw new ArgumentException("T must be an Entity type that is derived from the Entity class in the namespace Com.Domain.Core.");
                }

                var aEntity = entity as Entity;
                aEntity.IsDeleted = true;

                EFContext.RegisterSoftDeleted(entity);
            }

            return isSave ? EFContext.Commit() : 0;
        }
        public virtual int SoftRemove(Expression<Func<T, bool>> predicate, bool isSave = true)
        {
            var entities = EFContext.Set<T>().Where(predicate).ToList();
            return entities.Count > 0 ? SoftRemove(entities, isSave) : 0;
        }

        #endregion

        #region modify the entity
        /// <summary>  
        ///     更新实体记录  
        /// </summary>  
        /// <param name="entity"> 实体对象 </param>  
        /// <param name="isSave"> 是否执行保存 </param>  
        /// <returns> 操作影响的行数 </returns>  
        public virtual bool Modify(T entity, bool isSave = true)
        {
            EFContext.RegisterModified(entity);
            return isSave && EFContext.Commit() > 0;
        }
        /// <summary>  
        ///     更新实体记录  
        /// </summary>  
        /// <param name="entity"> 实体对象 </param>  
        /// <param name="properties">需部分更新的属性值名称</param>
        /// <param name="isSave"> 是否执行保存 </param>  
        /// <returns> 操作影响的行数 </returns>  
        public virtual bool Modify(T entity, string[] properties, bool isSave = true)
        {
            EFContext.RegisterModified(entity, properties);
            return isSave && EFContext.Commit() > 0;
        }

        /// <summary>  
        ///     更新实体记录  
        /// </summary>  
        /// <param name="entities"> 实体对象 </param>
        /// <param name="properties">需部分更新的属性值名称</param>
        /// <param name="isSave"> 是否执行保存 </param>  
        /// <returns> 操作影响的行数 </returns>  
        public virtual int Modify(IEnumerable<T> entities, string[] properties = null, bool isSave = true)
        {
            EFContext.RegisterModified(entities, properties);
            return isSave ? EFContext.Commit() : 0;
        }
        #endregion

        /// <summary>
        /// 获取总记录数
        /// </summary>
        /// <returns></returns>
        public virtual int GetRecordCount()
        {
            return EFContext.Set<T>().AsNoTracking().Count();
        }

        #endregion
    }

    public class CommonRepository<T> : EFRepositoryBase<T> where T : BaseEntity
    {
        public CommonRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
