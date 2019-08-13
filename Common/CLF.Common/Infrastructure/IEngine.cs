using CLF.Common.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace CLF.Common.Infrastructure
{
    public interface IEngine
    {
        /// <summary>
        /// 添加服务和配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="appConfig"></param>
        /// <returns></returns>
        IServiceProvider ConfigureServices(IServiceCollection services, IConfiguration configuration,AppConfig appConfig);

        /// <summary>
        /// 配置HTTP请求管道
        /// </summary>
        /// <param name="application"></param>
        void ConfigureRequestPipeline(IApplicationBuilder application);

        /// <summary>
        /// 解析依赖注入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Resolve<T>() where T : class;

        object Resolve(Type type);

        IEnumerable<T> ResolveAll<T>();

        /// <summary>
        /// 解析没注册过的服务
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        object ResolveUnregistered(Type type);
    }
}
