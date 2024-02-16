using ICP.Modules.Mvc.Admin.Attributes;
using ICP.Modules.Mvc.Admin.Commands;
using ICP.Modules.Mvc.Admin.Enums;
using ICP.Modules.Mvc.Admin.Models;
using ICP.Modules.Mvc.Admin.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Admin.Controllers
{
    public class AppFunctionSwitchController : BaseAdminController
    {
        FunctionSwitchCommand _functionSwitchCommand;

        private const string appName = "icp";

        public AppFunctionSwitchController(
            FunctionSwitchCommand functionSwitchCommand
            )
        {
            _functionSwitchCommand = functionSwitchCommand;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult Query(string FunctionName)
        {
            ViewBag.FunctionName = FunctionName;

            var list = _functionSwitchCommand.ListAppFunctionSwitch(FunctionName);

            return View(list);
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Edit)]
        public ActionResult UpdateAppFunctionSwitch(int FunctionID, byte Status)
        {
            var model = new UpdateAppFunctionSwitch
            {
                AppName = appName,
                FunctionID = FunctionID,
                Status = Status
            };

            var result = _functionSwitchCommand.UpdateAppFunctionSwitch(model, CurrentUser.Account, RealIP, ProxyIP);

            if (Request.IsAjaxRequest())
            {
                return Json(result);
            }
            else
            {
                return RedirectAndAlert(Url.Action("Index"), "成功");
            }
        }

        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult AppFunctionSwitchRev(int FunctionID, string FunctionName)
        {
            var list = _functionSwitchCommand.ListAppFunctionSwitchRev(appName, FunctionID);

            ViewBag.FunctionID = FunctionID;
            ViewBag.FunctionName = FunctionName;

            return View(list);
        }

        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult AddAppFunctionSwitchRev(int RevID, int FunctionID, string FunctionName)
        {
            AppFunctionSwitchRev model;
            if (RevID == 0)
            {
                model = new AppFunctionSwitchRev
                {
                    RevID = 0,
                    FunctionID = FunctionID,
                    FunctionStatus = 0
                };
            }
            else
            {
                model = _functionSwitchCommand.GetAppFunctionSwitchRev(RevID);
            }

            var viewModel = new AddAppFunctionSwitchRevVM
            {
                AppFunctionSwitchRev = model,
                SwitchStartDate = RevID == 0 ? null : model.StartDate.ToString("yyyy-MM-dd"),
                SwitchStartTime = RevID == 0 ? null : model.StartDate.ToString("HH:mm"),
                SwitchEndDate = RevID == 0 ? null : model.EndDate.ToString("yyyy-MM-dd"),
                SwitchEndTime = RevID == 0 ? null : model.EndDate.ToString("HH:mm")
            };

            ViewBag.FunctionName = FunctionName;

            return View(viewModel);
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Add)]
        public ActionResult AddAppFunctionSwitchRev(AddAppFunctionSwitchRevVM model)
        {
            model.AppFunctionSwitchRev.AppName = appName;

            var result = _functionSwitchCommand.AddAppFunctionSwitchRev(model, CurrentUser.Account, RealIP, ProxyIP);

            if (Request.IsAjaxRequest())
            {
                return Json(result);
            }
            else
            {
                return RedirectAndAlert(Url.Action("AddFunctionSwitchRev"), "成功");
            }
        }

        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult QueryAppFunctionSwitchLog(int FunctionID, string FunctionName)
        {
            var list = _functionSwitchCommand.ListAppFunctionSwitchLog(appName, FunctionID);

            ViewBag.FunctionName = FunctionName;

            return View(list);
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Delete)]
        public ActionResult DeleteAppFunctionSwitchRev(int RevID)
        {
            var result = _functionSwitchCommand.DeleteAppFunctionSwitchRev(RevID, CurrentUser.Account, RealIP, ProxyIP);

            if (Request.IsAjaxRequest())
            {
                return Json(result);
            }
            else
            {
                return Redirect(Url.Action("Index"));
            }
        }
    }
}
