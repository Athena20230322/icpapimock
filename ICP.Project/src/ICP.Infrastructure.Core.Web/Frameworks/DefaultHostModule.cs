using Autofac;
using ICP.Infrastructure.Core.Frameworks;
using ICP.Infrastructure.Core.Frameworks.AOP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Core.Web.Frameworks
{
    public class DefaultHostModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<EmailSenderModule>();
            builder.RegisterModule<WebGlobalAppSettingModule>();
            builder.RegisterModule<HandleErrorRequestModule>();
            builder.RegisterModule<LogModule>();
            builder.RegisterModule<DbUtilModule>();
            builder.RegisterModule<FilterProxyModule>();
            builder.RegisterModule<AuthorizationModule>();
            builder.RegisterModule<ResultMapperModule>();

            builder.RegisterType<ValidatableObjectInterceptor>();
            builder.RegisterType<DbProxyInterceptor>();
        }
    }
}
