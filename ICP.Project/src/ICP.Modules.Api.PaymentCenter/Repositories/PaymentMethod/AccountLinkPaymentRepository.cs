using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Helpers;
using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Utils;
using ICP.Library.Models.AccountLinkApi.Enums;
using ICP.Modules.Api.PaymentCenter.Models;
using ICP.Modules.Api.PaymentCenter.Models.PaymentMethod.AccountLink;
using ICP.Modules.Api.PaymentCenter.Models.PaymentMethod.AccountLink.Auth;
using ICP.Modules.Api.PaymentCenter.Models.PaymentMethod.iCash;
using System;

namespace ICP.Modules.Api.PaymentCenter.Repositories.PaymentMethod
{
    public class AccountLinkPaymentRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;
        private IDbConnection db;
        private readonly ILogger _logger = null;

        private const int _TIMEOUTSEC = 120;

        private bool _ISMOCK = GlobalConfigUtil.GetAppSetting("AccountLinkMock") == "1";

        public AccountLinkPaymentRepository(IDbConnectionPool dbConnectionPool, ILogger<AccountLinkPaymentRepository> logger)
        {
            _dbConnectionPool = dbConnectionPool;
            _logger = logger;
        }

        public BaseResult AddTrade(TradeReqModel tradeRequestModel, string bankCode)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_PaymentCenter);

            string sql = "EXEC ausp_PaymentCenter_Trade_ACLink_AddTradeDetail_I";
            var args = new
            {
                TradeID = tradeRequestModel.TradeID,
                BankCode = bankCode,
                AccountID = tradeRequestModel.AccountID
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        public BaseResult UpdateTrade(TradeResModel tradeRequestModel)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_PaymentCenter);

            string sql = "EXEC ausp_PaymentCenter_Trade_ACLink_UpdateTradeDetail_U";
            var args = new
            {
                TradeID = tradeRequestModel.PaymentCenterTradeID,
                PayRtnCode = tradeRequestModel.PayRtnCode,
                PayRtnMsg = tradeRequestModel.PayRtnMsg,
                VerifyRtnCode = tradeRequestModel.VerifyRtnCode,
                VerifyRtnMsg = tradeRequestModel.VerifyRtnMsg,
                VirtualAccount = tradeRequestModel.VirtualAccount,
                BankTradeNo = tradeRequestModel.BankTradeNo
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        public VirtualAccountModel CreateVirtualAccount(TradeReqModel tradeRequestModel)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_PaymentCenter);

            string sql = "EXEC ausp_PaymentCenter_VirtualAccount_AddGate_I";
            var args = new
            {
                PaymentTypeID = tradeRequestModel.PaymentTypeID,
                PaymentSubTypeID = tradeRequestModel.PaymentSubTypeID,
                Amount = tradeRequestModel.Amount,
                TradeID = tradeRequestModel.TradeID
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<VirtualAccountModel>(sql, args);
        }

        public AccountLinkInfoModel _GetAccountLinkInfo(long MID, long accountID)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_PaymentCenter);

            string sql = "EXEC ausp_PaymentCenter_Trade_GetMemberAccountLinkInfo_S";
            var args = new
            {
                MID,
                accountID
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<AccountLinkInfoModel>(sql, args);
        }

        public TradeResModel _GetPayedResultInfo(TradeReqModel tradeRequestModel, BankType bankType)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);

            string sql = "EXEC ausp_PaymentCenter_AccountLink_GetPayedResultInfo_S";
            var args = new
            {
                MID = tradeRequestModel.MID,
                TradeNo = tradeRequestModel.TradeNo,
                BankType = ((int)bankType).ToString("D3")
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<TradeResModel>(sql, args);
        }

        public string Pay(string postURL, string postData)
        {
            var payResult = string.Empty;
            if (_ISMOCK)
            {
                payResult = "\"gTlZY4TQ2np5G+QH0ug3QRcTwB36yY3Fxn0c870R6udA/A63N+RolhQU6enSbDCHR8EGyzkZ42kVEfftRGNMDBUzJmUCwY8LMkGSkZfe5GY/X2jIDTcsRTp3DBf9bbdX4YoAARll1FNJwWl93larlQ==\"";
            }
            else
            {
                try
                {
                    payResult = new NetworkHelper().DoRequest(postURL, postData, "application/x-www-form-urlencoded", _TIMEOUTSEC, null, null);
                }
                catch (Exception ex)
                {
                    _logger.Fatal(ex, "[OnException] {0}", ex.Message);
                    payResult = ex.ToString().Contains("作業逾時") ? "作業逾時" : $"Exception:{ex.ToString()}";
                }
            }

            return payResult;
        }

        public string PayedVerify(string postURL, string postData)
        {
            var verifyResult = string.Empty;
            if (_ISMOCK)
            {
                verifyResult = "\"gTlZY4TQ2np5G+QH0ug3QRcTwB36yY3Fxn0c870R6udA/A63N+RolhQU6enSbDCHR8EGyzkZ42kVEfftRGNMDBUzJmUCwY8LMkGSkZfe5GY/X2jIDTcsRTp3DBf9bbdX4YoAARll1FNJwWl93larlQ==\"";
            }
            else
            {
                verifyResult = new NetworkHelper().DoRequest(postURL, postData, "application/x-www-form-urlencoded", _TIMEOUTSEC, null, null);
            }
            
            return verifyResult;
        }

        public string Refund(string postURL, string postData)
        {
            var refundResult = string.Empty;
            if (_ISMOCK)
            {
                refundResult = "\"gTlZY4TQ2np5G+QH0ug3QRcTwB36yY3Fxn0c870R6udA/A63N+RolhQU6enSbDCHR8EGyzkZ42kVEfftRGNMDBUzJmUCwY8LMkGSkZfe5GY/X2jIDTcsRTp3DBf9bbdX4YoAARll1FNJwWl93larlQ==\"";
            }
            else
            {
                refundResult = new NetworkHelper().DoRequest(postURL, postData, "application/x-www-form-urlencoded", _TIMEOUTSEC, null, null);
            }

            return refundResult;
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

        public BaseResult ReduceICash(ICashIncDecModel model)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Coins);

            string sql = "EXEC ausp_Coins_UserCoin_ReduceUserCoins_SIU";
            var args = new
            {
                TradeNo = model.TradeNo,
                MID = model.MID,
                Account = "",
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

        public BaseResult AddICash(ICashIncDecModel model)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Coins);

            string sql = "EXEC ausp_Coins_UserCoin_AddUserCoins_SI";
            var args = new
            {
                TradeNo = model.TradeNo,
                MID = model.MID,
                TradeModeID = model.TradeModeID,
                PaymentTypeID = model.PaymentTypeID,
                PaymentSubTypeID = model.PaymentSubTypeID,
                TradeRealCash = model.Amount,
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
