using ICP.Infrastructure.Core.Models;
using ICP.Modules.Api.PaymentCenter.Models.TransactionMethod;
using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Modules.Api.PaymentCenter.Models;

namespace ICP.Modules.Api.PaymentCenter.Repositories
{
    public class TransactionRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        private IDbConnection db;

        public TransactionRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;

            db = _dbConnectionPool.Create(DatabaseName.ICP_PaymentCenter);
        }

        /// <summary>
        /// 新增訂單
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        public AddTradeResult AddTrade(TradeReqModel tradeRequestModel)
        {
            string sql = "EXEC ausp_PaymentCenter_Trade_AddTrade_I";
            var args = new
            {
                tradeRequestModel.MID,
                tradeRequestModel.PlatformID,
                tradeRequestModel.MerchantID,
                tradeRequestModel.TradeModeID,
                tradeRequestModel.PaymentTypeID,
                tradeRequestModel.PaymentSubTypeID,
                tradeRequestModel.TradeNo,
                tradeRequestModel.MerchantTradeNo,
                tradeRequestModel.Amount
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<AddTradeResult>(sql, args);
        }

        /// <summary>
        /// 更新訂單
        /// </summary>
        /// <param name="tradeResponseModel"></param>
        /// <returns></returns>
        public BaseResult UpdateTrade(DataResult<TradeResModel> tradeResponseModel)
        {
            string sql = "EXEC ausp_PaymentCenter_Trade_UpdateTrade_U";
            var args = new
            {
                TradeID = tradeResponseModel.RtnData.PaymentCenterTradeID,
                RtnCode = tradeResponseModel.RtnCode,
                RtnMsg = tradeResponseModel.RtnMsg
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
    }
}
