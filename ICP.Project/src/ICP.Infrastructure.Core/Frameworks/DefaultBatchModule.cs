using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autofac;

namespace ICP.Infrastructure.Core.Frameworks
{
    using Infrastructure.Core.Frameworks.AOP;

    public class DefaultBatchModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<EmailSenderModule>();
            builder.RegisterModule<LogModule>();
            builder.RegisterModule<DbUtilModule>();
            builder.RegisterModule<ResultMapperModule>();

            builder.RegisterType<ValidatableObjectInterceptor>();
            builder.RegisterType<DbProxyInterceptor>();
        }
    }
}
