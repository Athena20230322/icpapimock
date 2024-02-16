using Autofac;
using ICP.Infrastructure.Core.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.ExecSendFETSMS.App_Start
{
    using Commands;
    using Services;

    public static class AutofacConfig
    {
        public static IContainer Register()
        {
            // 容器建立者
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterModule<LogModule>();
            builder.RegisterType<SMSCommand>();
            builder.RegisterType<SMSService>();

            return builder.Build();
        }
    }
}
