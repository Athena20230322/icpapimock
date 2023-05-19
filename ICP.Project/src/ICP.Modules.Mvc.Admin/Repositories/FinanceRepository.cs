using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Modules.Mvc.Admin.Models.Finance;
using ICP.Modules.Mvc.Admin.Models.Finance.MerchantTradeDetail;
using ICP.Modules.Mvc.Admin.Models.Finance.TradeDetail;
using System.Collections.Generic;

namespace ICP.Modules.Mvc.Admin.Repositories
{
    public class FinanceRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        private IDbConnection db;

        public FinanceRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        /// <summary>
        /// 查詢帳務類型
        /// </summary>
        /// <param name="tradeModeType">撈取類型</param>
        /// <remarks>0:全部 1:凍結款項除外</remarks>
        /// <returns></returns>
        public List<TradeModeModel> ListTradeMode(int tradeModeType)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);
            string sql = "EXEC ausp_Payment_Trade_ListTradeMode_S";

            var req = new
            {
                TradeModeType = tradeModeType
            };

            sql += db.GenerateParameter(req);

            return db.Query<TradeModeModel>(sql, req);
        }

        /// <summary>
        /// 查詢交易類型
        /// </summary>
        /// <param name="tradeModeID">帳務類型</param>
        /// <returns></returns>
        public List<TradeTypeModel> ListTradeType(int tradeModeID)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);
            string sql = "EXEC ausp_Payment_Trade_ListTradeType_S";

            var req = new
            {
                TradeModeID = tradeModeID
            };


            sql += db.GenerateParameter(req);

            return db.Query<TradeTypeModel>(sql, req);
        }

        /// <summary>
        /// 查詢交易子類型
        /// </summary>
        /// <param name="paymentTypeID">交易類型</param>
        /// <returns></returns>
        public List<TradeSubTypeModel> ListTradeSubType(int paymentTypeID)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);
            string sql = "EXEC ausp_Payment_Trade_ListTradeSubType_S";

            var req = new
            {
                PaymentTypeID = paymentTypeID
            };


            sql += db.GenerateParameter(req);

            return db.Query<TradeSubTypeModel>(sql, req);
        }


        /// <summary>
        /// 實質交易明細查詢 - 查詢交易明細
        /// </summary>
        /// <param name="req">查詢條件</param>
        /// <returns></returns>
        public List<QryTradeDetailDbRes> ListTradeDetail(QryTradeDetailDbReq req)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);
            string sql = "EXEC ausp_Admin_Payment_ListTradeDetail_S";

            sql += db.GenerateParameter(req);

            return db.Query<QryTradeDetailDbRes>(sql, req);
        }

        /// <summary>
        /// 特店帳務進出明細 - 查詢明細
        /// </summary>
        /// <param name="req">查詢條件</param>
        /// <returns></returns>
        public List<QryMerchantTradeDetailDbRes> ListMerchantTradeDetail(QryMerchantTradeDetailDbReq req)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);
            string sql = "EXEC ausp_Admin_Payment_ListMerchantTradeDetail_S";

            sql += db.GenerateParameter(req);

            return db.Query<QryMerchantTradeDetailDbRes>(sql, req);
        }
    }
}
