using CLF.Common.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CLF.Web.Framework.Infrastructure.Extensions;

namespace CLF.Web.Framework.Infrastructure
{
    public  class MvcStartup : IAppStartup
    {
        public int Order => 1000;

        public void Configure(IApplicationBuilder application)
        {
            application.UseHttpsRedirection();
            application.UseStaticFiles();
            application.UseCookiePolicy();

            application.UseAuthentication();

            application.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity();
            services.AddAppMvc();
        }
    }
}
