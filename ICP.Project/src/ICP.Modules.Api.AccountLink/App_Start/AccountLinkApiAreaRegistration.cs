using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ICP.Modules.Api.AccountLink.App_Start
{
    public class AccountLinkApiAreaRegistration : AreaRegistration
    {
        public override string AreaName => "AccountLinkApi";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name: "AccountLinkApiArea",
                url: "api/AccountLink/{controller}/{action}/{id}",
                defaults: new { controller = "Test", action = "Test", id = UrlParameter.Optional },
                namespaces: new string[] { "ICP.Modules.Api.AccountLink.Controllers" });
        }
    }
}
