using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web.Mvc;

namespace ICP.Modules.Mvc.Admin.Controllers
{
    using Attributes;
    using Models;
    using Commands;
    using Infrastructure.Core.Web.Extensions;
    using Modules.Mvc.Admin.Models.MailLibrary;
    using Modules.Mvc.Admin.Enums;
    using Infrastructure.Core.Models;

    public class MailLibraryManageController : BaseAdminController
    {
        MailLibraryManageCommand _mailLibraryManageCommand;

        public MailLibraryManageController(
            MailLibraryManageCommand mailLibraryManageCommand
            )
        {
            _mailLibraryManageCommand = mailLibraryManageCommand;
        }

        #region 查詢
        //[UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        //[UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult Query(ContentQueryModel query)
        {
            var list = _mailLibraryManageCommand.QueryContent(query);

            return PagedListView(list, query);
        }
        #endregion

        #region MAIL

        #region 新增
        //[UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Add)]
        public ActionResult AddMail(int layout = 0, long NotifyID = 0)
        {
            ViewBag.NotifyID = NotifyID;

            ViewBag.layout = layout == 1;

            if (NotifyID > 0)
            {
                var result = _mailLibraryManageCommand.GetAddMailContent(NotifyID);
                if (result.IsSuccess)
                {
                    return View(result.RtnData);
                }
            }

            return View();
        }

        [HttpPost]
        //[UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Add)]
        public ActionResult AddMail(MailContentModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = _mailLibraryManageCommand.AddMailContent(model, CurrentUser.Account);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.RtnMsg);
                return View(model);
            }

