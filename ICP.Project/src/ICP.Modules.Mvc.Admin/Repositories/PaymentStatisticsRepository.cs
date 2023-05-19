using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Admin.Models.PaymentStatistics;
using ICP.Modules.Mvc.Admin.Models.PaymentStatistics.IncomeMonitor;
using ICP.Modules.Mvc.Admin.Models.PaymentStatistics.PaymentMonitor;
using ICP.Modules.Mvc.Admin.Models.PaymentStatistics.TimingMonitor;
using System.Collections.Generic;

namespace ICP.Modules.Mvc.Admin.Repositories
{
    public class PaymentStatisticsRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        private IDbConnection db;

        public PaymentStatisticsRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        /// <summary>
        /// 每日提領金額監控清單
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<ListWithdrawDbRes> ListWithdraw(ListWithdrawDbReq req)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Statistics);
            string sql = "EXEC ausp_Statistics_Admin_ListDailyWithdrawMonitor_S";

            sql += db.GenerateParameter(req);

            return db.Query<ListWithdrawDbRes>(sql, req);
        }

        /// <summary>
        /// 提領排程清單
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<BankTransferScheduleDbRes> ListBankTransferSchedule(object req)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);
            string sql = "EXEC ausp_Admin_Payment_ListBankTransferSchedule_S";

            sql += db.GenerateParameter(req);

            return db.Query<BankTransferScheduleDbRes>(sql, req);
        }

        /// <summary>
        /// 每日收款交易金額監控 - 查詢監控記錄
        /// </summary>
        /// <param name="request">查詢條件</param>
        /// <returns></returns>
        public List<QryIncomeMonitorDbRes> ListDailyIncomeMonitor(QryIncomeMonitorDbReq request)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Statistics);
            string sql = "EXEC ausp_Statistics_Admin_ListDailyIncomeMonitor_S";

            sql += db.GenerateParameter(request);

            return db.Query<QryIncomeMonitorDbRes>(sql, request);
        }

        /// <summary>
        /// 每日付款交易金額監控 - 查詢監控記錄
        /// </summary>
        /// <param name="request">查詢條件</param>
        /// <returns></returns>
        public List<QryPaymentMonitorDbRes> ListDailyPaymentMonitor(QryPaymentMonitorDbReq request)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Statistics);
            string sql = "EXEC ausp_Statistics_Admin_ListDailyPaymentMonitor_S";

            sql += db.GenerateParameter(request);

            return db.Query<QryPaymentMonitorDbRes>(sql, request);
        }


        /// <summary>
        /// 歷程清單
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<ListMonitorLogDbRes> GetMonitorLogList(object req)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Statistics);
            string sql = "EXEC ausp_Statistics_Admin_ListMerchantMonitorLog_S";

            sql += db.GenerateParameter(req);

            return db.Query<ListMonitorLogDbRes>(sql, req);
        }

        /// <summary>
        /// 新增歷程
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public BaseResult AddMonitorLog(AddMonitorLogDbReq req)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Statistics);
            string sql = "EXEC ausp_Statistics_Admin_AddMerchantMonitorLog_I";

            sql += db.GenerateParameter(req);

            return db.QuerySingleOrDefault<BaseResult>(sql, req);
        }

        /// <summary>
        /// 定時監控
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<TimingMonitorRes> ListTimingMonitor(TimingMonitorDbReq req)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);
            string sql = "EXEC ICP_Payment.dbo.ausp_Payment_Statistics_ListMerchantRiskMonitor_S";

            var args = new
            {
                StartDate = req.StartDate.ToString("yyyy/MM/dd"),
                ICPMID = req.ICPMID,
                MerchantName = req.MerchantName,
                MonitorStatus = req.MonitorStatus ? 1 : 0,
                PageNo = req.PageNo,
                PageSize = req.PageSize,
                RuleMode = req.RuleMode,
                Day1Waring = req.Day1Waring,
                Day10Waring = req.Day10Waring,
                Day30Waring = req.Day30Waring,
                SortType = req.SortType,
                SortSet = req.SortSet,
                Day1Amount = req.Day1Amount,
                Day10Amount = req.Day10Amount,
                Day30Amount = req.Day30Amount,
                WithdrawStatus = req.WithdrawStatus ? 1 : 0,
                TradeType = req.TradeType,
                TradeMode = req.TradeMode,
                TradeContent = req.TradeContent,
                SelectMode = req.SelectMode ? 1 : 0
            };

            sql += db.GenerateParameter(args);

            var result = db.Query<TimingMonitorRes>(sql, args);

            return result;
        }
    }
}
