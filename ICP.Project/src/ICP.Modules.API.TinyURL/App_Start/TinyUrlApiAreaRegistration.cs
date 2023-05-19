using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ICP.Modules.Api.TinyURL.App_Start
{
    public class PaymentCenterApiAreaRegistration : AreaRegistration
    {
        public override string AreaName => "TinyURLApi";

        public override void RegisterArea(AreaRegistrationContext context)
        {           
            context.MapRoute(
                name: "RedirectUrl",
                url: "{tinyurl}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "ICP.Modules.Api.TinyURL.Controllers" }).DataTokens["UseNamespaceFallback"] = false;

            context.MapRoute(
               name: "Default",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
               namespaces: new string[] { "ICP.Modules.Api.TinyURL.Controllers" }).DataTokens["UseNamespaceFallback"] = false;

           

        }
    }
}
