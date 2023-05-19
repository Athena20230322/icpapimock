using ICP.Infrastructure.Core.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Payment.Controllers
{
    public class TestController : BaseMvcController
    {
        public ActionResult Test()
        {
            return Content(base.GetType().FullName);
        }

        public ActionResult TestView()
        {
            return View();
        }
    }
}
