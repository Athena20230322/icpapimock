using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web.Mvc;

namespace ICP.Modules.Mvc.Admin.Controllers
{
    using Infrastructure.Core.Models;
    using Models;
    using Enums;
    using Attributes;
    using Commands;

    public class UserGroupController : BaseAdminController
    {
        UserGroupCommand _UserGroupCommand;

        public UserGroupController(UserGroupCommand UserGroupCommand)
        {
            _UserGroupCommand = UserGroupCommand;
        }

        #region 查詢
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult Index()
        {
            return View();
        }

        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult Query(UserGroupQuery query)
        {
            var list = _UserGroupCommand.ListUserGroup(query);
            return PagedListView(list, query);
        }
        #endregion

        #region 新增
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Add)]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Add)]
        public ActionResult Add(UserGroup model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = _UserGroupCommand.AddUserGroup(model);
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
                return RedirectAndAlert(Url.Action("Edit", new { id = result.RtnData }), "成功");
            }
        }
        #endregion

        #region 修改
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Edit)]
        public ActionResult Edit(int id)
        {
            int UserGroupID = id;
            var model = _UserGroupCommand.GetUserGroup(UserGroupID);
            if (model == null) return HttpNotFound();
            return View(model);
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Edit)]
        public ActionResult Edit(int id, UserGroup model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int UserGroupID = id;
            var result = _UserGroupCommand.UpdateUserGroup(UserGroupID, model);
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
                return RedirectAndAlert(Url.Action("Edit", new { id }), "成功");
            }
        }
        #endregion

        #region 移除
        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Delete)]
        public JsonResult Remove(int id)
        {
            int UserGroupID = id;
            var result = _UserGroupCommand.DeleteUserGroup(UserGroupID);
            return Json(result);
        }
        #endregion
    }
}
