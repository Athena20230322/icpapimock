using System;
using System.Collections.Generic;
using ICP.Host.Middleware.Mail.App_Start;
using Autofac.Integration.Web;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace ICP.Host.Middleware.Mail
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            AutofacConfig.Register();
        }
    }
}