using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Core.Frameworks.DbUtil;
using ICP.Modules.Api.Payment.Models.ChargeBack;

namespace ICP.Modules.Api.Payment.Repositories
{
    public class RefundRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        public RefundRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        /// <summary>
        /// 建立退款訂單
        /// </summary>
        /// <param name="paymentParameter"></param>
        /// <returns></returns>
        [EnableDbProxy]
        public virtual AddChargeBackDbRes AddChargeBack(ChargeBackReq chargeBackRequest)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);

            string sql = "EXEC [ICP_Payment].[dbo].[ausp_Payment_Trade_AddRefundTrade_I]";

            var args = new
            {
                PlatformID = chargeBackRequest.PlatformID,
                MerchantID = chargeBackRequest.MerchantID,
                TradeNo = chargeBackRequest.TransactionID,
                Amount = chargeBackRequest.Amount,
                StoreID = chargeBackRequest.StoreID,
                StoreName = chargeBackRequest.StoreName,
                PosRefNo = chargeBackRequest.PosRefNo,
                MerchantTID = chargeBackRequest.MerchantTID,
                MerchantTradeNo = chargeBackRequest.MerchantTradeNo,
                MerchantTradeDate = chargeBackRequest.MerchantTradeDate,
                ForCancel = chargeBackRequest.ForCancel,
                DebitPoint = chargeBackRequest.DebitPoint,
                BonusAmt = chargeBackRequest.BonusAmt
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<AddChargeBackDbRes>(sql, args);
        }

        /// <summary>
        /// 更新退款交易訂單
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        [EnableDbProxy]
        public virtual UpdateChargeBackDbRes UpdateChargeBack(ChargeBackSendToPaymentCenterRes sendToPaymentCenterResponse)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);

            string sql = "EXEC [ICP_Payment].[dbo].[ausp_Payment_Trade_UpdateRefundTrade_U]";

            var args = new
            {
                ChargeBackID = sendToPaymentCenterResponse.RtnData.ChargeBackID,
                RefundAmt = sendToPaymentCenterResponse.RtnData.RefundAmount,
                RefundDate = 
                    !"0001/01/01 00:00:00".Equals(sendToPaymentCenterResponse.RtnData.TradeDate.ToString("yyyy/MM/dd HH:mm:ss")) ? 
                        sendToPaymentCenterResponse.RtnData.TradeDate.ToString("yyyy/MM/dd HH:mm:ss") : 
                        sendToPaymentCenterResponse.RtnData.RefundDate.ToString("yyyy/MM/dd HH:mm:ss"),
                ForCancel = sendToPaymentCenterResponse.RtnData.ForCancel,
                PaymentCenterTradeID = 
                    sendToPaymentCenterResponse.RtnData.PaymentCenterTradeID > 0 ? 
                        sendToPaymentCenterResponse.RtnData.PaymentCenterTradeID : 
                        sendToPaymentCenterResponse.RtnData.RefundTradeID
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<UpdateChargeBackDbRes>(sql, args);
        }
    }
}
