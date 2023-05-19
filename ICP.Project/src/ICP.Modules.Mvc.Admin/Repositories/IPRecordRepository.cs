using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Modules.Mvc.Admin.Models.IPRecord;

namespace ICP.Modules.Mvc.Admin.Repositories
{
    public class IPRecordRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        private IDbConnection db = null;

        public IPRecordRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }
        /// <summary>
        /// IP紀錄查詢
        /// </summary>
        /// <param name="model">查詢物件</param>
        /// <returns></returns>
        public List<QueryIPRecordResult> ListLoginRecord(QueryIPRecord model)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);
            string sql = "EXEC ausp_Member_ListLoginRecord_S";
            sql += db.GenerateParameter(model);
            return db.Query<QueryIPRecordResult>(sql, model);
        }
    }
}
