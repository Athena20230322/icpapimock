using System.Web.Mvc;
using System.Web.Routing;

namespace ICP.Host.PaymentCenter.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                    name: "Default",
                    url: "{controller}/{action}/{id}",
                    defaults: new { controller = "Test", action = "Test", id = UrlParameter.Optional },
                    namespaces: new string[] { "ICP.Modules.Api.PaymentCenter" })
                  .DataTokens.Add("area", "Payment");
        }
    }
}