using Autofac;
using Autofac.Integration.Web;
using ICP.Infrastructure.Core.Web.Frameworks;
using ICP.Modules.Api.AccountLink.App_Start;
using ICP.Modules.Api.Member.App_Start;
using ICP.Modules.Api.Payment.App_Start;
using System.Reflection;

namespace ICP.Host.Middleware.DbProxy.App_Start
{
    public static class AutofacConfig
    {
        /// <summary>
        /// 註冊DI注入物件資料
        /// </summary>
        public static IContainerProvider Register()
        {
            // 容器建立者
            ContainerBuilder builder = new ContainerBuilder();

            // 載入模組
            builder.RegisterModule<DefaultHostModule>();
            builder.RegisterModule<MemberApiModule>();
            builder.RegisterModule<PaymentApiModule>();
            builder.RegisterModule<AccountLinkApiModule>();

            // 建立容器
            IContainer container = builder.Build();

            return new ContainerProvider(container);
        }
    }
}