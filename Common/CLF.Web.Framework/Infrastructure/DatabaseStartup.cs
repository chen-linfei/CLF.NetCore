using CLF.Common.Infrastructure;
using CLF.DataAccess.Account;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CLF.Web.Framework.Infrastructure.Extensions;

namespace CLF.Web.Framework.Infrastructure
{
    public class DatabaseStartup : IAppStartup
    {
        public int Order => 10;

        public void Configure(IApplicationBuilder application)
        {
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AccountContext>(configuration);
            services.AddEntityFrameworkSqlServer();
            services.AddEntityFrameworkProxies();
        }
    }
}
