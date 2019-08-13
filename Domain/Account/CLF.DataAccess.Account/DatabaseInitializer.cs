using CLF.Common.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace CLF.DataAccess.Account
{
  public static  class DatabaseInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var options = serviceProvider.GetService<DbContextOptions<AccountContext>>();
            var accountContext = new AccountContext(options);
            accountContext.Database.Migrate();
        }
    }
}
