using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Admin.Attributes;
using ICP.Modules.Mvc.Admin.Commands;
using ICP.Modules.Mvc.Admin.Enums;
using ICP.Modules.Mvc.Admin.Models.Announcement;
using ICP.Modules.Mvc.Admin.Models.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Admin.Controllers
{
    public class AnnouncementController : BaseAdminController
    {
        AnnouncementCommand _announcementCommand;

        public AnnouncementController(AnnouncementCommand announcementCommand)
        {
            _announcementCommand = announcementCommand;
        }

        #region 訊息公告內容

        #region 訊息公告內容清單
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult Query(ListAnnounceDbReq model)
        {
            List<ListAnnounceDbRes> list = _announcementCommand.ListContent(model);
            model.PageSize = 20;

            return PagedListView(list, model);
        }
        #endregion

        #region 新增訊息公告
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Add)]
        public ActionResult AddAnnounce()
        {
            ModifyAnnounceVM model = new ModifyAnnounceVM();
            model.CategoryList = _announcementCommand.ListCategoryItem();

            return View(model);
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Add)]
        public ActionResult AddAnnounce(ModifyAnnounceVM model)
        {
            model.CategoryList = _announcementCommand.ListCategoryItem();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = _announcementCommand.AddAnnounce(CurrentUser.Account, RealIP, ProxyIP, model);
            if (!result.IsSuccess)
            {
                TempData["RtnMsg"] = result.RtnMsg;
                return View(model);
            }

            return RedirectAndAlert(Url.Action("Index"), "新增成功");
        }
        #endregion

        #region 修改訊息公告
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Edit)]
        public ActionResult EditAnnounce(int id)
        {
            ModifyAnnounceVM model = new ModifyAnnounceVM();

            var result = _announcementCommand.GetAnnounce(id);
            if (!result.IsSuccess)
            {
                return RedirectAndAlert(Url.Action("Index"), result.RtnMsg);
            }

            model = result.RtnData;
            model.CategoryList = _announcementCommand.ListCategoryItem();

            return View(model);
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Edit)]
        public ActionResult EditAnnounce(ModifyAnnounceVM model)
        {
            model.CategoryList = _announcementCommand.ListCategoryItem();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = _announcementCommand.EditAnnounce(CurrentUser.Account, RealIP, ProxyIP, model);
            if (!result.IsSuccess)
            {
                TempData["RtnMsg"] = result.RtnMsg;
                return View(model);
            }

            return RedirectAndAlert(Url.Action("Index"), "修改成功");
        }
        #endregion

        #region 刪除訊息公告
        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Delete)]
        public JsonResult DeleteAnnounce(int id)
        {
            var result = _announcementCommand.DeleteAnnounce(CurrentUser.Account, RealIP, ProxyIP, id);
            return Json(result);
        }
        #endregion

        #region 審核訊息公告
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Check)]
        public ActionResult AuthAnnounce(int id)
        {
            ModifyAnnounceVM model = new ModifyAnnounceVM();

            var result = _announcementCommand.GetAnnounce(id);
            if (!result.IsSuccess)
            {
                return RedirectAndAlert(Url.Action("Index"), result.RtnMsg);
            }

            model = result.RtnData;

            return View(model);
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Check)]
        public ActionResult AuthAnnounce(ModifyAnnounceVM model)
        {
            var result = _announcementCommand.AuthAnnounce(CurrentUser.Account, RealIP, ProxyIP, model);
            if (!result.IsSuccess)
            {
                TempData["RtnMsg"] = result.RtnMsg;
                return View(model);
            }

            return RedirectAndAlert(Url.Action("Index"), "審核完畢");
        }
        #endregion

        #region 檔案上傳
        [HttpPost]
        public JsonResult UploadImage(UploadImageVM model)
        {
            DataResult<object> result = new DataResult<object>();
            result.SetError();

            if (!ModelState.IsValid)
            {
                return Json(result);
            }

            result = _announcementCommand.UploadFile("images", model.ImageFile);

            return Json(result);
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Add)]
        public JsonResult UploadMidFile(UploadFileVM model)
        {
            DataResult<object> result = new DataResult<object>();
            result.SetError();

            if (!ModelState.IsValid)
            {
                return Json(result);
            }

            #region 上傳文件
            var uploadResult = _announcementCommand.UploadFile("csv", model.MidFile);
            if (!uploadResult.IsSuccess)
            {
                return Json(uploadResult);
            }
            #endregion

            #region 檢查文件內容
            dynamic iData = uploadResult.RtnData;
            var checkResult = _announcementCommand.OpenCSV(iData.path);
            if (!checkResult.IsSuccess)
            {
                result.RtnCode = checkResult.RtnCode;
                result.RtnMsg = checkResult.RtnMsg;
                return Json(result);
            }
            #endregion

            result.SetSuccess(uploadResult.RtnData);

            return Json(result);
        }
        #endregion

        #region 測試發送
        [HttpPost]
        public JsonResult SendTest(ModifyAnnounceVM model)
        {
            var result = new BaseResult();
            result.SetError();

            if (model.TestMidList.Replace(" ", "").Length == 0)
            {
                result.RtnMsg = "請輸入測試發送電支帳號，多組帳號中間請以「，」隔開";
                return Json(result);
            }

            result = _announcementCommand.AddNotifyMessage(model);
            return Json(result);
        }
        #endregion

        #endregion

        #region 訊息公告類別

        #region 訊息公告類別清單
        [UserLoginAuth(MappingMethod = "Category", Action = MappingMethodAction.Query)]
        public ActionResult Category()
        {
            return View();
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Category", Action = MappingMethodAction.Query)]
        public ActionResult CategoryQuery(ListCategoryDbReq model)
        {
            List<ListCategoryDbRes> list = _announcementCommand.ListCategory(model);
            return PagedListView(list, model);
        }
        #endregion

        #region 新增公告類別
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Add)]
        public ActionResult AddCategory()
        {
            return View(new ModifyCategoryVM());
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Add)]
        public ActionResult AddCategory(ModifyCategoryVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = _announcementCommand.AddCategory(CurrentUser.Account, RealIP, ProxyIP, model);
            if (!result.IsSuccess)
            {
                TempData["RtnMsg"] = result.RtnMsg;
                return View(model);
            }

            return RedirectAndAlert(Url.Action("Category"), "新增成功");
        }
        #endregion

        #region 修改公告類別
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Edit)]
        public ActionResult EditCategory(int id)
        {
            var result = _announcementCommand.GetCategory(id);
            if (!result.IsSuccess)
            {
                return HttpNotFound();
            }

            return View(result.RtnData);
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Edit)]
        public ActionResult EditCategory(int id, ModifyCategoryVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.CategoryID = id;
            var result = _announcementCommand.EditCategory(CurrentUser.Account, RealIP, ProxyIP, model);
            if (!result.IsSuccess)
            {
                TempData["RtnMsg"] = result.RtnMsg;
                return View(model);
            }

            return RedirectAndAlert(Url.Action("Category"), "修改成功");
        }
        #endregion

        #region 刪除公告類別
        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Delete)]
        public JsonResult DeleteCategory(int id)
        {
            ModifyCategoryVM model = new ModifyCategoryVM()
            {
                CategoryID = id,
                CategoryName = "",
                Status = 2
            };
            var result = _announcementCommand.EditCategory(CurrentUser.Account, RealIP, ProxyIP, model);

            return Json(result);
        }
        #endregion

        #endregion
    }
}