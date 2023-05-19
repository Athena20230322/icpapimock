using ICP.Modules.Mvc.Admin.Commands;
using ICP.Modules.Mvc.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Admin.Controllers
{
    public class AppManagementController: BaseAdminController
    {
        Repositories.ConfigRepository _configRepository;
        AppManagementCommand _appManagementCommand;

        public AppManagementController(
            Repositories.ConfigRepository configRepository,
            AppManagementCommand appManagementCommand)
        {
            _configRepository = configRepository;
            _appManagementCommand = appManagementCommand;
        }

        #region APP設定清單
        public ActionResult ListAPPSetting()
        {
            return View();
        }

        public ActionResult QueryAPPSetting()
        {
            var list = _appManagementCommand.ListAPPSetting();

            return PartialView(list);
        }
        #endregion

        #region 新增APP設定
        public ActionResult AddAPPSetting()
        {
            ViewBag.MaxVersion = _appManagementCommand.GetAPPSettingMaxVersion();
            return View();
        }

        [HttpPost]
        public ActionResult AddAPPSetting(APPSetting model)
        {
            string APPXMLPath = Server.MapPath(_configRepository.APPXMLPath);
            
            var result = _appManagementCommand.AddAPPSetting(model, APPXMLPath, CurrentUser.Account);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.RtnMsg);
                ViewBag.MaxVersion = _appManagementCommand.GetAPPSettingMaxVersion();
                return View(model);
            }

            if (Request.IsAjaxRequest())
            {
                return Json(result);
            }
            else
            {
                return RedirectAndAlert(Url.Action("UpdateAPPSetting", new { id = model.VersionNo }), "成功");
            }
        }
        #endregion

        #region 發佈APP設定
        public JsonResult PublishAPPSetting(byte id)
        {
            string APPXMLPath = Server.MapPath(_configRepository.APPXMLPath);

            byte VersionNo = id;

            var result = _appManagementCommand.PublishAPPSetting(VersionNo, APPXMLPath, CurrentUser.Account);
            return Json(result);
        }
        #endregion

        #region 更新APP設定
        public ActionResult UpdateAPPSetting(byte id)
        {
            byte VersionNo = id;

            var result = _appManagementCommand.GetAPPSetting(VersionNo);
            if (!result.IsSuccess)
            {
                return HttpNotFound();
            }

            return View(result.RtnData);
        }

        [HttpPost]
        public ActionResult UpdateAPPSetting(byte id, APPSetting model)
        {
            string APPXMLPath = Server.MapPath(_configRepository.APPXMLPath);

            model.VersionNo = id;

            var result = _appManagementCommand.UpdateAPPSetting(model, APPXMLPath, CurrentUser.Account);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.RtnMsg);
                return View(model);
            }

            if (Request.IsAjaxRequest())
            {
                return Json(result);
            }
            else
            {
                return RedirectAndAlert(Url.Action("UpdateAPPSetting", new { id = model.VersionNo }), "成功");
            }
        }
        #endregion

        #region 修改歷程
        public ActionResult ListAPPXMLSettingLog(byte id)
        {
            byte VersionNo = id;

            var list = _appManagementCommand.ListAPPXMLSettingLog(VersionNo);

            return View(list);
        }
        #endregion
    }
}
