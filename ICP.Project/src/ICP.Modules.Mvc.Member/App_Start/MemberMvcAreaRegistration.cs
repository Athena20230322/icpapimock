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
        public override string AreaName => "Member";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            // Captcha (BotDetect)
            context.Routes.IgnoreRoute("{*botdetect}", new { botdetect = @"(.*)BotDetectCaptcha\.ashx" });

            context.MapRoute(
                name: "MemberMvcArea",
                url: "Member/{controller}/{action}/{id}",
                defaults: new { controller = "Test", action = "Test", id = UrlParameter.Optional },
                namespaces: new string[] { "ICP.Modules.Mvc.Member.Controllers" });
        }
    }
}
