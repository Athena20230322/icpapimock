using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICP.Host.Middleware.JCIC.App_Start
{
    using Autofac.Extras.CommonServiceLocator;
    using Autofac.Extras.DynamicProxy;
    using Commands;
    using CommonServiceLocator;
    using ICP.Infrastructure.Core.Frameworks;
    using ICP.Infrastructure.Core.Frameworks.AOP;
    using ICP.Infrastructure.Core.Web.Frameworks;
    using Services;
    using System.Reflection;

    public static class AutofacConfig
    {
        public static void Register()
        {
            // 容器建立者
            ContainerBuilder builder = new ContainerBuilder();

            // 載入模組
            builder.RegisterModule<DefaultHostModule>();

            var assembly = Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly)
                   .Where(x => x.FullName.StartsWith("ICP.Host.Middleware.JCIC.Commands") ||
                               x.FullName.StartsWith("ICP.Host.Middleware.JCIC.Services"))
                   .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(assembly)
                   .Where(x => x.FullName.StartsWith("ICP.Host.Middleware.JCIC.Repositories"))
                   .InstancePerLifetimeScope()
                   .EnableClassInterceptors()
                   .InterceptedBy(typeof(DbProxyInterceptor));

            // 建立容器
            IContainer container = builder.Build();

            // Set the service locator to an AutofacServiceLocator.
            var csl = new AutofacServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => csl);
        }
    }
}