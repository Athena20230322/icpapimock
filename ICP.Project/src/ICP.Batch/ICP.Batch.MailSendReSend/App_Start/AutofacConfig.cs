using Autofac;
using Autofac.Extras.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.MailSendReSend.App_Start
{
    using ICP.Infrastructure.Core.Models;
    using Infrastructure.Core.Frameworks;
    using Infrastructure.Core.Frameworks.AOP;
    using Library.Repositories.App_Start;
    using Library.Services.App_Start;


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
            

            builder.RegisterType<Autofac.Core.Activators.Reflection.DefaultConstructorFinder>();
            builder.RegisterType<ICP.Batch.MailSendReSend.Service.ReSendMailService>();
            builder.RegisterType<GlobalAppSetting>();
            //排程 Commands, Services 注入管理
            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(type => type.FullName.StartsWith(string.Format("{0}.Commands", sNamespace)) ||
                                  type.FullName.StartsWith(string.Format("{0}.Services", sNamespace)));

            //排程 Repositories 注入管理, DbProxy 攔截
            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(x => x.FullName.StartsWith(string.Format("{0}.Repositories", sNamespace)))
                   .EnableClassInterceptors()
                   .InterceptedBy(typeof(DbProxyInterceptor));

            return builder.Build();
        }
    }
}
