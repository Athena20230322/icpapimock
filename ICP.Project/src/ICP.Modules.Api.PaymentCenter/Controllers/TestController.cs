using ICP.Infrastructure.Core.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ICP.Modules.Api.PaymentCenter.Controllers
{
    public class TestController : BaseApiController
    {
        public ActionResult Test()
        {
            return Content(base.GetType().FullName);
        }
    }
}
