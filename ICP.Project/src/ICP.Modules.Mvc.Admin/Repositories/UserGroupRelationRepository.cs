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

    public class UserGroupRelationRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        private IDbConnection db;

        public UserGroupRelationRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;

            db = _dbConnectionPool.Create(DatabaseName.ICP_Admin);
        }

        public BaseResult AddUserGroupRelation(int UserGroupID, int UserID)
        {
            string sql = "EXEC ausp_Admin_AddUserGroupRelation_I";
            var args = new
            {
                UserGroupID,
                UserID
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        public BaseResult DeleteUserGroupRelation(int UserGroupID, int UserID)
        {
            string sql = "EXEC ausp_Admin_DeleteUserGroupRelation_D";
            var args = new
            {
                UserGroupID,
                UserID
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
    }
}
