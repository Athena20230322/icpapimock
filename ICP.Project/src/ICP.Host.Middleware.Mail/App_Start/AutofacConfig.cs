using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ICP.Infrastructure.Core.Frameworks.AOP;
using ICP.Infrastructure.Core.Web.Frameworks;
using System.Reflection;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using Autofac.Extras.DynamicProxy;
using ICP.Library.Repositories.App_Start;
using ICP.Library.Services.App_Start;

namespace ICP.Host.Middleware.Mail.App_Start
{
    public class AutofacConfig
    {
        public static void Register()
        {
            // 容器建立者
            ContainerBuilder builder = new ContainerBuilder();

            // 載入模組
            builder.RegisterModule<DefaultHostModule>();

            //Library 注入管理
            builder.RegisterModule<ServiceLibraryModule>();
            builder.RegisterModule<RepositoryLibraryModule>();

            var assembly = Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly)
                   .Where(x => x.FullName.StartsWith("ICP.Host.Middleware.Mail.Commands") ||
                               x.FullName.StartsWith("ICP.Host.Middleware.Mail.Services"))
                   .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(assembly)
                   .Where(x => x.FullName.StartsWith("ICP.Host.Middleware.Mail.Repositories"))
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