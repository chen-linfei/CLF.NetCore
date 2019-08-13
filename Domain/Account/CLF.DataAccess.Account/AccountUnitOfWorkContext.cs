using CLF.Common.Infrastructure;
using CLF.Domain.Core.EFRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CLF.DataAccess.Account
{
    public class AccountUnitOfWorkContext : EFUnitOfWorkContextBase
    {
        public AccountUnitOfWorkContext()
        {
            var options = EngineContext.Current.Resolve<DbContextOptions<AccountContext>>();
            AccountContext = new AccountContext(options);

            AccountContext.ChangeTracker.LazyLoadingEnabled = false;
        }
        public override DbContext Context => AccountContext;

        protected AccountContext AccountContext { get; private set; }
    }
}