            if (Request.IsAjaxRequest())
            {
                return Json(result);
            }
            else
            {
                return RedirectAndAlert(Url.Action("EditMail", new { id = result.RtnData }), "成功");
            }
        }
        #endregion

        #region 修改
        //[UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Edit)]
        public ActionResult EditMail(long id)
        {
            var result = _mailLibraryManageCommand.GetMailContent(id);

            if (!result.IsSuccess)
            {
                return HttpNotFound();
            }

            var model = result.RtnData;

            ViewBag.layout = model.Body.StartsWith("<!DOCTYPE") || model.Body.StartsWith("<html>");

            return View(model);
        }

        [HttpPost]
        //[UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Edit)]
        public ActionResult EditMail(long id, MailContentModel model, int layout = 0)
        {
            var result = _mailLibraryManageCommand.UpdateMailContent(id, model, CurrentUser.Account);

            ViewBag.layout = layout;

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.RtnMsg);
                return View(model);
            }

            if (Request.IsAjaxRequest())
            {
                return Json(result);
            }
            else
            {
                return RedirectAndAlert(Url.Action("EditMail", new { id }), "成功");
            }
        }
        #endregion

        #region 刪除
        [HttpPost]
        //[UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Delete)]
        public JsonResult DeleteMail(long id)
        {
            var result = _mailLibraryManageCommand.DeleteMailContent(id);

            return Json(result);
        }
        #endregion

        #region 測試
        //[UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult TestMail(long id)
        {
            var list = _mailLibraryManageCommand.ListMailTag(id);

            ViewBag.MailID = id;
            ViewBag.UserEmail = CurrentUser.UserEmail;

            return View(list);
        }

        [HttpPost]
        //[UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult TestMail(long id, FormCollection form)
        {
            string to = form["mailto"];

            if (string.IsNullOrWhiteSpace(to))
            {
                return Json(new BaseResult { RtnMsg = "mailto required" });
            }

            Dictionary<string, string> dict = new Dictionary<string, string>();

            foreach (string key in form.Keys)
            {
                if (key == "mailto") continue;

                dict.Add(key, form[key]);
            }

            var result = _mailLibraryManageCommand.TestMail(to, id, dict);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.RtnMsg);
                return TestMail(id);
            }

            if (Request.IsAjaxRequest())
            {
                return Json(result);
            }
            else
            {
                return RedirectAndAlert(Url.Action("EditMail", new { id }), "成功");
            }
        }
        #endregion

        #region 預覽
        public ActionResult PreViewMail(long id, FormCollection form)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            foreach (string key in form.Keys)
            {
                if (key == "mailto") continue;

                dict.Add(key, form[key]);
            }

            var result = _mailLibraryManageCommand.PreViewMail(id, dict);
            if (!result.IsSuccess)
            {
                return Content(result.RtnMsg);
            }

            var content = result.RtnData;
            return Content(content.Body);
        }
        #endregion

        #region ListTag
        //[UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult ListMailTag(long id)
        {
            List<MailTag> list = _mailLibraryManageCommand.ListMailTag(id);

            return PartialView(list);
        }
        #endregion

        #region AddTag
        //[UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Add)]
        public ActionResult AddMailTag(long id)
        {
            MailTag model = new MailTag { MailID = id };

            return View(model);
        }

        [HttpPost]
        //[UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Add)]
        public ActionResult AddMailTag(long id, MailTag model)
        {
            var result = _mailLibraryManageCommand.AddMailTag(id, model);

            if (!result.IsSuccess)
            {
                return Json(result);
            }

            if (Request.IsAjaxRequest())
            {
                return Json(result);
            }
            else
            {
                return RedirectAndAlert(Url.Action("EditMailTag", new { id = result.RtnData }), "成功");
            }
        }
        #endregion

        #region UpdateTag
        [HttpPost]
        //[UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Edit)]
        public ActionResult EditMailTag(long id, MailTag model)
        {
            var result = _mailLibraryManageCommand.UpdateMailTag(id, model);

            if (!result.IsSuccess)
            {
                return Json(result);
            }

            if (Request.IsAjaxRequest())
            {
                return Json(result);
            }
            else
            {
                return RedirectAndAlert(Url.Action("EditMailTag", new { id }), "成功");
            }
        }
        #endregion

        #region DeleteTag
        [HttpPost]
        //[UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Edit)]
        public JsonResult DeleteMailTag(long id)
        {
            var result = _mailLibraryManageCommand.DeleteMailTag(id);

            return Json(result);
        }
        #endregion

        #endregion

        #region Notify

        #region 新增
        //[UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Add)]
        public ActionResult AddNotify(int layout = 0, long MailID = 0)
        {
            ViewBag.MailID = MailID;

            ViewBag.layout = layout == 1;

            if (MailID > 0)
            {
                var result = _mailLibraryManageCommand.GetAddNotifyContent(MailID);
                if (result.IsSuccess)
                {
                    return View(result.RtnData);
                }
            }

            return View();
        }

        [HttpPost]
        //[UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Add)]
        public ActionResult AddNotify(NotifyContentModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = _mailLibraryManageCommand.AddNotifyContent(model, CurrentUser.Account);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.RtnMsg);
                return View(model);
            }

            if (Request.IsAjaxRequest())
            {
                return Json(result);
            }
            else
            {
                return RedirectAndAlert(Url.Action("EditNotify", new { id = result.RtnData }), "成功");
            }
        }
        #endregion

        #region 修改
        //[UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Edit)]
        public ActionResult EditNotify(long id)
        {
            var result = _mailLibraryManageCommand.GetNotifyContent(id);

            if (!result.IsSuccess)
            {
                return HttpNotFound();
            }

            var model = result.RtnData;

            ViewBag.layout = model.Body.StartsWith("<!DOCTYPE") || model.Body.StartsWith("<html>");

            return View(model);
        }

        [HttpPost]
        //[UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Edit)]
        public ActionResult EditNotify(long id, NotifyContentModel model, int layout = 0)
        {
            var result = _mailLibraryManageCommand.UpdateNotifyContent(id, model, CurrentUser.Account);

            ViewBag.layout = layout;

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.RtnMsg);
                return View(model);
            }

            if (Request.IsAjaxRequest())
            {
                return Json(result);
            }
            else
            {
                return RedirectAndAlert(Url.Action("EditNotify", new { id }), "成功");
            }
        }
        #endregion

        #region 刪除
        [HttpPost]
        //[UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Delete)]
        public JsonResult DeleteNotify(long id)
        {
            var result = _mailLibraryManageCommand.DeleteNotifyContent(id);

            return Json(result);
        }
        #endregion

        #region ListTag
        //[UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult ListNotifyTag(long id)
        {
            List<NotifyTag> list = _mailLibraryManageCommand.ListNotifyTag(id);

            return PartialView(list);
        }
        #endregion

        #region AddTag
        //[UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Add)]
        public ActionResult AddNotifyTag(long id)
        {
            NotifyTag model = new NotifyTag { NotifyID = id };

            return View(model);
        }

        [HttpPost]
        //[UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Add)]
        public ActionResult AddNotifyTag(long id, NotifyTag model)
        {
            var result = _mailLibraryManageCommand.AddNotifyTag(id, model);

            if (!result.IsSuccess)
            {
                return Json(result);
            }

            if (Request.IsAjaxRequest())
            {
                return Json(result);
            }
            else
            {
                return RedirectAndAlert(Url.Action("EditNotifyTag", new { id = result.RtnData }), "成功");
            }
        }
        #endregion

        #region UpdateTag
        [HttpPost]
        //[UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Edit)]
        public ActionResult EditNotifyTag(long id, NotifyTag model)
        {
            var result = _mailLibraryManageCommand.UpdateNotifyTag(id, model);

            if (!result.IsSuccess)
            {
                return Json(result);
            }

            if (Request.IsAjaxRequest())
            {
                return Json(result);
            }
            else
            {
                return RedirectAndAlert(Url.Action("EditNotifyTag", new { id }), "成功");
            }
        }
        #endregion

        #region DeleteTag
        [HttpPost]
        //[UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Edit)]
        public JsonResult DeleteNotifyTag(long id)
        {
            var result = _mailLibraryManageCommand.DeleteNotifyTag(id);

            return Json(result);
        }
        #endregion

        #endregion
    }
}
