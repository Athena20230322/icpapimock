using Autofac;
using Autofac.Extras.DynamicProxy;
using ICP.Infrastructure.Abstractions.Authorization;
using ICP.Infrastructure.Abstractions.FilterProxy;
using ICP.Infrastructure.Core.Frameworks.AOP;
using ICP.Library.Commands.App_Start;
using ICP.Library.Repositories.App_Start;
using ICP.Library.Services.App_Start;
using ICP.Modules.Api.Authorization.Commands;
using ICP.Modules.Api.Authorization.FilterProxies;

namespace ICP.Modules.Api.Authorization.App_Start
{
    public class AuthorizationApiModule: Module
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
                   .WithMetadata(nameof(AuthorizationType), AuthorizationType.Api)
                   .InstancePerLifetimeScope();

            builder.RegisterType<ApiAuthorizeFilterProxy>()
                   .As<IFilterProxy>()
                   .WithMetadata(nameof(IFilterProxy), ProxyType.AuthorizationApi);

            builder.RegisterType<OPCustomAuthorizeFilterProxy>()
                   .As<IFilterProxy>()
                   .WithMetadata(nameof(IFilterProxy), ProxyType.OPCustomApi);

            builder.RegisterType<OPWebUIAuthorizeFilterProxy>()
                   .As<IFilterProxy>()
                   .WithMetadata(nameof(IFilterProxy), ProxyType.OPWebUIApi);

            builder.RegisterType<AdminApiAuthorizeFilterProxy>()
                   .As<IFilterProxy>()
                   .WithMetadata(nameof(IFilterProxy), ProxyType.AdminAuthorizationApi);
        }
    }
}