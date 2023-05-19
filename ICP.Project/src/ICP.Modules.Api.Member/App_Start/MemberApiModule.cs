using Autofac;
using Autofac.Extras.DynamicProxy;
using Autofac.Integration.Mvc;
using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Frameworks.AOP;
using ICP.Library.Commands.App_Start;
using ICP.Library.Repositories.App_Start;
using ICP.Library.Services.App_Start;
using ICP.Modules.Api.Member.Commands;
using ICP.Modules.Api.Member.Controllers;
using ICP.Modules.Api.Member.Repositories;
using ICP.Modules.Api.Member.Services;
using System.Linq;

namespace ICP.Modules.Api.Member.App_Start
{
    public class MemberApiModule : Autofac.Module
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

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(x => x.Name.EndsWith("Service"))
                   .EnableClassInterceptors()
                   .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(x => x.FullName.StartsWith("ICP.Modules.Api.Member.Area.Commands") ||
                               x.FullName.StartsWith("ICP.Modules.Api.Member.Area.Services"))
                   .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(x => x.FullName.StartsWith("ICP.Modules.Api.Member.Area.Repositories"))
                   .InstancePerLifetimeScope()
                   .EnableClassInterceptors()
                   .InterceptedBy(typeof(DbProxyInterceptor));

            builder.RegisterModule<CommandLibraryModule>();
            builder.RegisterModule<ServiceLibraryModule>();
            builder.RegisterModule<RepositoryLibraryModule>();
        }
    }
}
