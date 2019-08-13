using Autofac;
using CLF.Common.Configuration;
using CLF.Common.Infrastructure.TypeFinder;
using System;
using System.Collections.Generic;
using System.Text;

namespace CLF.Common.Infrastructure
{
    public interface IDependencyRegistrar
    {
        void Register(ContainerBuilder builder, ITypeFinder typeFinder, AppConfig config);
        int Order { get; }
    }
}
