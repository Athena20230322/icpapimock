using System.Web.Mvc;
using ICP.Modules.Api.CheckEinvoiceToken.Controllers;

namespace ICP.Modules.Api.CheckEinvoiceToken
{
    public class CheckEinvoiceAreaRegistration:AreaRegistration
    {
        public override string AreaName => "Einvoice";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name: "CheckEinvoiceToken",
                url: "Einvoice/Query/checkIssueToken",
                defaults: new { controller = "CheckEinvoiceToken", action = "Index"},
                namespaces:new string[]{"ICP.Modules.Api.CheckEinvoiceToken.Controllers"}).DataTokens["UseNamespaceFallback"] = false;;

    }
    }
}