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
    using Models.ViewModels;

    public class UserGroupRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        private IDbConnection db;

        public UserGroupRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;

            db = _dbConnectionPool.Create(DatabaseName.ICP_Admin);
        }

        public DataResult<int> AddUserGroup(UserGroup model)
        {
            string sql = "EXEC ausp_Admin_AddUserGroup_I";
            var args = new
            {
                model.UserGroupCode,
                model.UserGroupName,
                model.Remark
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<DataResult<int>>(sql, args);
        }

        public List<UserGroupQueryResult> ListUserGroup(UserGroupQuery model = null)
        {
            if (model == null)
            {
                model = new UserGroupQuery
                {
                    PageNo = 1,
                    PageSize = int.MaxValue
                };
            }

            string sql = "EXEC ausp_Admin_ListUserGroup_S";
            sql += db.GenerateParameter(model);
            return db.Query<UserGroupQueryResult>(sql, model);
        }

        public UserGroup GetUserGroup(int UserGroupID)
        {
            string sql = "EXEC ausp_Admin_GetUserGroup_S";
            var args = new
            {
                UserGroupID
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<UserGroup>(sql, args);
        }

        public BaseResult UpdateUserGroup(int UserGroupID, UserGroup model)
        {
            string sql = "EXEC ausp_Admin_UpdateUserGroup_U";
            var args = new
            {
                UserGroupID,
                model.UserGroupCode,
                model.UserGroupName,
                model.Remark,
                model.Visible
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        public BaseResult DeleteUserGroup(int UserGroupID)
        {
            string sql = "EXEC ausp_Admin_DeleteUserGroup_D";
            var args = new
            {
                UserGroupID
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
    }
}
