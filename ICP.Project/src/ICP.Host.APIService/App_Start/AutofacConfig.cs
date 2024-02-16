using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Autofac.Integration.Mvc;
using ICP.Infrastructure.Core.Frameworks.AOP;
using ICP.Infrastructure.Core.Web.Frameworks;
using ICP.Library.Repositories.SystemRepositories;
using ICP.Modules.Api.TinyURL.App_Start;
using System.Web.Mvc;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using ICP.Modules.Api.CheckEinvoiceToken;

namespace ICP.Host.APIService.App_Start
{
    public class AutofacConfig
    {
        /// <summary>
        /// 註冊DI注入物件資料
        /// </summary>
        public static void Register()
        {
            // 容器建立者
            ContainerBuilder builder = new ContainerBuilder();

            var ThisAssembly = Assembly.GetExecutingAssembly();
            string sNamespace = ThisAssembly.GetName().Name;

            // 載入模組
            builder.RegisterModule<DefaultHostModule>();
            builder.RegisterType<ConfigKeyValueRepository>();
            builder.RegisterModule<TinyURLApiModule>();
            builder.RegisterModule<CheckEinvoiceTokenModule>();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(type => type.FullName.StartsWith($"{sNamespace}.Commands") ||
                               type.FullName.StartsWith($"{sNamespace}.Services"));

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(x => x.FullName.StartsWith($"{sNamespace}.Repositories"))
                .EnableClassInterceptors()
                .InterceptedBy(typeof(DbProxyInterceptor));

            // 建立容器
            IContainer container = builder.Build();

            // 解析容器內的型別
            AutofacDependencyResolver resolver = new AutofacDependencyResolver(container);

            // 建立相依解析器
            DependencyResolver.SetResolver(resolver);

            // Set the service locator to an AutofacServiceLocator.
            var csl = new AutofacServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => csl);
        }
    }
}