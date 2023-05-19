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
    using Infrastructure.Core.Web.Extensions;

    public class UserController : BaseAdminController
    {
        UserCommand _userCommand;
        DepartmentService _departmentService;
        UserGroupService _userGroupService;

        public UserController(UserCommand userCommand, DepartmentService departmentService, UserGroupService userGroupService)
        {
            _userCommand = userCommand;
            _departmentService = departmentService;
            _userGroupService = userGroupService;
        }

        #region 查詢
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult Index()
        {
            ViewBag.ListUserGroup = _userGroupService.ListUserGroup();
            return View();
        }

        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult Query(UserQuery query)
        {
            //UserStatus 預設為 1, 這邊改成 null 查所有帳號
            query.UserStatus = null;
            var list = _userCommand.ListUser(query);

            ViewBag.ListDepartment = _departmentService.ListDepartment();

            return PagedListView(list, query);
        }

        [HttpPost]
        public JsonResult QueryByDeptID(int id)
        {
            int DeptID = id;
            var query = new UserQuery
            {
                PageSize = int.MaxValue,
                DeptID = id
            };
            var list = _userCommand.ListUser(query);
            var result = list.Select(t => new
            {
                t.UserID,
                t.Account,
                t.CName
            });
            return Json(result);
        }

        [HttpPost]
        public JsonResult QueryByGroupID(int id)
        {
            int UserGroupID = id;
            var list = _userCommand.ListUserByGroup(UserGroupID);
            var result = list.Select(t => new
            {
                t.UserID,
                t.Account,
                t.CName
            });
            return Json(result);
        }
        #endregion

        #region 新增
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Add)]
        public ActionResult Add()
        {
            var model = new User();
            model.UserStatus = 1;

            Add_ViewBag();
            return View(model);
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Add)]
        public ActionResult Add(User model)
        {
            if (!ModelState.IsValid)
            {
                Add_ViewBag();
                return View(model);
            }

            var result = _userCommand.AddUser(model, RealIP);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.RtnMsg);
                Add_ViewBag();
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
        private void Add_ViewBag()
        {
            ViewBag.ListDepartment = _departmentService.ListDepartment();
        }
        #endregion

        #region 更新
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Edit)]
        public ActionResult Edit(int id)
        {
            int UserID = id;
            var result = _userCommand.GetUser(UserID);
            if (!result.IsSuccess) return HttpNotFound();
            Edit_ViewBag();
            return View(result.RtnData);
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Edit)]
        public ActionResult Edit(int id, User model)
        {
            Edit_ViewBag();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int UserID = id;
            var result = _userCommand.UpdateUser(UserID, model, RealIP);
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
        private void Edit_ViewBag()
        {
            ViewBag.ListDepartment = _departmentService.ListDepartment();
        }
        #endregion

        #region 移除
        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Delete)]
        public JsonResult Remove(int id)
        {
            int UserID = id;
            var result = _userCommand.DeleteUser(UserID);
            return Json(result);
        }
        #endregion
    }
}
