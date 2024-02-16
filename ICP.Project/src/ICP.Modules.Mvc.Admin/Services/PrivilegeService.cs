using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using AutoMapper;

namespace ICP.Modules.Mvc.Admin.Services
{
    using Infrastructure.Core.Models;
    using Models;
    using Models.ViewModels;
    using Enums;
    using Modules.Mvc.Admin.Repositories;

    public class PrivilegeService
    {
        private readonly PrivilegeRepository _repository = null;

        public PrivilegeService(PrivilegeRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// 更新使用者功能權限
        /// </summary>
        /// <param name="UserGroupID"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public BaseResult UpdateUserGroupFunctionPermission(int UserGroupID, int UserID, List<PermissionModel> permissionModels, List<FunctionCatagory> listFunc)
        {
            #region 加入上一層查詢權限
            (
                from a in
                (
                    from a in permissionModels.Select(t => t.FunctionGroupID).Distinct().ToList()
                    join b in permissionModels
                    on a equals b.FunctionID into c
                    from b in c.DefaultIfEmpty()
                    where b == null
                    select a
                )
                join b in listFunc
                on a equals b.FunctionID
                select b
            ).ToList().ForEach(t => 
            {
                if (!permissionModels.Exists(p => p.FunctionID == t.FunctionID))
                {
                    permissionModels.Add(new PermissionModel { FunctionID = t.FunctionID, FunctionGroupID = t.FunctionGroupID, Query = true });
                }

                if (!permissionModels.Exists(p => p.FunctionID == t.FunctionGroupID))
                {
                    permissionModels.Add(new PermissionModel { FunctionID = t.FunctionGroupID, FunctionGroupID = t.FunctionGroupID, Query = true });
                }
            });
            #endregion

            #region 用原本權限來修改, 避免異動到 127 以上的值
            var listOrign = _repository.ListUserGroupFunction(UserGroupID, UserID);

            var list =
            (
                from a in permissionModels
                join b in listOrign
                on a.FunctionID equals b.FunctionID
                select new
                {
                    permissionModel = a,
                    orign = b
                }
            ).Select(t => 
            {
                int orignActionSum = t.orign.ActionSum;
                int ActionSum = orignActionSum
                    - CheckedAction(CheckedAction(orignActionSum, MappingMethodAction.Query), false, MappingMethodAction.Query)
                    - CheckedAction(CheckedAction(orignActionSum, MappingMethodAction.Add), false, MappingMethodAction.Add)
                    - CheckedAction(CheckedAction(orignActionSum, MappingMethodAction.Edit), false, MappingMethodAction.Edit)
                    - CheckedAction(CheckedAction(orignActionSum, MappingMethodAction.Delete), false, MappingMethodAction.Delete)
                    - CheckedAction(CheckedAction(orignActionSum, MappingMethodAction.Check), false, MappingMethodAction.Check)
                    - CheckedAction(CheckedAction(orignActionSum, MappingMethodAction.Import), false, MappingMethodAction.Import)
                    - CheckedAction(CheckedAction(orignActionSum, MappingMethodAction.Export), false, MappingMethodAction.Export)
                    ;

                var p = t.permissionModel;

                return new UserGroupFunctionPermission
                {
                    FunctionGroupID = p.FunctionGroupID,
                    FunctionID = p.FunctionID,
                    ActionSum = 
                        ActionSum +
                        CheckedAction(p.Query, p.QueryDisable, MappingMethodAction.Query) +
                        CheckedAction(p.Add, p.QueryDisable, MappingMethodAction.Add) +
                        CheckedAction(p.Edit, p.QueryDisable, MappingMethodAction.Edit) +
                        CheckedAction(p.Delete, p.QueryDisable, MappingMethodAction.Delete) +
                        CheckedAction(p.Check, p.QueryDisable, MappingMethodAction.Check) +
                        CheckedAction(p.Import, p.QueryDisable, MappingMethodAction.Import) +
                        CheckedAction(p.Export, p.QueryDisable, MappingMethodAction.Export)
                };
            }).ToList();
            #endregion

            string JSON = JsonConvert.SerializeObject(list.Select(t => new { t.FunctionID, t.ActionSum }).ToList());
            return _repository.UpdateUserGroupFunctionPermission(UserGroupID, UserID, JSON);
        }

        /// <summary>
        /// 取得使用者功能權限
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="ControllerName"></param>
        /// <param name="MethodName"></param>
        /// <returns></returns>
        public int GetFunctionActionByUser(int UserID, string ControllerName, string MethodName)
        {
            return _repository.GetFunctionActionByUser(UserID, ControllerName, MethodName);
        }

        /// <summary>
        /// 取得使用者功能權限
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="IncludeGroupFunction">包含群組功能權限</param>
        /// <returns></returns>
        public List<FunctionCatagory> ListUserFunction(int UserID, int IncludeGroupFunction = 1)
        {
            return _repository.ListUserFunction(UserID, IncludeGroupFunction);
        }

        /// <summary>
        /// 取得功能群組功能權限
        /// </summary>
        /// <param name="UserGroupID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<FunctionCatagory> ListUserGroupFunction(int UserGroupID, int UserID)
        {
            return _repository.ListUserGroupFunction(UserGroupID, UserID);
        }

        private bool CheckedAction(int ActoinSum, MappingMethodAction Action)
        {
            int ActionID = (int)Action;
            return (ActoinSum & ActionID) == ActionID;
        }

        private int CheckedAction(bool enable, bool actionDisable, MappingMethodAction Action)
        {
            if (actionDisable || !enable) return 0;

            return (int)Action;
        }

        public PermissionModel CheckPermission(int FunctionID, int FunctionGroupID, int targetActionSum, int targetGroupActionSum)
        {
            return new PermissionModel
            {
                FunctionGroupID = FunctionGroupID,
                FunctionID = FunctionID,

                Query = CheckedAction(targetActionSum, MappingMethodAction.Query),
                QueryDisable = !CheckedAction(targetGroupActionSum, MappingMethodAction.Query),

                Add = CheckedAction(targetActionSum, MappingMethodAction.Add),
                AddDisable = !CheckedAction(targetGroupActionSum, MappingMethodAction.Add),

                Edit = CheckedAction(targetActionSum, MappingMethodAction.Edit),
                EditDisable = !CheckedAction(targetGroupActionSum, MappingMethodAction.Edit),

                Delete = CheckedAction(targetActionSum, MappingMethodAction.Delete),
                DeleteDisable = !CheckedAction(targetGroupActionSum, MappingMethodAction.Delete),

                Check = CheckedAction(targetActionSum, MappingMethodAction.Check),
                CheckDisable = !CheckedAction(targetGroupActionSum, MappingMethodAction.Check),

                Import = CheckedAction(targetActionSum, MappingMethodAction.Import),
                ImportDisable = !CheckedAction(targetGroupActionSum, MappingMethodAction.Import),

                Export = CheckedAction(targetActionSum, MappingMethodAction.Export),
                ExportDisable = !CheckedAction(targetGroupActionSum, MappingMethodAction.Export)
            };
        }

        private void CheckPermissionDisable(ref List<PermissionModel> list, List<FunctionCatagory> targetGroupFunc)
        {
            (
                from a in list
                join b in targetGroupFunc
                on a.FunctionID equals b.FunctionID into c
                from b in c.DefaultIfEmpty()
                select new { a, ActionSum = b != null ? b.ActionSum : 0 }
            ).ToList().ForEach(t =>
            {
                var obj = t.a;

                int targetGroupActionSum = t.ActionSum;

                if (!obj.QueryDisable && !CheckedAction(targetGroupActionSum, MappingMethodAction.Query))
                {
                    obj.QueryDisable = true;
                }

                if (!obj.AddDisable && !CheckedAction(targetGroupActionSum, MappingMethodAction.Add))
                {
                    obj.AddDisable = true;
                }

                if (!obj.EditDisable && !CheckedAction(targetGroupActionSum, MappingMethodAction.Edit))
                {
                    obj.EditDisable = true;
                }

                if (!obj.DeleteDisable && !CheckedAction(targetGroupActionSum, MappingMethodAction.Delete))
                {
                    obj.DeleteDisable = true;
                }

                if (!obj.CheckDisable && !CheckedAction(targetGroupActionSum, MappingMethodAction.Check))
                {
                    obj.CheckDisable = true;
                }

                if (!obj.ImportDisable && !CheckedAction(targetGroupActionSum, MappingMethodAction.Import))
                {
                    obj.ImportDisable = true;
                }

                if (!obj.ExportDisable && !CheckedAction(targetGroupActionSum, MappingMethodAction.Export))
                {
                    obj.ExportDisable = true;
                }
            });
        }

        public List<PermissionModel> ListEditUserGroupFunction(
            List<PermissionModel> targetPermissions,
            List<FunctionCatagory> targetGroupFunc,
            List<FunctionCatagory> currentUserFunc,
            List<FunctionCatagory> listFunc
            )
        {
            if (targetGroupFunc != null) CheckPermissionDisable(ref targetPermissions, targetGroupFunc);
            CheckPermissionDisable(ref targetPermissions, currentUserFunc);
            CheckPermissionDisable(ref targetPermissions, listFunc);

            targetPermissions =
            (
                from a in listFunc
                join b in targetPermissions
                on a.FunctionID equals b.FunctionID
                select new { a, b }
            ).Select(t => 
            {
                var func = t.a;
                var model = t.b;

                model.FunctionGroupID = func.FunctionGroupID;
                model.FunctionLevel = func.FunctionLevel;
                model.FunctionName = func.FunctionName;

                return model;
            }).ToList();

            return targetPermissions;
        }

        public List<PermissionViewModel> ListMenuItem(List<PermissionModel> userFunctions)
        {
            var items = Mapper.Map<List<PermissionViewModel>>(userFunctions);

            var rootTree = items.Where(f => f.FunctionLevel == 1).ToList();
            foreach (var rootFuc in rootTree)
            {
                var sencondTree = items.Where(t => t.FunctionGroupID == rootFuc.FunctionID && t.FunctionGroupID != t.FunctionID).ToList();
                foreach (var secondFuc in sencondTree)
                {
                    var thirdTree = items.Where(t => t.FunctionGroupID == secondFuc.FunctionID && t.FunctionGroupID != t.FunctionID).ToList();
                    secondFuc.ChildrenFunction = thirdTree;
                }

                rootFuc.ChildrenFunction = sencondTree;
            }

            return rootTree;
        }

        /// <summary>
        /// 取得使用者特殊權限
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public UserPermissionSpecial GetPermissionSpecialByUser(int UserID)
        {
            return _repository.GetPermissionSpecialByUser(UserID);
        }

        /// <summary>
        /// 檢查特殊權限
        /// </summary>
        /// <param name="permission"></param>
        /// <param name="checkType"></param>
        /// <returns></returns>
        public bool CheckPermissionSpecialByUser(UserPermissionSpecial permission, UserPermissionSpecialType checkType)
        {
            if (permission == null) return false;

            switch (checkType)
            {
                case UserPermissionSpecialType.PersonalInfoView:
                    return (permission.PersonalInfoActionSum & 1) == 1;
            }

            return false;
        }

        /// <summary>
        /// 檢查特殊權限
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="checkType"></param>
        /// <returns></returns>
        public bool CheckPermissionSpecialByUser(int UserID, UserPermissionSpecialType checkType)
        {
            var permission = _repository.GetPermissionSpecialByUser(UserID);

            return CheckPermissionSpecialByUser(permission, checkType);
        }

        /// <summary>
        /// 取得功能開關狀態
        /// </summary>
        /// <param name="ControllerName"></param>
        /// <param name="MethodName"></param>
        /// <returns></returns>
        public byte GetFunctionStatus(string ControllerName, string MethodName)
        {
            return _repository.GetFunctionStatus(ControllerName, MethodName);
        }
    }
}