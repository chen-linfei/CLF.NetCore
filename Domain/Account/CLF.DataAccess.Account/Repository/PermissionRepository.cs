using CLF.Common.Extensions;
using CLF.Domain.Core.EFRepository;
using CLF.Model.Account;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CLF.DataAccess.Account.Repository
{
    public class PermissionRepository : EFRepositoryBase<Permission>
    {
        public PermissionRepository(EFUnitOfWorkContextBase unitOfWork)
            : base(unitOfWork)
        {
        }

        public IList<Permission> LoadPermissionByFilter(Expression<Func<Permission, bool>> predicate, string sort, bool ascending = true)
        {
            var q = Entities
                .Where(predicate)
                .AsNoTracking()
                .OrderBy(m => m.Index);

            var results = q.SingleOrderBy(sort, ascending).ToList();
            return results;
        }

        public override Tuple<int, IList<Permission>> FindPagenatedListWithCount<K>(int pageIndex, int pageSize,
       Expression<Func<Permission, bool>> predicate, string sortProperty, bool ascending = true)
        {
            EFContext.Context.ChangeTracker.LazyLoadingEnabled = false;
            var q = Entities
              .Where(predicate).AsNoTracking();
            int recordCount = q.Count();

            var data = new Tuple<int, IList<Permission>>(recordCount,
                q.SingleOrderBy(sortProperty, ascending)
                    .Skip((pageIndex) * pageSize)
                    .Take(pageSize)
                    .ToList());

            EFContext.Context.ChangeTracker.LazyLoadingEnabled = true;
            return data;
        }
    }
}
