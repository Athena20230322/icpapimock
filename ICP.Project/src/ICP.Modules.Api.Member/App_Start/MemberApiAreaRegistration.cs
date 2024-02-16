using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ICP.Modules.Api.Member.App_Start
{
    public class MemberApiAreaRegistration : AreaRegistration
    {
        public override string AreaName => "MemberApi";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name: "MemberAppArea",
                url: "app/{controller}/{action}",
                namespaces: new string[] { "ICP.Modules.Api.Member.Controllers" });

            context.MapRoute(
                name: "MemberApiArea",
                url: "api/Member/{controller}/{action}",
                namespaces: new string[] { "ICP.Modules.Api.Member.Controllers" });

            //context.MapRoute(
            //    name: "DefaultArea",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { id = UrlParameter.Optional },
            //    namespaces: new string[] { "ICP.Modules.Api.Member.Controllers" });
        }
    }
}
