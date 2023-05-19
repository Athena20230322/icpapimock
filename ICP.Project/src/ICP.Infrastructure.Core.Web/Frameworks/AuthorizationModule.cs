using Autofac;
using ICP.Infrastructure.Abstractions.Authorization;
using ICP.Infrastructure.Core.Web.Frameworks.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Core.Web.Frameworks
{
    public class AuthorizationModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AuthorizationFactory>()
                   .As<IAuthorizationFactory>();
        }
    }
}
