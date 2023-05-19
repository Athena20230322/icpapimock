using ICP.Infrastructure.Core.Models;
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
    public class SuspenseMainController : BaseAdminController
    {
        SuspenseMainCommand _suspenseMainCommand;

        public SuspenseMainController(SuspenseMainCommand suspenseMainCommand)
        {
            _suspenseMainCommand = suspenseMainCommand;
        }

        public ActionResult Index()
        {
            return View(new QuerySuspenseMainVM());
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult Query(QuerySuspenseMainVM query)
        {
            query.PageSize = 10;
            var result = _suspenseMainCommand.ListSuspenseMain(query);
            if (!result.IsSuccess)
            {
                return RedirectAndAlert(Url.Action("Index"), result.RtnMsg);
            }

            ViewBag.QueryModel = query;
            var rtnData = result.RtnData;

            return PagedListView(rtnData, query);
        }

        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Add)]
        public ActionResult AddSuspenseMain()
        {
            var settings = _suspenseMainCommand.GetSuspenseSettingList();

            ViewBag.SuspenseTypes = settings.Item1;
            ViewBag.ReasonTypes = settings.Item2;
            ViewBag.MessageTypes = settings.Item3;

            return View();
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Add)]
        public ActionResult AddSuspenseMain(SuspenseMain model)
        {
            var result = _suspenseMainCommand.AddSuspenseMain(model, CurrentUser.Account, RealIP, ProxyIP);

            if (Request.IsAjaxRequest())
            {
                return Json(result);
            }
            else
            {
                return RedirectAndAlert(Url.Action("Index"), "成功");
            }
        }

        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Check)]
        public ActionResult SuspenseDetail(long SuspenseID)
        {
            var suspenseMain = _suspenseMainCommand.GetSuspenseMain(SuspenseID);

            ViewBag.SuspenseID = SuspenseID;
            ViewBag.AuthStatus = suspenseMain.AuthStatus;
            ViewBag.SuspenseLogs = _suspenseMainCommand.ListSuspenseMainLog(SuspenseID);

            return View(new UpdateSuspenseMainVM());
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Check)]
        public ActionResult UpdateSuspenseMain(UpdateSuspenseMainVM model)
        {
            var result = _suspenseMainCommand.UpdateSuspenseMain(model, CurrentUser.Account, RealIP, ProxyIP);

            if (Request.IsAjaxRequest())
            {
                return Json(result);
            }
            else
            {
                return RedirectAndAlert(Url.Action("SuspenseDetail", new { model.SuspenseID, model.AuthStatus }), "成功");
            }
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Check)]
        public ActionResult UnlockSuspenseMain(long SuspenseID)
        {
            var result = _suspenseMainCommand.UnlockSuspenseMain(SuspenseID, CurrentUser.Account, RealIP, ProxyIP);
            if (result.IsSuccess)
            {
                return Json(new BaseResult
                {
                    RtnCode = 1,
                    RtnMsg = "解除成功"
                });
            }

            return Json(result);
        }
        
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Export)]
        public ActionResult GetSuspenseMainExport(QuerySuspenseMainVM model)
        {
            model.PageSize = int.MaxValue;
            var xlsStream = _suspenseMainCommand.GetSuspenseMainExport(model);
            if (xlsStream == null)
                return Content(string.Empty);

            string fileName = "BanList_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            xlsStream.Flush();
            xlsStream.Position = 0;
            return File(xlsStream, "application/ms-excel", fileName);
        }
    }
}
