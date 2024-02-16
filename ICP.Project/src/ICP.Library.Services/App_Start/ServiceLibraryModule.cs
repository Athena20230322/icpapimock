using Autofac;
using Autofac.Extras.DynamicProxy;
using ICP.Infrastructure.Core.Frameworks.AOP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Services.App_Start
{
    public class ServiceLibraryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                   .InstancePerLifetimeScope()
                   .EnableClassInterceptors();
        }
    }
}
