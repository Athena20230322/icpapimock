using System.Web.Mvc;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Admin.Attributes;
using ICP.Modules.Mvc.Admin.Commands;
using ICP.Modules.Mvc.Admin.Enums;
using ICP.Modules.Mvc.Admin.Models.ViewModels;

namespace ICP.Modules.Mvc.Admin.Controllers
{
    public class MemberDeviceIdManageController : BaseAdminController
    {
        private MemberDeviceIdManageCommand _deviceIdManageCommand;

        public MemberDeviceIdManageController(MemberDeviceIdManageCommand deviceIdManageCommand)
        {
            _deviceIdManageCommand = deviceIdManageCommand;
        }


        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult Index()
        {
            return View(new MemberDeviceIdQuery());
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult Query(MemberDeviceIdQuery query)
        {
            var result = _deviceIdManageCommand.MemberDeviceIdManageList(query);

            ViewBag.QueryModel = query;

            return PagedListView(result, query);
        }


        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult EditMemberDeviceId(MemberDeviceIdVM model)
        {
            return View(model);
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Edit)]
        public ActionResult EditMemberDeviceIdResult(MemberDeviceIdVM model)
        {
            BaseResult result = new BaseResult();
            result.SetError();

            if (string.IsNullOrEmpty(model.DeviceID) ||
                string.IsNullOrEmpty(model.Memo) ||
                string.IsNullOrEmpty(CurrentUser.CName))
            {
                return Json(result);
            }

            
            result =
                _deviceIdManageCommand.AddMemberDeviceId(model.DeviceID, model.Status, CurrentUser.CName, model.Memo, RealIP, ProxyIP);

            if (Request.IsAjaxRequest())
            {
                return Json(result);
            }
            else
            {
                return RedirectAndAlert(Url.Action("EditMemberDeviceId", new {model.Status, model.DeviceID}), "成功");
            }
        }

        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult QueryMemberDeviceIdLog(string DeviceID)
        {
            var result = _deviceIdManageCommand.MemberDeviceIdVmList(DeviceID);
            return View(result);
        }

        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult AddMemberDeviceId()
        {
            return View(new MemberDeviceIdVM());
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Add)]
        public ActionResult AddMemberDeviceIdResult(MemberDeviceIdVM model)
        {
            model.Status = (int) MemberDeviceIdStatusType.Lock;
            BaseResult result = new BaseResult();
            result.SetError();

            if (string.IsNullOrEmpty(model.DeviceID) ||
                string.IsNullOrEmpty(model.Memo) ||
                string.IsNullOrEmpty(CurrentUser.CName))
            {
                return Json(result);
            }

            result =
                _deviceIdManageCommand.AddMemberDeviceId(model.DeviceID, model.Status, CurrentUser.CName, model.Memo, RealIP, ProxyIP);

            if (Request.IsAjaxRequest())
            {
                return Json(result);
            }
            else
            {
                return RedirectAndAlert(Url.Action("AddMemberDeviceId", new {model.Status, model.DeviceID}), "成功");
            }
        }
    }
}