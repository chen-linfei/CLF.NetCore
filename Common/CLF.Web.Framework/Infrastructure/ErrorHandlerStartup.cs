using CLF.Common.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using CLF.Web.Framework.Infrastructure.Extensions;
namespace CLF.Web.Framework.Infrastructure
{
    public  class ErrorHandlerStartup: IAppStartup
    {
        public int Order => 2;

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
        }

        public void Configure(IApplicationBuilder application)
        {
            application.UseAppExceptionHandler();
        }
    }
}
