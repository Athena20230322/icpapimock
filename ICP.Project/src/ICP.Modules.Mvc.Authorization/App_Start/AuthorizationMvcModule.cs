using Autofac;
using Autofac.Extras.DynamicProxy;
using ICP.Infrastructure.Abstractions.Authorization;
using ICP.Infrastructure.Abstractions.FilterProxy;
using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Frameworks.AOP;
using ICP.Library.Commands.App_Start;
using ICP.Library.Repositories.App_Start;
using ICP.Library.Services.App_Start;
using ICP.Modules.Mvc.Authorization.Commands;
using ICP.Modules.Mvc.Authorization.FilterProxies;
using ICP.Modules.Mvc.Authorization.Services;
using System.Web;

namespace ICP.Modules.Mvc.Authorization.App_Start
{
    public class AuthorizationMvcModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            string sNamespace = ThisAssembly.GetName().Name;

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(x => x.FullName.StartsWith($"{sNamespace}.Commands") ||
                               x.FullName.StartsWith($"{sNamespace}.Services"))
                   .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(x => x.FullName.StartsWith($"{sNamespace}.Repositories"))
                   .InstancePerLifetimeScope()
                   .EnableClassInterceptors()
                   .InterceptedBy(typeof(DbProxyInterceptor));

            builder.RegisterModule<CommandLibraryModule>();
            builder.RegisterModule<ServiceLibraryModule>();
            builder.RegisterModule<RepositoryLibraryModule>();

            builder.RegisterType<UserManagerCommand>()
                   .As<IUserManager>()
                   .WithMetadata(nameof(AuthorizationType), AuthorizationType.Mvc)
                   .InstancePerLifetimeScope();

            builder.RegisterType<MvcAuthorizeFilterProxy>()
                   .As<IFilterProxy>()
                   .WithMetadata(nameof(IFilterProxy), ProxyType.AuthorizationMvc);            

            builder.Register(context =>
                    {
                        var httpContext = HttpContext.Current;
                        var logger = context.Resolve<ILogger<IdentifyService>>();
                        return new IdentifyService(httpContext, logger);
                    })
                   .InstancePerLifetimeScope();
        }
    }
}
