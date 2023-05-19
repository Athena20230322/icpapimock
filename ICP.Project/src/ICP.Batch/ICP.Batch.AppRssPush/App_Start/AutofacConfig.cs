using System.Reflection;
using Autofac;
using Autofac.Extras.DynamicProxy;
using ICP.Batch.AppRssPush.Repositories;
using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Core.Frameworks;
using ICP.Infrastructure.Core.Frameworks.AOP;
using ICP.Library.Repositories.MemberRepositories;
using ICP.Library.Repositories.SystemRepositories;

namespace ICP.Batch.AppRssPush.APP_Start
{
    using Commands;
    using Services;

    public class AutofacConfig
    {
        public static IContainer Register()
        {
            // 容器建立者
            ContainerBuilder builder = new ContainerBuilder();

            var ThisAssembly = Assembly.GetExecutingAssembly();
            string sNamespace = ThisAssembly.GetName().Name;
            //預設排程 注入管理
            builder.RegisterModule<DefaultBatchModule>();
            
            //Library
            builder.RegisterType<ConfigKeyValueRepository>();
            builder.RegisterType<MemberConfigCyptRepository>();

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