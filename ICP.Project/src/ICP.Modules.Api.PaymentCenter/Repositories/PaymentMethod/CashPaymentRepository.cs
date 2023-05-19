using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Api.PaymentCenter.Models;
using ICP.Modules.Api.PaymentCenter.Models.PaymentMethod.iCash;

namespace ICP.Modules.Api.PaymentCenter.Repositories.PaymentMethod
{
    public class CashPaymentRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;
        private IDbConnection db;

        public CashPaymentRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        public BaseResult ReduceTopupCash(ICashIncDecModel model)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Coins);

            string sql = "EXEC ausp_Coins_TopUp_ReduceTopUpCoins_SIU";
            var args = new
            {
                TradeNo = model.TradeNo,
                MID = model.MID,
                MerchantID = model.MerchantID,
                TradeModeID = model.TradeModeID,
                PaymentTypeID = model.PaymentTypeID,
                PaymentSubTypeID = model.PaymentSubTypeID,
                ReduceCash = model.Amount,
                Notes = model.Notes
            };
            sql += db.GenerateParameter(args);
            BaseResult result = db.QuerySingleOrDefault<BaseResult>(sql, args);
            return result;
        }

        public BaseResult AddTopupCash(ICashIncDecModel model)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Coins);

            string sql = "EXEC ausp_Coins_TopUp_AddTopUpCoins_SI";
            var args = new
            {
                TradeNo = model.TradeNo,
                MID = model.MID,
                MerchantID = model.MerchantID,
                TradeModeID = model.TradeModeID,
                PaymentTypeID = model.PaymentTypeID,
                PaymentSubTypeID = model.PaymentSubTypeID,
                TradeTopUpCash = model.Amount,
                Notes = model.Notes
            };
            sql += db.GenerateParameter(args);
            BaseResult result = db.QuerySingleOrDefault<BaseResult>(sql, args);
            return result;
        }

        public QryRefundTradeModel QryRefundTrade(RefundReqModel requestModel)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_PaymentCenter);

            string sql = "EXEC ausp_PaymentCenter_Refund_QryRefund_S";
            var args = new
            {
                TradeID = requestModel.PaymentCenterTradeID,
                MerchantID = requestModel.MerchantID,
                PlatformID = requestModel.PlatformID,
                TradeNo = requestModel.TradeNo,
                MerchantTradeNo = requestModel.MerchantTradeNo,
                RefundAmount = requestModel.Amount
            };
            sql += db.GenerateParameter(args);
            QryRefundTradeModel result = db.QuerySingleOrDefault<QryRefundTradeModel>(sql, args);
            return result;
        }

        public AddRefundTradeModel AddRefundTrade(RefundReqModel requestModel)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_PaymentCenter);

            string sql = "EXEC ausp_PaymentCenter_Refund_AddRefundTrade_S";
            var args = new
            {
                TradeID = requestModel.PaymentCenterTradeID,
                RefundAmount = requestModel.Amount
            };
            sql += db.GenerateParameter(args);
            AddRefundTradeModel result = db.QuerySingleOrDefault<AddRefundTradeModel>(sql, args);
            return result;
        }

        public RefundResModel UpdateRefundTrade(long tradeID, DataResult<RefundResModel> requestModel)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_PaymentCenter);

            string sql = "EXEC ausp_PaymentCenter_Refund_UpdateRefundTrade_S";
            var args = new
            {
                SeqNo = requestModel.RtnData.PaymentCenterTradeID,
                TradeID = tradeID,
                RtnCode = requestModel.RtnCode,
                RtnMsg = requestModel.RtnMsg,
                RefundAmount = requestModel.RtnData.RefundAmount
            };
            sql += db.GenerateParameter(args);
            RefundResModel result = db.QuerySingleOrDefault<RefundResModel>(sql, args);
            return result;
        }

        public QryReversalTradeModel QryReversalTrade(ReversalReqModel requestModel)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_PaymentCenter);

            string sql = "EXEC ausp_PaymentCenter_Reversal_QryReversal_S";
            var args = new
            {
                TradeID = requestModel.PaymentCenterTradeID,
                MerchantID = requestModel.MerchantID,
                PlatformID = requestModel.PlatformID,
                TradeNo = requestModel.TradeNo,
                MerchantTradeNo = requestModel.MerchantTradeNo
            };
            sql += db.GenerateParameter(args);
            QryReversalTradeModel result = db.QuerySingleOrDefault<QryReversalTradeModel>(sql, args);
            return result;
        }

        public AddReversalTradeModel AddReversalTrade(ReversalReqModel requestModel)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_PaymentCenter);

            string sql = "EXEC ausp_PaymentCenter_Reversal_AddReversalTrade_S";
            var args = new
            {
                TradeID = requestModel.PaymentCenterTradeID
            };
            sql += db.GenerateParameter(args);
            AddReversalTradeModel result = db.QuerySingleOrDefault<AddReversalTradeModel>(sql, args);
            return result;
        }

        public ReversalResModel UpdateReversalTrade(long tradeID, DataResult<ReversalResModel> requestModel)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_PaymentCenter);

            string sql = "EXEC ausp_PaymentCenter_Reversal_UpdateReversalTrade_S";
            var args = new
            {
                SeqNo = requestModel.RtnData.PaymentCenterTradeID,
                TradeID = tradeID,
                RtnCode = requestModel.RtnCode,
                RtnMsg = requestModel.RtnMsg
            };
            sql += db.GenerateParameter(args);
            ReversalResModel result = db.QuerySingleOrDefault<ReversalResModel>(sql, args);
            return result;
        }
    }
}
