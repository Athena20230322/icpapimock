using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

namespace ICP.Modules.Mvc.Admin.Commands
{
    using Infrastructure.Core.Models;
    using Models;
    using Models.ViewModels;
    using Services;

    public class PermissionsCommand
    {
        PrivilegeService _privilegeService;
        FunctionCategoryService _functionCategoryService;

        public PermissionsCommand(PrivilegeService privilegeService, FunctionCategoryService functionCategoryService)
        {
            _privilegeService = privilegeService;
            _functionCategoryService = functionCategoryService;
        }

        public List<PermissionViewModel> ListEditUserGroupFunction(int CurrentUserID, int UserGroupID, int UserID)
        {
            var listFunc = _functionCategoryService.ListFunction();

            var currentUserFunc = _privilegeService.ListUserFunction(CurrentUserID, IncludeGroupFunction: 1);

            var targetFunc = _privilegeService.ListUserGroupFunction(UserGroupID, UserID);

            var targetPermissions = (
                from a in listFunc
                join b in targetFunc
                on a.FunctionID equals b.FunctionID into c
                from b in c.DefaultIfEmpty()
                select new
                {
                    func = a,
                    ActionSum = b != null ? b.ActionSum : 0
                })
                .ToList().Select(t => _privilegeService.CheckPermission(t.func.FunctionID, t.func.FunctionGroupID, t.ActionSum, t.func.ActionSum)).ToList();

            List<FunctionCatagory> targetGroupFunc = null;

            if (UserGroupID > 0 && UserID > 0)
            {
                targetGroupFunc = _privilegeService.ListUserGroupFunction(UserGroupID, UserID: 0);
            }

            var list = _privilegeService.ListEditUserGroupFunction(targetPermissions, targetGroupFunc, currentUserFunc, listFunc);

            return _privilegeService.ListMenuItem(list);
        }

        public BaseResult UpdateUserGroupFunctionPermission(int CurrentUserID, int UserGroupID, int UserID, List<UserGroupFunctionPermission> list)
        {
            var listFunc = _functionCategoryService.ListFunction();

            var currentUserFunc = _privilegeService.ListUserFunction(CurrentUserID, IncludeGroupFunction: 1);

            var targetFunc = list.Select(t => new FunctionCatagory { FunctionID = t.FunctionID, ActionSum = t.ActionSum }).ToList();

            var targetPermissions = targetFunc.Select(t => _privilegeService.CheckPermission(t.FunctionID, t.FunctionGroupID, t.ActionSum, t.ActionSum)).ToList();

            List<FunctionCatagory> targetGroupFunc = null;

            if (UserGroupID > 0 && UserID > 0)
            {
                targetGroupFunc = _privilegeService.ListUserGroupFunction(UserGroupID, UserID: 0);
            }

            var permissionModels = _privilegeService.ListEditUserGroupFunction(targetPermissions, targetGroupFunc, currentUserFunc, listFunc);

            return _privilegeService.UpdateUserGroupFunctionPermission(UserGroupID, UserID, permissionModels, listFunc);

        }
    }
}
