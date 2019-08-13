using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using CLF.Common.Configuration;
using CLF.Common.Exceptions;
using CLF.Common.Infrastructure.Mapper;
using CLF.Common.Infrastructure.TypeFinder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CLF.Common.Infrastructure
{
    public class EngineManager : IEngine
    {
        private IServiceProvider _serviceProvider { get; set; }

        public virtual IServiceProvider ServiceProvider => _serviceProvider;

        protected IServiceProvider GetServiceProvider()
        {
            var accessor = ServiceProvider.GetService<IHttpContextAccessor>();
            if (accessor != null)
            {
                var context = accessor.HttpContext;
                return context?.RequestServices ?? ServiceProvider;
            }
            return ServiceProvider;
        }

        /// <summary>
        /// 配置整个项目的ConfigureService
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public virtual IServiceProvider ConfigureServices(IServiceCollection services, IConfiguration configuration, AppConfig appConfig)
        {
            var typeFinder = new AppDomainTypeFinder();
            var startupConfigurations = typeFinder.FindClassesOfType<IAppStartup>();

            //创建项目所有实现IAppStartup接口类的实例，实现配置服务方法
            var instances = startupConfigurations
                .Select(startup => (IAppStartup)Activator.CreateInstance(startup))
                .OrderBy(startup => startup.Order);
            foreach (var instance in instances)
                instance.ConfigureServices(services, configuration);

            //注册AutoMapper服务
            AddAutoMapper(services, typeFinder);

            //注册依赖注入服务
            RegisterDependencies(services, typeFinder, appConfig);

            return _serviceProvider;
        }

        /// <summary>
        /// 配置HTTP请求管道
        /// </summary>
        /// <param name="application"></param>
        public void ConfigureRequestPipeline(IApplicationBuilder application)
        {
            var typeFinder = Resolve<ITypeFinder>();
            var startupConfigurations = typeFinder.FindClassesOfType<IAppStartup>();

            //创建项目所有实现IAppStartup接口类的实例，实现Configure方法
            var instances = startupConfigurations
                .Select(startup => (IAppStartup)Activator.CreateInstance(startup))
                .OrderBy(startup => startup.Order);
            foreach (var instance in instances)
                instance.Configure(application);
        }

        /// <summary>
        /// 注册和配置AutoMapper
        /// </summary>
        /// <param name="services"></param>
        /// <param name="typeFinder"></param>
        protected virtual void AddAutoMapper(IServiceCollection services, ITypeFinder typeFinder)
        {
            //创建项目所有实现IOrderedMapperProfile接口的类的实例
            var mapperConfigurations = typeFinder.FindClassesOfType<IOrderedMapperProfile>();
            var instances = mapperConfigurations
                .Select(mapperConfiguration => (IOrderedMapperProfile)Activator.CreateInstance(mapperConfiguration))
                .OrderBy(mapperConfiguration => mapperConfiguration.Order);

            var config = new MapperConfiguration(cfg =>
            {
                foreach (var instance in instances)
                {
                    cfg.AddProfile(instance.GetType());
                }
            });

            //创建映射
            AutoMapperConfiguration.Init(config);
        }


        /// <summary>
        /// 注册整个项目依赖注入服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="typeFinder"></param>
        /// <returns></returns>
        protected virtual IServiceProvider RegisterDependencies(IServiceCollection services, ITypeFinder typeFinder,AppConfig appConfig)
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterInstance(this).As<IEngine>().SingleInstance();

            containerBuilder.RegisterInstance(typeFinder).As<ITypeFinder>().SingleInstance();

            containerBuilder.Populate(services);

            //创建项目所有实现IDependencyRegistrar接口类的实例，实现注册依赖注入方法
            var dependencyRegistrars = typeFinder.FindClassesOfType<IDependencyRegistrar>();
            var instances = dependencyRegistrars
                .Select(dependencyRegistrar => (IDependencyRegistrar)Activator.CreateInstance(dependencyRegistrar))
                .OrderBy(dependencyRegistrar => dependencyRegistrar.Order);
            foreach (var dependencyRegistrar in instances)
                dependencyRegistrar.Register(containerBuilder, typeFinder, appConfig);

            //创建服务提供程序
            _serviceProvider = new AutofacServiceProvider(containerBuilder.Build());

            return _serviceProvider;
        }

        public T Resolve<T>() where T : class
        {
            return (T)Resolve(typeof(T));
        }

        public object Resolve(Type type)
        {
            return GetServiceProvider().GetService(type);
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            return (IEnumerable<T>)GetServiceProvider().GetServices(typeof(T));
        }

        public object ResolveUnregistered(Type type)
        {
            Exception innerException = null;
            foreach (var constructor in type.GetConstructors())
            {
                try
                {
                    var parameters = constructor.GetParameters().Select(parameter =>
                    {
                        var service = Resolve(parameter.ParameterType);
                        if (service == null)
                            throw new ComponentException(nameof(service));
                        return service;
                    });
                    return Activator.CreateInstance(type, parameters.ToArray());
                }
                catch (Exception ex)
                {
                    innerException = ex;
                }
            }

            throw new ComponentException("找不到合适注入的构造函数", innerException);
        }
    }
}
