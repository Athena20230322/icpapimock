using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Member.App_Start
{
    public class MemberMvcAreaRegistration : AreaRegistration
    {
        public override string AreaName => "Admin";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name: "AdminMvcArea",
                url: "Admin/{controller}/{action}/{id}",
                defaults: new { controller = "Frame", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "ICP.Modules.Mvc.Admin.Controllers" });
        }
    }
}
