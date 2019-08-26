using CLF.Common.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace CLF.DataAccess.Account
{
  public static  class DatabaseInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider = null)
        {
            try
            {
                var options = EngineContext.Current.Resolve<DbContextOptions<AccountContext>>();
                var accountContext = new AccountContext(options);
                accountContext.Database.Migrate();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
