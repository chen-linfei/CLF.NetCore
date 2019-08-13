using CLF.Common.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace CLF.Test
{
    public class TestServiceProvider : IServiceProvider
    {
        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(IHttpContextAccessor))
                return new Mock<IHttpContextAccessor>().Object;

            if (serviceType == typeof(IServiceCollection))
                return new Mock<IServiceCollection>().Object;

            if (serviceType == typeof(IConfiguration))
                return new Mock<IConfiguration>().Object;

            return null;
        }

        public T GetService<T>()
        {
            return (T)GetService(typeof(T));
        }
    }
}
