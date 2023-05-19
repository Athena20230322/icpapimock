using ICP.Host.AccountLink.App_Start;
using ICP.Infrastructure.Core.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using ICP.Infrastructure.Core.Web.Extensions;

namespace ICP.Host.AccountLink
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // 自定義模組
            AutofacConfig.Register();
            LoggerConfig.Register();
            AutoMapperConfig.Register();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // 讓每一個請求都有自己獨立的編號
            HttpContext.Current.GenerateRequestId();
        }
    }
}