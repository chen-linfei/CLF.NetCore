using CLF.Model.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CLF.Model.Core
{
    public interface IRepository<T> where T : BaseEntity
    {
        IList<T> FindAll();
        IList<T> Find(Expression<Func<T, bool>> predicate);
        IList<T> FindAll<K>(Expression<Func<T, K>> keySelector, bool ascending = true);
        IList<T> Find<K>(Expression<Func<T, bool>> predicate, Expression<Func<T, K>> keySelector, bool ascending = true);

        bool Add(T entity, bool isSave = true);
        bool Modify(T entity, bool isSave = true);

        bool Remove(T entity, bool isSave = true);
        int Remove(IEnumerable<T> entities, bool isSave = true);
        bool RemoveAll();
    }
}
