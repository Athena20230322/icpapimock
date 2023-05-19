using Autofac.Integration.Web;
using ICP.Host.Middleware.DbProxy.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace ICP.Host.Middleware.DbProxy
{
    public class Global : System.Web.HttpApplication, IContainerProviderAccessor
    {
        private static IContainerProvider _containerProvider = null;

        public IContainerProvider ContainerProvider => _containerProvider;

        protected void Application_Start(object sender, EventArgs e)
        {
            _containerProvider = AutofacConfig.Register();
            LoggerConfig.Register();
        }
    }
}