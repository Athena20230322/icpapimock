using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ICP.Modules.Api.PaymentCenter.App_Start
{
    public class PaymentCenterApiAreaRegistration : AreaRegistration
    {
        public override string AreaName => "PaymentCenterApi";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name: "PaymentCenterApiArea",
                url: "api/PaymentCenter/{controller}/{action}/{id}",
                defaults: new { id = UrlParameter.Optional },
                namespaces: new string[] { "ICP.Modules.Api.PaymentCenter.Controllers" });
        }
    }
}
