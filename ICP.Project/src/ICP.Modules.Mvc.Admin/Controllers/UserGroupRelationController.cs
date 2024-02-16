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
    using Services;
    using Commands;

    public class UserGroupRelationController : BaseAdminController
    {
        UserGroupRelationCommand _userGroupRelationCommand;
        UserGroupService _userGroupService;
        UserService _userService;
        DepartmentService _departmentService;

        public UserGroupRelationController(UserGroupRelationCommand userGroupRelationCommand, UserGroupService userGroupService, UserService userService, DepartmentService departmentService)
        {
            _userGroupRelationCommand = userGroupRelationCommand;
            _userGroupService = userGroupService;
            _userService = userService;
            _departmentService = departmentService;
        }

        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult Index()
        {
            return View();
        }

        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult Query(UserGroupQuery model)
        {
            model.Visible = null;
            model.PageSize = int.MaxValue;
            var list = _userGroupService.ListUserGroup(model);
            return View(list);
        }

        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult QueryUsers(int id)
        {
            int UserGroupID = id;

            var group = _userGroupService.GetUserGroup(UserGroupID);
            if (group == null) return Content(string.Empty);
            
            var list = _userService.ListUserByGroup(UserGroupID);

            ViewBag.group = group;

            return View(list);
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult QueryNotJoinUsers(int id, int DeptID)
        {
            int UserGroupID = id;
            var list = _userGroupRelationCommand.QueryNotJoinUsersByDeptID(UserGroupID, DeptID);
            var result = list.Select(t => new
            {
                t.UserID,
                t.Account,
                t.CName
            });
            return Json(result);
        }

        #region 新增成員
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Add)]
        public ActionResult Add(int id)
        {
            int UserGroupID = id;
            ViewBag.ListDepartment = _departmentService.ListDepartment();
            Add_ViewBag(UserGroupID);
            return View();
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Add)]
        public ActionResult Add(int id, int UserID)
        {
            int UserGroupID = id;
            var result = _userGroupRelationCommand.AddUserGroupRelation(UserGroupID, UserID);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.RtnMsg);
                Add_ViewBag(UserGroupID);
                return View();
            }

            if (Request.IsAjaxRequest())
            {
                return Json(result);
            }
            else
            {
                return RedirectAndAlert(Url.Action("Add", new { id }), "成功");
            }
        }

        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Add)]
        public ActionResult BatchAdd(int id)
        {
            int UserGroupID = id;
            Add_ViewBag(UserGroupID);
            return View();
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Add)]
        public ActionResult BatchAdd(int id, List<int> UserIDs)
        {
            int UserGroupID = id;
            var result = _userGroupRelationCommand.BatchAddUserGroupRelation(UserGroupID, UserIDs);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.RtnMsg);
                Add_ViewBag(UserGroupID);
                return View();
            }

            if (Request.IsAjaxRequest())
            {
                return Json(result);
            }
            else
            {
                return RedirectAndAlert(Url.Action("BatchAdd", new { id }), "成功");
            }
        }

        private void Add_ViewBag(int UserGroupID)
        {
            ViewBag.UserGroupID = UserGroupID;
            ViewBag.ListDepartment = _departmentService.ListDepartment();
        }
        #endregion

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Delete)]
        public ActionResult Remove(int UserGroupID, int UserID)
        {
            var result = _userGroupRelationCommand.DeleteUserGroupRelation(UserGroupID, UserID);
            return Json(result);
        }
    }
}
