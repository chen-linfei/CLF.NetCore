using CLF.Common.Configuration;
using CLF.Common.Http;
using CLF.Common.Infrastructure;
using CLF.DataAccess.Account;
using CLF.Web.Framework.Mvc.Filters;
using EasyCaching.Core;
using EasyCaching.InMemory;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace CLF.Web.Framework.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceProvider ConfigureApplicationServices(this IServiceCollection services,
            IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            services.AddHttpContextAccessor();

            var appconfig = services.ConfigureStartupConfig<AppConfig>(configuration.GetSection("App"));
            var engine = EngineContext.Create();
            var serviceProvider = engine.ConfigureServices(services, configuration,appconfig);

            services.AddSerilog();

            return serviceProvider;
        }

        /// <summary>
        /// 配置数据库上下文
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddDbContext<TDbContext>(this IServiceCollection services, IConfiguration configuration) where TDbContext : DbContext
        {
            var appConfig = services.ConfigureStartupConfig<AppConfig>(configuration.GetSection("App"));
            services.AddDbContextPool<TDbContext>(optionsBuilder =>
            {
                optionsBuilder.UseSqlServer(appConfig.SqlServerConnectionString);
            });
        }

        public static void AddHttpContextAccessor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }


        public static IMvcBuilder AddAppMvc(this IServiceCollection services)
        {
            var mvcBuilder = services.AddMvc(options =>
            {
                //options.Filters.Add<HttpExceptionFilter>(); //全局异常过滤器
            });

            mvcBuilder.AddMvcOptions(options => options.EnableEndpointRouting = false);

            mvcBuilder.AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; //忽略循环引用
                options.SerializerSettings.ContractResolver = new DefaultContractResolver(); //不使用驼峰样式的key
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });

            mvcBuilder.SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            mvcBuilder.AddControllersAsServices();
     
            return mvcBuilder;
        }

        public static IdentityBuilder AddIdentity(this IServiceCollection services)
        {
            return services.AddDefaultIdentity<IdentityUser>()
            .AddDefaultUI(UIFramework.Bootstrap4)
            .AddEntityFrameworkStores<AccountContext>();
        }

        public static void AddEasyCaching(this IServiceCollection services)
        {
            services.AddEasyCaching(option =>
            {
                option.UseInMemory("clfnetcore_memory_cache");
            });
        }

        public static void AddHttpSession(this IServiceCollection services)
        {
            services.AddSession(options =>
            {
                options.Cookie.Name = $"{AppCookieDefaults.Prefix}{AppCookieDefaults.SessionCookie}";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                //options.Cookie.IsEssential = true;
            });
        }

        public static void AddSerilog(this IServiceCollection services)
        {
            var config = EngineContext.Current.Resolve<AppConfig>();

            var logEventLevel = GetLogEventLevel(config.LogEventLevel);
            var rollingInterval = GetRollingInterval(config.RollingInterval);

            var serilog = new LoggerConfiguration()
                 .Enrich.FromLogContext()
                 .WriteTo.File(config.LogFilePath, logEventLevel, rollingInterval: rollingInterval)
                 .CreateLogger();

            //启动时创建logger对象，静态方法，后面所有地方通过类似Log.Debug()方式调用，无需再创建logger对象
            Log.Logger = serilog;

            //注入.net core 日志框架
            var loggerFactory = EngineContext.Current.Resolve<ILoggerFactory>();
            loggerFactory.AddSerilog(serilog);

            //系统遇到问题继续把内存中的日志信息输出到日志文件
            IApplicationLifetime appLifetime = EngineContext.Current.Resolve<IApplicationLifetime>();
            if (appLifetime != null)
            {
                appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);
            }

            LogEventLevel GetLogEventLevel(string logLevel)
            {
                switch (logLevel)
                {
                    case "Verbose":
                        return LogEventLevel.Verbose;
                    case "Debug":
                        return LogEventLevel.Debug;
                    case "Information":
                        return LogEventLevel.Information;
                    case "Warning":
                        return LogEventLevel.Warning;
                    case "Error":
                        return LogEventLevel.Error;
                    case "Fatal":
                        return LogEventLevel.Fatal;
                    default:
                        return LogEventLevel.Error;
                }
            }

            RollingInterval GetRollingInterval(string interval)
            {
                switch (interval)
                {
                    case "Infinite":
                        return RollingInterval.Infinite;
                    case "Year":
                        return RollingInterval.Year;
                    case "Day":
                        return RollingInterval.Day;
                    case "Month":
                        return RollingInterval.Month;
                    case "Hour":
                        return RollingInterval.Hour;
                    case "Minute":
                        return RollingInterval.Minute;
                    default:
                        return RollingInterval.Hour;
                }
            }
        }

        public static TConfig ConfigureStartupConfig<TConfig>(this IServiceCollection services, IConfiguration configuration) where TConfig : class, new()
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            var config = new TConfig();

            //绑定配置文件到对象
            configuration.Bind(config);

            //注册TConfig对象，后续通过EngineContext.Current.Resolve<TConfig>()即可获取到相关配置
            services.AddSingleton(config);

            return config;
        }
    }
}
