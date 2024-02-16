using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Payment.App_Start
{
    public class PaymentMvcAreaRegistration : AreaRegistration
    {
        public override string AreaName => "Payment";

        public override void RegisterArea(AreaRegistrationContext context)
        {

            context.MapRoute(
                name: "defaultPaymentMvcArea",
                url: "{controller}/{action}",
                defaults: new { controller = "Test", action = "Test", id = UrlParameter.Optional },
                namespaces: new string[] { "ICP.Modules.Mvc.Payment.Controllers" });

            context.MapRoute(
                name: "PaymentMvcArea",
                url: "Payment/{controller}/{action}/{id}",
                defaults: new { controller = "Test", action = "Test", id = UrlParameter.Optional },
                namespaces: new string[] { "ICP.Modules.Mvc.Payment.Controllers" });
        }
    }
}
