
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Repositories
{
    using Infrastructure.Abstractions.DbUtil;
    using Models.Logging;

    public class LoggingRepository
    {
        #region 建構、連線
        private readonly IDbConnectionPool _dbConnectionPool = null;

        private IDbConnection db;

        public LoggingRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;

            db = dbConnectionPool.Create(DatabaseName.ICP_Logging);
        }
        #endregion

        /// <summary>
        /// 訊息公告清單
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CustomerMutipleLogModel ListCustomerMutipleLog(long CustomerID)
        {
            string sql = "EXEC ausp_Admin_Merchant_ListCustomerMutipleLog_S";

            var args = new { CustomerID };

            sql += db.GenerateParameter(args);

            var types = new Type[]
            {
                typeof(List<CustomerAllocateDayLog>),
                typeof(List<CustomerAnnualFeeLog>),
                typeof(List<CustomerMinimunRetentionLog>),
                typeof(List<CustomerTransferUsedLimitLog>),
                typeof(List<CustomerSalesLog>),
                typeof(List<CustomerAuditLog>),
                typeof(List<CustomerMemoLog>),
                typeof(List<CustomerArchivingNoLog>)
            };

            var results = db.QueryMultiple(types, sql, args);

            var result = new CustomerMutipleLogModel();
            result.CustomerAllocateDayLogs = results[0].Cast<CustomerAllocateDayLog>().ToList();
            result.CustomerAnnualFeeLogs = results[1].Cast<CustomerAnnualFeeLog>().ToList();
            result.CustomerMinimunRetentionLogs = results[2].Cast<CustomerMinimunRetentionLog>().ToList();
            result.CustomerTransferUsedLimitLogs = results[3].Cast<CustomerTransferUsedLimitLog>().ToList();
            result.CustomerSalesLogs = results[4].Cast<CustomerSalesLog>().ToList();
            result.CustomerAuditLogs = results[5].Cast<CustomerAuditLog>().ToList();
            result.CustomerMemoLogs = results[6].Cast<CustomerMemoLog>().ToList();
            result.CustomerArchivingNoLogs = results[7].Cast<CustomerArchivingNoLog>().ToList();

            return result;
        }
    }
}
