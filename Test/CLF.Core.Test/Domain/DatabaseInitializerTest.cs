using CLF.Common.Infrastructure;
using CLF.DataAccess.Account;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using CLF.Web.Framework.Infrastructure.Extensions;
using CLF.Web.Framework.Infrastructure;

namespace CLF.Core.Test.Domain
{
    [TestFixture]
    public class DatabaseInitializerTest
    {
        private IConfiguration _configuration;

        [SetUp]
        public void SetConfiguration()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

            _configuration = builder.Build();
        }

        /// <summary>
        /// 生成Acccount数据库.
        /// </summary>
        [Test]
        public void InitAcccountDatabase()
        {
            var serviceCollection = new ServiceCollection();

            DatabaseStartup databaseStartup = new DatabaseStartup();
            databaseStartup.ConfigureServices(serviceCollection, _configuration);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            DatabaseInitializer.Initialize(serviceProvider);
        }
    }
}
