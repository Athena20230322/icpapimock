using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Frameworks.DbUtil;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Api.Payment.Models.Cashier;
using ICP.Modules.Api.Payment.Models.ChargeOnline;
using ICP.Modules.Api.Payment.Models.GetMemberPaymentInfo;
using System;
using System.Collections.Generic;

namespace ICP.Modules.Api.Payment.Repositories
{
    public class PaymentTradeRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        public PaymentTradeRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        /// <summary>
        /// 建立訂單
        /// </summary>
        /// <param name="paymentParameter"></param>
        /// <returns></returns>
        [EnableDbProxy]
        public virtual AddTradeDbRes AddTrade(AddTradeDBReq cashierRequest)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);

            string sql = "EXEC [ICP_Payment].[dbo].[ausp_Payment_Trade_AddTrade_I]";

            var args = new
            {
                PlatformID = cashierRequest.PlatformID,
                MerchantID = cashierRequest.MerchantID,
                MID = cashierRequest.MID,
                TradeType = cashierRequest.TradeType,
                TradeModeID = cashierRequest.TradeModeID,
                PaymentTypeID = cashierRequest.PaymentTypeID,
                PaymentSubTypeID = cashierRequest.PaymentSubTypeID,
                WalletID = cashierRequest.WalletID,
                Barcode = cashierRequest.Barcode,
                Amount = cashierRequest.Amount,
                StoreID = cashierRequest.StoreID,
                StoreName = cashierRequest.StoreName,
                posRefNo = cashierRequest.PosRefNo,
                MerchantTID = cashierRequest.MerchantTID,
                MerchantTradeNo = cashierRequest.MerchantTradeNo,
                MerchantTradeDate = cashierRequest.MerchantTradeDate,
                CarrierType = cashierRequest.CarrierType,
                ItemAmt = cashierRequest.ItemAmt,
                UtilityAmt = cashierRequest.UtilityAmt,
                CommAmt = cashierRequest.CommAmt,
                ExceptAmt1 = cashierRequest.ExceptAmt1,
                ExceptAmt2 = cashierRequest.ExceptAmt2,
                RedeemFlag = cashierRequest.RedeemFlag,
                BonusAmt = cashierRequest.BonusAmt,
                DebitPoint = cashierRequest.DebitPoint,
                NonRedeemAmt = cashierRequest.NonRedeemAmt,
                NonPointAmt = cashierRequest.NonPointAmt,
                Ccy = cashierRequest.Ccy,
                ItemList = cashierRequest.ItemList,
                PayID = cashierRequest.PayID,
                Automation = cashierRequest.Automation,
                OWSubmitDate = cashierRequest.OWSubmitDate,
                Remark = cashierRequest.Remark,
                AccountID = cashierRequest.AccountID
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<AddTradeDbRes>(sql, args);

        }

        /// <summary>
        /// 更新交易訂單
        /// </summary>
        /// <param name="paymentCenterResponse"></param>
        /// <returns></returns>
        [EnableDbProxy]
        public virtual UpdateTradeDBRes UpdateTrade(UpdateTradeDBReq paymentCenterResponse)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);

            string sql = "EXEC [ICP_Payment].[dbo].[ausp_Payment_Trade_UpdateTrade_U]";

            var args = new
            {
                TradeID = paymentCenterResponse.TradeID,
                RtnCode = paymentCenterResponse.RtnCode,
                PaymentCenterTradeID = paymentCenterResponse.RtnData != null? paymentCenterResponse.RtnData.PaymentCenterTradeID : 0
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<UpdateTradeDBRes>(sql, args);
        }

        /// <summary>
        /// 更新ATM訂單(更新PaymentCenter回傳資訊,並非代表消費者已付款)
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        [EnableDbProxy]
        public virtual BaseResult UpdatePaymentDetailATM(UpdateTradeDBReq result)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);

            string sql = "EXEC [ICP_Payment].[dbo].[ausp_Payment_Trade_UpdateATMDetail_U]";

            var args = new
            {
                TradeID = result.TradeID,
                RtnCode = result.RtnCode,
                RtnMsg = result.SetCode(result.RtnCode).RtnMsg,
                TradeDate = !string.IsNullOrWhiteSpace(result.RtnData.ATMTradeDate) && DateTime.TryParse(result.RtnData.ATMTradeDate, out DateTime tradeDate) ? tradeDate.ToString("yyyy/MM/dd HH:mm:ss") : Convert.DBNull,
                vAccount = result.RtnData.VirtualAccount,
                ExpireDate = !string.IsNullOrWhiteSpace(result.RtnData.ATMExpireDate) && DateTime.TryParse(result.RtnData.ATMExpireDate, out DateTime atmExpireDate) ? atmExpireDate.ToString("yyyy/MM/dd") : Convert.DBNull,
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 更新AccountLink訂單(更新PaymentCenter回傳資訊)
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        [EnableDbProxy]
        public virtual BaseResult UpdatePaymentDetailAccountLink(UpdateTradeDBReq result)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);

            string sql = "EXEC [ICP_Payment].[dbo].[ausp_Payment_Trade_UpdateAccountLinkDetail_U]";

            var args = new
            {
                TradeID = result.TradeID,
                BankTradeNo = result.RtnData.BankTradeNo,
                PayRtnCode = result.RtnCode,
                PayRtnMsg = result.SetCode(result.RtnCode).RtnMsg,
                VerifyRtnCode = result.RtnData.VerifyRtnCode,
                VerifyRtnMsg = result.RtnData.VerifyRtnMsg,
                VirtualAccount = result.RtnData.VirtualAccount,
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }        

        /// <summary>
        /// 建立Temp訂單
        /// </summary>
        /// <param name="addTempTrade"></param>
        /// <returns></returns>
        [EnableDbProxy]
        public virtual AddTempTradeDbRes AddTempTrade(AddTempTradeDbReq addTempTrade)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);

            string sql = "EXEC [ICP_Payment].[dbo].[ausp_Payment_Trade_AddTempTrade_I]";

            var args = new
            {
                PlatformID = addTempTrade.PlatformID,
                MerchantID = addTempTrade.MerchantID,
                WalletID = addTempTrade.WalletId,
                Amount =  addTempTrade.Amount,
                StoreID = addTempTrade.StoreId,
                StoreName = addTempTrade.StoreName,
                MerchantTradeNo = addTempTrade.MerchantTradeNo,
                MerchantTradeDate = addTempTrade.MerchantTradeDate,
                CarrierType = addTempTrade.CarrierType,
                ItemAmt = addTempTrade.ItemAmt,
                UtilityAmt = addTempTrade.UtilityAmt,
                CommAmt = addTempTrade.CommAmt,
                ExceptAmt1 = addTempTrade.ExceptAmt1,
                ExceptAmt2 = addTempTrade.ExceptAmt2,
                RedeemFlag = addTempTrade.RedeemFlag,
                BonusAmt = addTempTrade.BonusAmt,
                DebitPoint = addTempTrade.DebitPoint,
                NonRedeemAmt = addTempTrade.NonRedeemAmt,
                NonPointAmt = addTempTrade.NonPointAmt,
                ItemList = addTempTrade.ItemList,
                ts = addTempTrade.ts,
                PayID = addTempTrade.PayID,
                MID = addTempTrade.MID,
                Remark = addTempTrade.Remark
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<AddTempTradeDbRes>(sql, args);

        }

        /// <summary>
        /// 撈取訂單資訊 
        /// </summary>
        /// <param name="paymentParameter"></param>
        /// <returns></returns>
        public virtual PaymentTempTradeDbRes GetTempTradeNoInfo(EncDataModel encData)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);

            string sql = "EXEC [ICP_Payment].[dbo].[ausp_Payment_Trade_GetTempTrade_S]";

            var args = new
            {
                MerchantTradeNo = encData.MerchantTradeNo,
                ts = encData.ts
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<PaymentTempTradeDbRes>(sql, args);
        }

        /// <summary>
        /// 撈取訂單資訊(TradeItemsDetail)
        /// </summary>
        /// <param name="encData"></param>
        /// <returns></returns>
        public virtual List<ItemModel> GetTempTradeItemsDetailInfo(EncDataModel encData)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);

            string sql = "EXEC [ICP_Payment].[dbo].[ausp_Payment_Trade_GetTempTradeItemDetail_S]";

            var args = new
            {
                MerchantTradeNo = encData.MerchantTradeNo,
                ts = encData.ts
            };

            sql += db.GenerateParameter(args);

            return db.Query<ItemModel>(sql, args);
        }


        /// <summary>
        /// 撈取日期區間交易總金額
        /// </summary>
        /// <param name="startDate">開始日期</param>
        /// <param name="endDate">結束日期</param>
        /// <param name="mid">會員代碼</param>
        /// <param name="idType">角色(1:買家 2:賣家)</param>
        /// <param name="tradeModeID">交易模式(1:交易、2:儲值、3:轉帳、4:提領)</param>
        /// <param name="tradeType">交易類型(1:EC、2:Mobile)</param>
        /// <param name="paymentTypeID">交易付款方式</param>
        /// <param name="paymentSubTypeID">交易子付款方式</param>
        /// <returns></returns>
        public virtual Decimal GetTradeStatistics(string startDate, string endDate, long mid, int idType, int tradeModeID, int tradeType, int paymentTypeID, int paymentSubTypeID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);

            string sql = "EXEC [ICP_Payment].[dbo].[ausp_Payment_Trade_GetTradeStatistics_S]";

            var args = new
            {
                StartDate = startDate,
                EndDate = endDate,
                MID = mid,
                IDType = idType,
                TradeModeID = tradeModeID,
                TradeType = tradeType,
                PaymentTypeID = paymentTypeID,
                PaymentSubTypeID = paymentSubTypeID
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<Decimal>(sql, args);
        }
    }
}
