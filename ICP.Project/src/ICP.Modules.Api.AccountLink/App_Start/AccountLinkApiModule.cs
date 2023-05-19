using Autofac;
using Autofac.Extras.DynamicProxy;
using Autofac.Integration.Mvc;
using ICP.Infrastructure.Core.Frameworks.AOP;
using ICP.Library.Commands.App_Start;
using ICP.Library.Models.AccountLinkApi.Enums;
using ICP.Library.Repositories.App_Start;
using ICP.Library.Services.App_Start;
using ICP.Modules.Api.AccountLink.Commands;
using ICP.Modules.Api.AccountLink.Factories;
using System.Linq;

namespace ICP.Modules.Api.AccountLink.App_Start
{
    public class AccountLinkApiModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterControllers(ThisAssembly);

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(x => x.FullName.StartsWith("ICP.Modules.Api.AccountLink.Commands") ||
                               x.FullName.StartsWith("ICP.Modules.Api.AccountLink.Services"))
                   .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(x => x.FullName.StartsWith("ICP.Modules.Api.AccountLink.Repositories"))
                   .InstancePerLifetimeScope()
                   .EnableClassInterceptors()
                   .InterceptedBy(typeof(DbProxyInterceptor));

            builder.RegisterType<ACLinkFactory>();

            //中國信託
            builder.RegisterType<ChinaTrustCommand>()
                   .As<BaseACLinkCommand>()
                   .WithMetadata("bankType", BankType.ChinaTrust);

            //第一銀行
            builder.RegisterType<FirstCommand>()
                   .As<BaseACLinkCommand>()
                   .WithMetadata("bankType", BankType.First);

            //國泰世華
            builder.RegisterType<CathayCommand>()
                   .As<BaseACLinkCommand>()
                   .WithMetadata("bankType", BankType.Cathay);


            builder.RegisterModule<CommandLibraryModule>();
            builder.RegisterModule<ServiceLibraryModule>();
            builder.RegisterModule<RepositoryLibraryModule>();
        }
    }
}
