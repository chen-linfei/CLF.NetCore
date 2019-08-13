using CLF.Common.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using CLF.Common.Configuration;
using Microsoft.Extensions.Logging;
using Serilog.Events;

namespace CLF.Web.Framework.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void ConfigureRequestPipeline(this IApplicationBuilder application)
        {
            EngineContext.Current.ConfigureRequestPipeline(application);
        }

        public static void UseAppExceptionHandler(this IApplicationBuilder application)
        {
            var hostingEnvironment = application.ApplicationServices.GetService<IHostingEnvironment>();

            if (hostingEnvironment.IsDevelopment())
            {
                application.UseDeveloperExceptionPage();
            }
            else
            {
                application.UseExceptionHandler("/Error/Error");
            }

            //错误写入日志
            application.UseExceptionHandler(handler =>
            {
                handler.Run(context =>
                {
                    var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                    if (exception == null)
                        return Task.CompletedTask;

                    try
                    {
                        Log.Error(exception.Message);
                    }
                    finally
                    {
                        //再次抛出异常
                        ExceptionDispatchInfo.Throw(exception);
                    }
                    return Task.CompletedTask;
                });
            });
        }
    }
}
