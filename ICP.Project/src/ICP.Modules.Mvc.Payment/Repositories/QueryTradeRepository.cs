using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Core.Frameworks.DbUtil;
using ICP.Modules.Mvc.Payment.Models.QueryTrade;
using System.Collections.Generic;

namespace ICP.Modules.Mvc.Payment.Repositories
{
    public class QueryTradeRepository
    {
        protected IDbConnectionPool _dbConnectionPool = null;

        public QueryTradeRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        /// <summary>
        /// 取得ATM儲值訂單
        /// </summary>
        /// <param name="dbReq"></param>
        /// <returns></returns>
        [EnableDbProxy]
        public TopUpATMTradeDbRes GetATMTopUpTradeDetail(TradeDbReq dbReq)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);
            string sql = "EXEC [ICP_Payment].[dbo].[ausp_Payment_Trade_GetATMTopUpTradeDetail_S]";

            var args = new
            {
                dbReq.MID,
                dbReq.TradeID
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<TopUpATMTradeDbRes>(sql, args);
        }

        /// <summary>
        /// 取得ACLink儲值訂單
        /// </summary>
        /// <param name="dbReq"></param>
        /// <returns></returns>
        [EnableDbProxy]
        public TopUpACLinkTradeDbRes GetACLinkTopUpTradeDetail(TradeDbReq dbReq)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);
            string sql = "EXEC [ICP_Payment].[dbo].[ausp_Payment_Trade_GetACLinkTopUpTradeDetail_S]";

            var args = new
            {
                dbReq.MID,
                dbReq.TradeID
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<TopUpACLinkTradeDbRes>(sql, args);
        }

        /// <summary>
        /// 取得現金儲值訂單
        /// </summary>
        /// <param name="dbReq"></param>
        /// <returns></returns>
        [EnableDbProxy]
        public TopUpCashTradeDbRes GetCashTopUpTradeDetail(TradeDbReq dbReq)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);
            string sql = "EXEC [ICP_Payment].[dbo].[ausp_Payment_Trade_GetCashTopUpTradeDetail_S]";

            var args = new
            {
                dbReq.MID,
                dbReq.TradeID
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<TopUpCashTradeDbRes>(sql, args);
        }

        /// <summary>
        /// 取得付款訂單
        /// </summary>
        /// <param name="dbReq"></param>
        /// <returns></returns>
        public List<PaidTradeInfo> GetPaidTradeDetail(TradeDbReq dbReq)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);
            string sql = "EXEC [ICP_Payment].[dbo].[ausp_Payment_Trade_GetPaidTradeDetail_S]";

            var args = new
            {
                dbReq.MID,
                dbReq.TradeID
            };

            sql += db.GenerateParameter(args);
            return db.Query<PaidTradeInfo>(sql, args);
        }

        /// <summary>
        /// 取得轉帳訂單
        /// </summary>
        /// <param name="dbReq"></param>
        /// <returns></returns>
        public TransferTradeInfo GetTransferTradeDetail(TradeDbReq dbReq)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);
            string sql = "EXEC [ICP_Payment].[dbo].[ausp_Payment_Trade_GetTransferTradeDetail_S]";

            var args = new
            {
                dbReq.MID,
                dbReq.TradeID,
                dbReq.ReceiveTransfer
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<TransferTradeInfo>(sql, args);
        }

        /// <summary>
        /// 取得提領訂單
        /// </summary>
        /// <param name="dbReq"></param>
        /// <returns></returns>
        public BankTransferTradeInfo GetBankTransferTradeDetail(TradeDbReq dbReq)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);
            string sql = "EXEC [ICP_Payment].[dbo].[ausp_Payment_Trade_GetBankTransferTradeDetail_S]";

            var args = new
            {
                dbReq.MID,
                dbReq.TradeID
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BankTransferTradeInfo>(sql, args);
        }
    }
}