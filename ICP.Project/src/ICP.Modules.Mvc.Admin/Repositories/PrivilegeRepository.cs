using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Repositories
{
    using Infrastructure.Abstractions.DbUtil;
    using Infrastructure.Core.Models;
    using Models;

    public class PrivilegeRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        private IDbConnection db;

        public PrivilegeRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;

            db = _dbConnectionPool.Create(DatabaseName.ICP_Admin);
        }

        /// <summary>
        /// 更新使用者功能權限
        /// </summary>
        /// <param name="UserGroupID"></param>
        /// <param name="UserID"></param>
        /// <param name="JSON"></param>
        /// <returns></returns>
        public BaseResult UpdateUserGroupFunctionPermission(int UserGroupID, int UserID, string JSON)
        {
            string sql = "EXEC ausp_Admin_Privilege_UpdateUserGroupFunctionPermission_IUD";
            var args = new
            {
                UserGroupID,
                UserID,
                JSON
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
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
            string sql = "EXEC ausp_Admin_Privilege_GetFunctionActionByUser_S";
            var args = new
            {
                UserID,
                ControllerName,
                MethodName
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<int>(sql, args);
        }

        /// <summary>
        /// 取得使用者特殊權限
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public UserPermissionSpecial GetPermissionSpecialByUser(int UserID)
        {
            string sql = "EXEC ausp_Admin_Privilege_GetPermissionSpecialByUser_S";
            var args = new
            {
                UserID
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<UserPermissionSpecial>(sql, args);
        }

        /// <summary>
        /// 取得使用者功能權限
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="IncludeGroupFunction">包含群組功能權限</param>
        /// <returns></returns>
        public List<FunctionCatagory> ListUserFunction(int UserID, int IncludeGroupFunction = 1)
        {
            string sql = "EXEC ausp_Admin_Privilege_ListUserFunction_S";
            var args = new
            {
                UserID,
                IncludeGroupFunction
            };
            sql += db.GenerateParameter(args);
            return db.Query<FunctionCatagory>(sql, args);
        }

        /// <summary>
        /// 取得功能群組功能權限
        /// </summary>
        /// <param name="UserGroupID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<FunctionCatagory> ListUserGroupFunction(int UserGroupID, int UserID)
        {
            string sql = "EXEC ausp_Admin_Privilege_ListUserGroupFunction_S";
            var args = new
            {
                UserGroupID,
                UserID
            };
            sql += db.GenerateParameter(args);
            return db.Query<FunctionCatagory>(sql, args);
        }

        /// <summary>
        /// 取得功能開關狀態
        /// </summary>
        /// <param name="ControllerName"></param>
        /// <param name="MethodName"></param>
        /// <returns></returns>
        public byte GetFunctionStatus(string ControllerName, string MethodName)
        {
            string sql = "EXEC ausp_Admin_Privilege_GetFunctionStatus_S";
            var args = new
            {
                ControllerName,
                MethodName
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<byte>(sql, args);
        }
    }
}
