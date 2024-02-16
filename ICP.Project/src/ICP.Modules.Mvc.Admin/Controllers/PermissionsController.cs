using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web.Mvc;

namespace ICP.Modules.Mvc.Admin.Controllers
{
    using Models;
    using Enums;
    using Attributes;
    using Commands;
    using Services;
    using Infrastructure.Core.Models;

    public class PermissionsController : BaseAdminController
    {
        PermissionsCommand _permissionsCommand;
        UserGroupService _userGroupService;
        FrameService _frameService;

        public PermissionsController(PermissionsCommand permissionsCommand, UserGroupService userGroupService, FrameService frameService)
        {
            _permissionsCommand = permissionsCommand;
            _userGroupService = userGroupService;
            _frameService = frameService;
        }

        #region 群組權限管理

        #region 查詢
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult Index()
        {
            ViewBag.GroupUser = false;
            ViewBag.ListUserGroup = _userGroupService.ListUserGroup();

            return View();
        }
        #endregion

        #region 編輯
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult Edit(int UserGroupID)
        {
            var list = _permissionsCommand.ListEditUserGroupFunction(base.CurrentUserID, UserGroupID, UserID: 0);

            return View(list);
        }


        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Edit)]
        public ActionResult Edit(int UserGroupID, List<UserGroupFunctionPermission> model)
        {
            int UserID = 0;

            var result = _permissionsCommand.UpdateUserGroupFunctionPermission(base.CurrentUserID, UserGroupID, UserID, model);

            if (Request.IsAjaxRequest())
            {
                return Json(result);
            }
            else
            {
                return RedirectAndAlert(Url.Action("Edit", new { UserGroupID }), "成功");
            }
        }
        #endregion

        #endregion

        #region 群組內個人權限設定

        #region 查詢
        [UserLoginAuth(MappingMethod = "IndexGroupUser", Action = MappingMethodAction.Query)]
        public ActionResult IndexGroupUser()
        {
            ViewBag.GroupUser = true;
            ViewBag.ListUserGroup = _userGroupService.ListUserGroup();

            return View("Index");
        }
        #endregion

        #region 編輯
        [UserLoginAuth(MappingMethod = "IndexGroupUser", Action = MappingMethodAction.Query)]
        public ActionResult EditGroupUser(int UserGroupID, int UserID)
        {
            var list = _permissionsCommand.ListEditUserGroupFunction(base.CurrentUserID, UserGroupID, UserID);

            return View("Edit", list);
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "IndexGroupUser", Action = MappingMethodAction.Edit)]
        public ActionResult EditGroupUser(int UserGroupID, int UserID, List<UserGroupFunctionPermission> model)
        {
            var result = _permissionsCommand.UpdateUserGroupFunctionPermission(base.CurrentUserID, UserGroupID, UserID, model);

            if (Request.IsAjaxRequest())
            {
                return Json(result);
            }
            else
            {
                return RedirectAndAlert(Url.Action("Edit", new { UserGroupID, UserID }), "成功");
            }
        }
        #endregion

        #endregion
    }
}
