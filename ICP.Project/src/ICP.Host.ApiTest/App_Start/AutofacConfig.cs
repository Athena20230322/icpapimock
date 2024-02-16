using Autofac;
using Autofac.Integration.Mvc;
using ICP.Infrastructure.Core.Frameworks;
using ICP.Infrastructure.Core.Web.Frameworks;
using ICP.Library.Repositories.App_Start;
using ICP.Library.Services.App_Start;
using System.Reflection;
using System.Web.Mvc;

namespace ICP.Host.ApiTest.App_Start
{
    public static class AutofacConfig
    {
        /// <summary>
        /// 註冊DI注入物件資料
        /// </summary>
        public static void Register()
        {
            var ThisAssembly = Assembly.GetExecutingAssembly();

            string sNamespace = ThisAssembly.GetName().Name;

            // 容器建立者
            ContainerBuilder builder = new ContainerBuilder();

            // 註冊 Controllers
            builder.RegisterControllers(ThisAssembly);

            // 攔截Attribute注入屬性
            builder.RegisterFilterProvider();

            // 載入模組
            builder.RegisterModule<DefaultHostModule>();

            builder.RegisterControllers(ThisAssembly);

            // Library 注入管理
            builder.RegisterModule<ServiceLibraryModule>();
            builder.RegisterModule<RepositoryLibraryModule>();

            // Controller 注入管理
            builder.RegisterControllers(ThisAssembly);

            //排程 Commands, Services 注入管理
            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(type => type.FullName.StartsWith(string.Format("{0}.Commands", sNamespace)) ||
                                  type.FullName.StartsWith(string.Format("{0}.Services", sNamespace)) ||
                                  type.FullName.StartsWith(string.Format("{0}.Repositories", sNamespace)));

            // 建立容器
            IContainer container = builder.Build();

            // 解析容器內的型別
            AutofacDependencyResolver resolver = new AutofacDependencyResolver(container);

            // 建立相依解析器
            DependencyResolver.SetResolver(resolver);
        }
    }
}