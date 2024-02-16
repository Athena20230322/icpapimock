using Autofac;
using Autofac.Extras.DynamicProxy;
using ICP.Infrastructure.Core.Frameworks.AOP;

namespace ICP.Library.Repositories.App_Start
{
    public class RepositoryLibraryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                   .InstancePerLifetimeScope()
                   .EnableClassInterceptors()
                   .InterceptedBy(typeof(DbProxyInterceptor));
        }
    }
}
