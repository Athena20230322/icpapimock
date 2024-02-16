using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Autofac;
using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Web.Extensions;

namespace ICP.Infrastructure.Core.Web.Frameworks
{
    public class WebGlobalAppSettingModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {            
            builder.Register((context) => 
                        GlobalAppSetting.Create(HttpContext.Current.GetRequestId))
                   .InstancePerLifetimeScope();
        }
    }
}
