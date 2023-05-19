using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Admin.Attributes;
using ICP.Modules.Mvc.Admin.Commands;
using ICP.Modules.Mvc.Admin.Enums;
using ICP.Modules.Mvc.Admin.Models.Banner;
using ICP.Modules.Mvc.Admin.Models.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Admin.Controllers
{
    public class BannerController : BaseAdminController
    {
        BannerCommand _bannerCommand;

        public BannerController(BannerCommand bannerCommand)
        {
            _bannerCommand = bannerCommand;
        }

        #region 廣告管理清單
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult Index()
        {
            QryBannerVM model = new QryBannerVM();
            model.PageSize = 20;
            model.BannerStatusList = _bannerCommand.ListBannerStatusItem();

            return View(model);
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult Query(QryBannerVM model)
        {
            model.BannerStatusList = _bannerCommand.ListBannerStatusItem();
            model.PageSize = 20;

            var result = _bannerCommand.ListBanner(model);
            if (!result.IsSuccess)
            {
                return RedirectAndAlert(Url.Action("Index"), result.RtnMsg);
            }

            return PagedListView(result.RtnData, model);
        }
        #endregion

        #region 新增廣告
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Add)]
        public ActionResult AddBanner()
        {
            ModifyBannerVM model = new ModifyBannerVM();
            model.BannerSiteList = _bannerCommand.ListBannerSite();

            return View(model);
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Add)]
        public ActionResult AddBanner(ModifyBannerVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = _bannerCommand.AddBanner(CurrentUser.Account, model);
            if (!result.IsSuccess)
            {
                return RedirectAndAlert(Url.Action("Index"), result.RtnMsg);
            }

            return RedirectAndAlert(Url.Action("Index"), "新增成功");
        }
        #endregion

        #region 修改廣告
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Edit)]
        public ActionResult EditBanner(int id)
        {
            ModifyBannerVM model = new ModifyBannerVM();

            var result = _bannerCommand.GetBanner(id);
            if (!result.IsSuccess)
            {
                return RedirectAndAlert(Url.Action("Index"), result.RtnMsg);
            }

            model = result.RtnData;

            return View(model);
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Edit)]
        public ActionResult EditBanner(ModifyBannerVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = _bannerCommand.EditBanner(CurrentUser.Account, model);
            if (!result.IsSuccess)
            {
                return View(model);
            }

            return RedirectAndAlert(Url.Action("Index"), "修改成功");
        }
        #endregion

        #region 刪除廣告
        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Delete)]
        public JsonResult DeleteBanner(int id)
        {
            var result = _bannerCommand.DeleteBanner(CurrentUser.Account, id);
            return Json(result);
        }
        #endregion

        #region 審核廣告
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Check)]
        public ActionResult AuthBanner(int id)
        {
            ModifyBannerVM model = new ModifyBannerVM();

            var result = _bannerCommand.GetBanner(id);
            if (!result.IsSuccess)
            {
                return RedirectAndAlert(Url.Action("Index"), result.RtnMsg);
            }

            model = result.RtnData;

            return View(model);
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Check)]
        public ActionResult AuthBanner(ModifyBannerVM model)
        {
            var result = _bannerCommand.AuthBanner(CurrentUser.Account, model);
            if (!result.IsSuccess)
            {
                return RedirectAndAlert(Url.Action("Index"), result.RtnMsg);
            }

            return RedirectAndAlert(Url.Action("Index"), "審核完畢");
        }
        #endregion

        #region 圖片上傳
        [HttpPost]
        public JsonResult UploadImage(UploadImageLimit2m model)
        {
            DataResult<object> result = new DataResult<object>();
            result.SetError();

            if (!ModelState.IsValid)
            {
                result.RtnMsg = "檔案大小不得超過2M，請重新選擇";
                return Json(result);
            }

            result = _bannerCommand.UploadFile("images", model.ImageFile);

            return Json(result);
        }
        #endregion

        #region 檢查廣告排序
        [HttpPost]
        public JsonResult CheckBannerOrderID(ModifyBannerVM model)
        {
            var result = _bannerCommand.CheckBannerOrderID(model);
            return Json(result);
        }
        #endregion
    }
}
