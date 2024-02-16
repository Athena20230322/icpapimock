using Autofac;
using Autofac.Integration.Mvc;
using ICP.Infrastructure.Core.Web.Attributes;
using System.Web.Mvc;

namespace ICP.Infrastructure.Core.Web.Frameworks
{
    public class HandleErrorRequestModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HandleErrorRequestAttribute>()
                   .AsExceptionFilterFor<Controller>();
        }
    }
}
