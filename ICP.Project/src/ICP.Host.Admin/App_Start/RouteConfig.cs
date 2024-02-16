using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ICP.Host.Admin.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                    name: "Default",
                    url: "{controller}/{action}/{id}",
                    defaults: new { area = "Admin", controller = "Frame", action = "Index", id = UrlParameter.Optional },
                    namespaces: new string[] { "ICP.Modules.Mvc.Admin.Controllers" })
                  .DataTokens.Add("area", "Admin");
        }
    }
}