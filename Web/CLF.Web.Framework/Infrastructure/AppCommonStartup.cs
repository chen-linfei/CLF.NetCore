using CLF.Common.Infrastructure;
using EasyCaching.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CLF.Web.Framework.Infrastructure.Extensions;

namespace CLF.Web.Framework.Infrastructure
{
    public class AppCommonStartup : IAppStartup
    {
        public int Order => 100;

        public void Configure(IApplicationBuilder application)
        {
            //session会话中间件
            application.UseSession();

            //easy caching中间件
            application.UseEasyCaching();

        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();

            services.AddEasyCaching();

            services.AddDistributedMemoryCache();

            services.AddHttpSession();
        }
    }
}
