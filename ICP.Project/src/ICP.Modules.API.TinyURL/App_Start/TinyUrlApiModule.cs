using Autofac;
using Autofac.Extras.DynamicProxy;
using Autofac.Integration.Mvc;
using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Frameworks.AOP;
using System.Linq;

namespace ICP.Modules.Api.TinyURL.App_Start
{
    public class TinyURLApiModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            string sNamespace = ThisAssembly.GetName().Name;

            builder.RegisterControllers(ThisAssembly);

            builder.RegisterAssemblyTypes(ThisAssembly)
                     .Where(x => x.FullName.StartsWith($"{sNamespace}.Commands") ||
                                 x.FullName.StartsWith($"{sNamespace}.Services"))
                     .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(x => x.FullName.StartsWith($"{sNamespace}.Repositories"))
                   .InstancePerLifetimeScope()
                   .EnableClassInterceptors()
                   .InterceptedBy(typeof(DbProxyInterceptor));

        }
    }
}
