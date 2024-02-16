﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Mvc;
using ICP.Host.APIService.App_Start;

namespace ICP.Host.APIService
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            AreaRegistration.RegisterAllAreas();
            AutofacConfig.Register();
            LoggerConfig.Register();
        }
    }
}