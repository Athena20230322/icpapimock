using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Modules.Mvc.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Repositories
{
    public class TopUpReportRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;
        private readonly IDbConnection db;

        public TopUpReportRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
            db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);
        }

        /// <summary>
        /// 取得儲值明細資料清單
        /// </summary>
        /// <param name="topUpReportQueryCondition"></param>
        /// <returns></returns>
        public List<TopUpReportQueryResult> ListTopUpDetails(TopUpReportQueryCondition topUpReportQueryCondition)
        {
            string sql = "EXEC ausp_Payment_Report_ListTopUpDetails_S";
            var args = new
            {
                topUpReportQueryCondition.DateType,
                topUpReportQueryCondition.StartDate,
                topUpReportQueryCondition.EndDate,
                topUpReportQueryCondition.TopUpType,
                topUpReportQueryCondition.MemberDataType,
                topUpReportQueryCondition.MemberDataContent,
                topUpReportQueryCondition.TopUpStatus,
                topUpReportQueryCondition.QueryDataType,
                topUpReportQueryCondition.QueryDataContent,
                topUpReportQueryCondition.PageNo,
                topUpReportQueryCondition.PageSize
            };
            sql += db.GenerateParameter(args);
            return db.Query<TopUpReportQueryResult>(sql, args);
        }
    }
}
