using Autofac;
using Autofac.Extras.DynamicProxy;
using Autofac.Integration.Mvc;
using ICP.Infrastructure.Core.Frameworks.AOP;

namespace ICP.Modules.Api.CheckEinvoiceToken
{
    public class CheckEinvoiceTokenModule: Autofac.Module
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