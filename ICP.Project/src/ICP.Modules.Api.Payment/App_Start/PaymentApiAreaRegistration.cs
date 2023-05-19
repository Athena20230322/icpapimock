using System.Web.Mvc;

namespace ICP.Modules.Api.Payment.App_Start
{
    public class PaymentApiAreaRegistration : AreaRegistration
    {
        public override string AreaName => "PaymentApi";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name: "PaymentApiArea",
                url: "api/Payment/{controller}/{action}",
                namespaces: new string[] { "ICP.Modules.Api.Payment.Controllers" });

            context.MapRoute(
                name: "PaymentAppArea",
                url: "app/{controller}/{action}",
                namespaces: new string[] { "ICP.Modules.Api.Payment.Controllers" });
        }
    }
}
