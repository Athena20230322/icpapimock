using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Modules.Mvc.Admin.Models;
using ICP.Modules.Mvc.Admin.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Repositories
{
    public class UnregisteredDataRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        private IDbConnection db;

        public UnregisteredDataRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        /// <summary>
        /// 取得被刪除的會員資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<UnregisteredData> ListUnregisteredData(QueryUnregisteredDataVM model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);

            string sql = "EXEC ausp_Admin_ListMemberUnregisteredDataLog_S";

            var args = new
            {
                model.StartDate,
                model.EndDate,
                model.CName,
                model.CellPhone,
                model.Source,
                model.PageNo,
                model.PageSize
            };

            sql += db.GenerateParameter(args);

            return db.Query<UnregisteredData>(sql, args).ToList();
        }

        /// <summary>
        /// 取得單筆被刪除的會員資料
        /// </summary>
        /// <param name="MID"></param>
        /// <returns></returns>
        public UnregisteredData GetUnregisteredData(long MID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);

            string sql = "EXEC ausp_Admin_GetMemberUnregisteredDataLog_S";

            var args = new
            {
                MID
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<UnregisteredData>(sql, args);
        }
    }
}
