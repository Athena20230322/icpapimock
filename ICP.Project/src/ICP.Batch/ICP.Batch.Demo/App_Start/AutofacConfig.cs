using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.Demo.App_Start
{
    using Commands;
    using Infrastructure.Core.Frameworks;

    public static class AutofacConfig
    {
        public static IContainer Register()
        {
            // 容器建立者
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterModule<LogModule>();
            builder.RegisterType<DemoBatchCommand>();

            return builder.Build();
        }
    }
}
