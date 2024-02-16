using ICP.Modules.Mvc.Admin.Attributes;
using ICP.Modules.Mvc.Admin.Commands;
using ICP.Modules.Mvc.Admin.Enums;
using ICP.Modules.Mvc.Admin.Models;
using ICP.Modules.Mvc.Admin.Models.ViewModels;
using System;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Admin.Controllers
{
    public class FunctionSwitchController : BaseAdminController
    {
        FunctionSwitchCommand _functionSwitchCommand;

        public FunctionSwitchController(
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

            var list = _functionSwitchCommand.ListFunctionCategory(FunctionName);

            return View(list);
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Edit)]
        public ActionResult UpdateFunctionSwitch(int FunctionID, byte Status)
        {
            var result = _functionSwitchCommand.UpdateFunctionCategoryStatus(FunctionID, Status, CurrentUser.Account, RealIP, ProxyIP);

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
        public ActionResult FunctionSwitchRev(int FunctionID, string FunctionName)
        {
            var list = _functionSwitchCommand.ListFunctionCategoryStatusRev(FunctionID);

            ViewBag.FunctionID = FunctionID;
            ViewBag.FunctionName = FunctionName;

            return View(list);
        }

        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Add)]
        public ActionResult AddFunctionSwitchRev(int RevID, int FunctionID, string FunctionName)
        {
            FunctionCategoryStatusRev model;
            if (RevID == 0)
            {
                model = new FunctionCategoryStatusRev
                {
                    RevID = 0,
                    FunctionID = FunctionID,
                    FunctionStatus = 0
                };
            }
            else
            {
                model = _functionSwitchCommand.GetFunctionCategoryStatusRev(RevID);
            }

            var viewModel = new AddFunctionSwitchRevVM
            {
                FunctionCategoryStatusRev = model,
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
        public ActionResult AddFunctionSwitchRev(AddFunctionSwitchRevVM model)
        {
            var result = _functionSwitchCommand.AddFunctionCategoryStatusRev(model, CurrentUser.Account, RealIP, ProxyIP);

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
        public ActionResult QueryFunctionCategoryLog(int FunctionID, string FunctionName)
        {
            var list = _functionSwitchCommand.ListFunctionCategoryLog(FunctionID);

            ViewBag.FunctionName = FunctionName;

            return View(list);
        }

        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Delete)]
        public ActionResult DeleteFunctionSwitchRev(int RevID)
        {
            var result = _functionSwitchCommand.DeleteFunctionCategoryStatusRev(RevID, CurrentUser.Account, RealIP, ProxyIP);

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
