using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Modules.Mvc.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Repositories
{
    public class RefundReportRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;
        private readonly IDbConnection db;

        public RefundReportRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
            db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);
        }

        /// <summary>
        /// 取得退款明細資料
        /// </summary>
        /// <param name="refundReportQueryCondition"></param>
        /// <returns></returns>
        public List<RefundReportQueryResult> ListRefundDetail(RefundReportQueryCondition refundReportQueryCondition)
        {
            string sql = "EXEC ausp_Payment_Report_ListRefundDetails_S";
            var args = new
            {
                refundReportQueryCondition.DateType,
                refundReportQueryCondition.StartDate,
                refundReportQueryCondition.EndDate,
                refundReportQueryCondition.PaymentType,
                refundReportQueryCondition.PaymentSideDataType,
                refundReportQueryCondition.PaymentSideDataContent,
                refundReportQueryCondition.AllocateStatus,
                refundReportQueryCondition.ReceiptSideDataType,
                refundReportQueryCondition.ReceiptSideDataContent,
                refundReportQueryCondition.TradeNo,
                refundReportQueryCondition.PageNo,
                refundReportQueryCondition.PageSize
            };
            sql += db.GenerateParameter(args);
            return db.Query<RefundReportQueryResult>(sql, args);
        }
    }
}
