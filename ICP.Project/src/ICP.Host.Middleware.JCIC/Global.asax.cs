using ICP.Host.Middleware.JCIC.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace ICP.Host.Middleware.JCIC
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            AutofacConfig.Register();
            LoggerConfig.Register();
        }
    }
}