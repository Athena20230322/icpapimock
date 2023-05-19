using Autofac;
using ICP.Infrastructure.Abstractions.FilterProxy;
using ICP.Infrastructure.Core.Web.Frameworks.FilterProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Core.Web.Frameworks
{
    public class FilterProxyModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FilterProxyFactory>()
                   .As<IFilterProxyFactory>();
        }
    }
}
