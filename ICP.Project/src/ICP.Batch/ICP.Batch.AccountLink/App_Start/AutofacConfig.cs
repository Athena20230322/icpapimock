using Autofac;
using Autofac.Extras.DynamicProxy;
using ICP.Batch.AccountLink.Commands;
using ICP.Batch.AccountLink.Factories;
using ICP.Infrastructure.Core.Frameworks;
using ICP.Infrastructure.Core.Frameworks.AOP;
using ICP.Library.Models.AccountLinkApi.Enums;
using ICP.Library.Repositories.App_Start;
using ICP.Library.Services.App_Start;
using System.Linq;
using System.Reflection;

namespace ICP.Batch.AccountLink.App_Start
{
    public static class AutofacConfig
    {
        public static IContainer Register()
        {
            var ThisAssembly = Assembly.GetExecutingAssembly();

            string sNamespace = ThisAssembly.GetName().Name;

            // 容器建立者
            ContainerBuilder builder = new ContainerBuilder();

            //預設排程 注入管理
            builder.RegisterModule<DefaultBatchModule>();

            //Library 注入管理
            builder.RegisterModule<ServiceLibraryModule>();
            builder.RegisterModule<RepositoryLibraryModule>();

            //排程 Commands, Services 注入管理
            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(type => type.FullName.StartsWith(string.Format("{0}.Commands", sNamespace)) ||
                                  type.FullName.StartsWith(string.Format("{0}.Services", sNamespace)));

            //排程 Repositories 注入管理, DbProxy 攔截
            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(x => x.FullName.StartsWith(string.Format("{0}.Repositories", sNamespace)))
                   .EnableClassInterceptors()
                   .InterceptedBy(typeof(DbProxyInterceptor));

            builder.RegisterType<BankFactory>();

            //第一銀行
            builder.RegisterType<FirstCommand>()
                   .As<BaseCommand>()
                   .WithMetadata("bankType", BankType.First);

            return builder.Build();
        }
    }
}
