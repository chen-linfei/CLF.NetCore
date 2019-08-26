using CLF.Common.Infrastructure;
using CLF.DataAccess.Account;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using CLF.Web.Framework.Infrastructure.Extensions;
using CLF.Web.Framework.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.TestHost;

namespace CLF.Core.Test.Domain
{
    [TestFixture]
    public class DatabaseInitializerTest
    {
        [SetUp]
        public void UseStartup()
        {
            var builder = new WebHostBuilder()
                   .ConfigureAppConfiguration(config =>
                      {
                          config.SetBasePath(Directory.GetCurrentDirectory());
                          config.AddJsonFile("appsettings.json");
                      })
                      .UseStartup<CLF.Web.Mvc.Startup>();

            var server = new TestServer(builder);
        }

        /// <summary>
        /// 生成Acccount数据库
        /// </summary>
        [Test]
        public void InitAcccountDatabase()
        {
            DatabaseInitializer.Initialize();
        }

        #region

        private IConfiguration _configuration;
        public void SetConfiguration()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

            _configuration = builder.Build();
        }

        /// <summary>
        /// 生成Acccount数据库(不启动startup的方式)
        /// </summary>
        public void InitAcccountDB()
        {
            var serviceCollection = new ServiceCollection();

            DatabaseStartup databaseStartup = new DatabaseStartup();
            databaseStartup.ConfigureServices(serviceCollection, _configuration);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            DatabaseInitializer.Initialize(serviceProvider);
        }
        #endregion
    }
}
