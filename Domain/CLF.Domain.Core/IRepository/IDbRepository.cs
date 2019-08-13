using CLF.Model.Core;
using CLF.Model.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace CLF.Domain.Core.IRepository
{
    public interface IDbRepository<T> : IRepository<T> where T : BaseEntity
    {
        #region 属性
        /// <summary>
        ///     获取 当前实体的查询数据集
        /// </summary>
        IQueryable<T> Entities { get; }

        #endregion


        #region 公共方法

        #region Search
        T FindById(params object[] keyValues);
        T FindByFilter(Expression<Func<T, bool>> predicate);

        IList<T> FindPagenatedList<K>(int pageIndex, int pageSize, Expression<Func<T, bool>> predicate,
            Expression<Func<T, K>> keySelector, bool ascending = true);
        IList<T> FindPagenatedList<K>(int pageIndex, int pageSize, Expression<Func<T, bool>> predicate,
          string sortProperty, bool ascending = true);
        Tuple<int, IList<T>> FindPagenatedListWithCount<K>(int pageIndex, int pageSize,
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, K>> keySelector, bool ascending = true);

        #endregion

        /// <summary>
        ///     批量插入实体记录集合
        /// </summary>
        /// <param name="entities"> 实体记录集合 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        int Add(IEnumerable<T> entities, bool isSave = true);

        /// <summary>  
        ///     更新实体记录  
        /// </summary>  
        /// <param name="entity"> 实体对象 </param>
        /// <param name="properties">需部分更新的属性值名称</param>
        /// <param name="isSave"> 是否执行保存 </param>  
        /// <returns> 操作影响的行数 </returns>  
        bool Modify(T entity, string[] properties, bool isSave = true);

        /// <summary>  
        ///     更新实体记录  
        /// </summary>  
        /// <param name="entities"> 实体对象 </param>
        /// <param name="properties">需部分更新的属性值名称</param>
        /// <param name="isSave"> 是否执行保存 </param>  
        /// <returns> 操作影响的行数 </returns>  
        int Modify(IEnumerable<T> entities, string[] properties = null, bool isSave = true);

        /// <summary>  
        ///     删除指定编号的记录  
        /// </summary>  
        /// <param name="id"> 实体记录编号 </param>  
        /// <param name="isSave"> 是否执行保存 </param>  
        /// <returns> 操作影响的行数 </returns>  
        bool Remove(object id, bool isSave = true);

        /// <summary>  
        ///     删除指定编号的记录  
        /// </summary>  
        /// <param name="id"> 实体记录编号 </param>  
        /// <param name="isSave"> 是否执行保存 </param>  
        /// <returns> 操作影响的行数 </returns>  
        bool SoftRemove(object id, bool isSave = true);

        /// <summary>  
        ///     删除实体记录  
        /// </summary>  
        /// <param name="entity"> 实体对象 </param>  
        /// <param name="isSave"> 是否执行保存 </param>  
        /// <returns> 操作影响的行数 </returns>  
        bool SoftRemove(T entity, bool isSave = true);

        #endregion
    }
}
