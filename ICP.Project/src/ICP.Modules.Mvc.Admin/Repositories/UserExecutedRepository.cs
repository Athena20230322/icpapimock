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

    public class UserExecutedRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        private IDbConnection db;

        public UserExecutedRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;

            db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);
        }

        /// <summary>
        /// 新增使用者操作記錄
        /// </summary>
        /// <param name="UserGroupID"></param>
        /// <param name="UserID"></param>
        /// <param name="JSON"></param>
        /// <returns></returns>
        public BaseResult AddActionLog(UserExecuted AddModel)
        {
            string sql = "EXEC ausp_Admin_AddUserExecuted_I";
            var args = new
            {
                AddModel.UserID,
                AddModel.Account,
                AddModel.ControllerName,
                AddModel.ActionName,
                AddModel.Path,
                AddModel.Headers,
                AddModel.UrlQuery,
                AddModel.FormData,
                AddModel.RealIP,
                AddModel.ProxyIP
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

      
    }
}
