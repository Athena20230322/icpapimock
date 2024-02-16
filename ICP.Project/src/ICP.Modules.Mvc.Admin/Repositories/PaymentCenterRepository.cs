using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Modules.Mvc.Admin.Models.PaymentCenter;
using System.Collections.Generic;

namespace ICP.Modules.Mvc.Admin.Repositories
{
    /// <summary>
    /// 金流中心
    /// </summary>
    public class PaymentCenterRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;
        private readonly IDbConnection db;

        public PaymentCenterRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
            db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);
        }

        #region 金流中心統計資訊
        /// <summary>
        /// 金流中心統計資訊查詢
        /// </summary>
        public List<TradeStatisticsModel> ListTradeStatistics(TradeStatisticsQueryModel model)
        {
            string sql = "EXEC ausp_Admin_Payment_ListTradeStatistics_S";
            var args = new
            {
                model.StartDate,
                model.EndDate,
                model.PageNo,
                model.PageSize
            };
            sql += db.GenerateParameter(args);
            return db.Query<TradeStatisticsModel>(sql, args);
        }

        /// <summary>
        /// 金流中心統計資訊明細查詢
        /// </summary>
        public List<TradeStatisticsDetailModel> ListTradeStatisticsDetail(TradeStatisticsDetailQueryModel model)
        {
            string sql = "EXEC ausp_Admin_Payment_ListTradeStatisticsDetail_S";
            var args = new
            {
                model.BankCode,
                model.PaymentDate,
                model.QueryType
            };
            sql += db.GenerateParameter(args);
            return db.Query<TradeStatisticsDetailModel>(sql, args);
        }
        #endregion

        #region 每日營收統計
        /// <summary>
        /// 每日營收統計資訊查詢
        /// </summary>
        public List<DailyRevenueStatisticsModel> ListDailyRevenueStatistics(DailyRevenueStatisticsQueryModel model)
        {
            string sql = "EXEC ausp_Admin_Payment_ListDailyRevenueStatistics_S";
            var args = new
            {
                model.StartDate,
                model.EndDate,
                model.PageNo,
                model.PageSize
            };
            sql += db.GenerateParameter(args);
            return db.Query<DailyRevenueStatisticsModel>(sql, args);
        }
        #endregion

        #region 金流手續費統計(月結)
        /// <summary>
        /// 金流手續費統計資訊查詢
        /// </summary>
        public List<FeeStatisticsModel> ListFeeStatistics(FeeStatisticsQueryModel model)
        {
            string sql = "EXEC ausp_Admin_Payment_ListFeeStatistics_S";
            var args = new
            {
                model.MerchantQueryType,
                model.MerchantQueryValue,
                model.StartDate,
                model.EndDate,
                model.TradeModeID,
                model.PageNo,
                model.PageSize
            };
            sql += db.GenerateParameter(args);
            return db.Query<FeeStatisticsModel>(sql, args);
        }

        /// <summary>
        /// 金流手續費統計資訊明細查詢
        /// </summary>
        public List<FeeStatisticsDetailModel> ListFeeStatisticsDetail(FeeStatisticsDetailQueryModel model)
        {
            string sql = "EXEC ausp_Admin_Payment_ListFeeStatisticsDetail_S";
            var args = new
            {
                model.StartDate,
                model.EndDate,
                model.MerchantID,
                model.TradeModeID,
                model.PageNo,
                model.PageSize
            };
            sql += db.GenerateParameter(args);
            return db.Query<FeeStatisticsDetailModel>(sql, args);
        }
        #endregion
    }
}
