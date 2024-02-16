using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Admin.Attributes;
using ICP.Modules.Mvc.Admin.Commands;
using ICP.Modules.Mvc.Admin.Enums;
using ICP.Modules.Mvc.Admin.Models.ViewModels.CustomerServiceRecord;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Admin.Controllers
{
    public class CustomServiceRecordController : BaseAdminController
    {
        CustomServiceRecordCommand _customServiceRecordCommand;

        public CustomServiceRecordController(CustomServiceRecordCommand customServiceRecordCommand)
        {
            _customServiceRecordCommand = customServiceRecordCommand;
        }

        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult Index()
        {
            ViewBag.StatusOptions = _customServiceRecordCommand.GetStatusOptions();
            return View(new QueryCustomServiceRecordVM());
        }
        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult Query(QueryCustomServiceRecordVM model)
        {
            model.PageSize = int.MaxValue;

            return PagedListView(_customServiceRecordCommand.ListRecords(model), model);
        }

        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Add)]
        public ActionResult Add()
        {
            ViewBag.StatusOptions = _customServiceRecordCommand.GetStatusOptions();
            ViewBag.GateWaySettingOptions = _customServiceRecordCommand.GetGateWaySettingOptions();
            ViewBag.TypeSettingOptions = _customServiceRecordCommand.GetTypeSettingOptions();
            return View(new AddCustomServiceRecordVM());
        }
        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Add)]
        public ActionResult AddResult(AddCustomServiceRecordVM model)
        {
            var result = _customServiceRecordCommand.AddRecord(model, RealIP, ProxyIP, CurrentUser.CName);
            if (Request.IsAjaxRequest())
            {
                return Json(result);
            }
            else
            {
                return RedirectAndAlert(Url.Action("Add"), "成功");
            }
        }

        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult Detail(long CustomerServiceID)
        {
            ViewBag.StatusOptions = _customServiceRecordCommand.GetStatusOptions();
            var result = _customServiceRecordCommand.GetRecordAndDetail(CustomerServiceID);
            ViewBag.Result = result;
            return View(new UpdateCustomServiceRecordVM());
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Edit)]
        public ActionResult Update(long CustomerServiceID, byte? Status, string Note)
        {
            var result = _customServiceRecordCommand.UpdateRecord(CustomerServiceID, (byte)Status, Note, CurrentUser.CName, RealIP, ProxyIP);
            if (Request.IsAjaxRequest())
            {
                return Json(result);
            }
            else
            {
                return RedirectAndAlert(Url.Action("Detail", new { CustomerServiceID }), "成功");
            }
        }

        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Export)]
        public ActionResult Export(QueryCustomServiceRecordVM model)
        {
            MemoryStream file = _customServiceRecordCommand.ExportRecords(model);

            file.Flush();
            file.Position = 0;

            return File(file, "application/ms-excel", $"CustomerServiceRecord_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xls");
        }
    }
}
