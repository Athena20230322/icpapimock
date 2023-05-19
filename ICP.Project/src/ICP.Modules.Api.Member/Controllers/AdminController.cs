using ICP.Infrastructure.Abstractions.FilterProxy;
using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Web.Attributes;
using ICP.Infrastructure.Core.Web.Controllers;
using ICP.Modules.Api.Member.Commands;
using ICP.Modules.Api.Member.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ICP.Modules.Api.Member.Controllers
{
    public class AdminController : BaseController
    {
        AdminCommand _adminCommand;

        public AdminController(AdminCommand adminCommand)
        {
            _adminCommand = adminCommand;
        }

        [HttpPost]
        [LogRequest(LogTextResponse = true)]
        public ActionResult CloseMemberAccount(CloseMemberAccountRequest request)
        {
            byte source = 2;
            var result = _adminCommand.CloseMemberAccount(request.MID, source, RealIP, ProxyIP, request.Modifier);

            return Json(result);
        }
    }
}
