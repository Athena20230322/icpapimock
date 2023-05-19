using ICP.Modules.Mvc.Admin.Attributes;
using ICP.Modules.Mvc.Admin.Commands;
using ICP.Modules.Mvc.Admin.Enums;
using ICP.Modules.Mvc.Admin.Models.ViewModels.MemberIdentityVerificationCount;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Admin.Controllers
{
    public class MemberIdentityVerificationCountController : BaseAdminController
    {
        MemberIdentityVerificationCountCommand _memberIdentityVerificationCountCommand;

        public MemberIdentityVerificationCountController(MemberIdentityVerificationCountCommand memberIdentityVerificationCountCommand)
        {
            _memberIdentityVerificationCountCommand = memberIdentityVerificationCountCommand;
        }

        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult Index()
        {
            //預設上個月第一天~這個月最後一天
            DateTime LastTime = DateTime.Today.AddMonths(-1);
            DateTime NextTime = DateTime.Today.AddMonths(1);
            var viewModel = new QueryVM
            {
                StartDate = new DateTime(LastTime.Year, LastTime.Month, 1),
                EndDate = new DateTime(NextTime.Year, NextTime.Month, 1).AddDays(-1)
            };

            return View(viewModel);
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult Query(QueryVM query)
        {
            var result = _memberIdentityVerificationCountCommand.ListMemberIdentityVerificationCount(query);
            ViewBag.QueryModel = query;
            return View(result);
        }

        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Export)]
        public ActionResult ExportExcel(QueryVM query)
        {
            MemoryStream file = _memberIdentityVerificationCountCommand.ExportExcel(query);
            file.Flush();
            file.Position = 0;

            return File(file, "application/ms-excel", $"P11P33Verify_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xls");
        }
    }
}
