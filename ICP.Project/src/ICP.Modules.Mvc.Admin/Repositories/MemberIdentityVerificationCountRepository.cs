using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Modules.Mvc.Admin.Models.IPRecord;
using ICP.Modules.Mvc.Admin.Models.MemberIdentityVerificationCount;

namespace ICP.Modules.Mvc.Admin.Repositories
{
    public class MemberIdentityVerificationCountRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        private IDbConnection db = null;

        public MemberIdentityVerificationCountRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }
        /// <summary>
        /// 查詢聯徵費用
        /// </summary>
        /// <param name="StartDate">起始日</param>
        /// <param name="EndDate">迄日</param>
        /// <returns></returns>
        public List<QueryJCICChargeFeeSettingResult> ListJCICChargeFeeSetting(DateTime StartDate, DateTime EndDate)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Admin);
            string sql = "EXEC ausp_Admin_ListJCICChargeFeeSetting_S";
            var args = new
            {
                StartDate,
                EndDate
            };
            sql += db.GenerateParameter(args);
            return db.Query<QueryJCICChargeFeeSettingResult>(sql, args);
        }
        /// <summary>
        /// 查詢P11/P33 Logs
        /// </summary>
        /// <param name="model">查詢物件</param>
        /// <returns></returns>
        public List<QueryMemberIdentityVerificationCountResult> ListMemberP11P33Log(QueryMemberIdentityVerificationCount model)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);
            string sql = "EXEC ausp_Admin_ListP11P33AuthLogByQueryJCICChargeFee_S";
            sql += db.GenerateParameter(model);
            return db.Query<QueryMemberIdentityVerificationCountResult>(sql, model);
        }
    }
}
