using AutoMapper;
using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Helpers;
using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Utils;
using ICP.Library.Services.Payment;
using ICP.Modules.Api.Payment.Interface;
using ICP.Modules.Api.Payment.Models.ACLink;
using ICP.Modules.Api.Payment.Models.Cashier;
using ICP.Modules.Api.Payment.Models.Payment;
using ICP.Modules.Api.Payment.Models.TopUp;
using ICP.Modules.Api.Payment.Services;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;

namespace ICP.Modules.Api.Payment.Commands
{
    public class ChargeCommand
    {
        private readonly PaymentService _paymentService = null;
        private readonly PaymentProvider _paymentProviderService = null;
        private readonly CommonService _commonService = null;
        private readonly PaymentCommonService _paymentCommonService = null;
        private readonly TopUpPaymentService _topUpPaymentService = null;
        private readonly AccountLinkService _accountLinkService = null;
        private readonly ILogger _logger = null; 

        public ChargeCommand(
            PaymentProvider paymentProviderService,
            PaymentService paymentService,
            CommonService commonService,
            PaymentCommonService paymentCommonService,
            TopUpPaymentService topUpPaymentService,
            AccountLinkService accountLinkService,
            ILogger<ChargeCommand> logger
        )
        {
            _paymentProviderService = paymentProviderService;
            _paymentService = paymentService;
            _commonService = commonService;
            _paymentCommonService = paymentCommonService;
            _topUpPaymentService = topUpPaymentService;
            _accountLinkService = accountLinkService;
            _logger = logger;
        }

        /// <summary>
        /// 建立交易訂單
        /// </summary>
        /// <param name="cashierRequest"></param>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public CashierRes Payment(CashierReq cashierRequest, NameValueCollection queryString = null)
        {
            CashierRes result = new CashierRes();
            //### 預設錯誤回傳
            result.SetCode(2034);

            if (!cashierRequest.IsValid())
            {
                string msg = cashierRequest.GetFirstErrorMessage();
                result.SetFormatError(msg);
                return result;
            }
                       
            //### 撿查驗證碼
            if (!_commonService.ValidateCheckMacValue(cashierRequest, cashierRequest.CheckMacValue, queryString))
            {
                return _commonService.SetChckMacValueResult<CashierRes, CashierRes>(result, result);
            }           

            //### 根據傳入參數判斷要走哪一個TradeMode交易(交易、儲值、轉帳、提領)
            ITradeMode trademodePayment = _paymentProviderService.GetTradeMode(cashierRequest.TradeModeID);

            if (trademodePayment == null)
            {
                return _commonService.SetChckMacValueResult<CashierRes, CashierRes>(result, result);
            }

            //### 驗證交易類型所需基本參數
            BaseResult paymentValidateResult = trademodePayment.Validate(cashierRequest);

            //### 驗證交易方式基本參數不成功回傳錯誤訊息
            if (!paymentValidateResult.IsSuccess)
            {
                return _commonService.SetChckMacValueResult<CashierRes, BaseResult>(result, paymentValidateResult);
            }

            if (eTradeMode.Topup != (eTradeMode)cashierRequest.TradeModeID)
            {
                //### 賣家額度判斷
                BaseResult sellerLimitValidateRes = _paymentService.ValidateTradeAmtLimit(cashierRequest.MerchantID, 2, cashierRequest.TradeModeID, cashierRequest.Amount);

                if (!sellerLimitValidateRes.IsSuccess)
                {
                    return _commonService.SetChckMacValueResult<CashierRes, BaseResult>(result, sellerLimitValidateRes);
                }

                //### 買家額度判斷
                BaseResult buyerLimitValidateRes = _paymentService.ValidateTradeAmtLimit(cashierRequest.MID, 1, cashierRequest.TradeModeID, cashierRequest.Amount);
                if (!buyerLimitValidateRes.IsSuccess)
                {
                    return _commonService.SetChckMacValueResult<CashierRes, BaseResult>(result, buyerLimitValidateRes);
                }
            }

            //### 自動儲值判斷(僅交易TradeModeID=1才需要)
            if (eTradeMode.Transaction == (eTradeMode)cashierRequest.TradeModeID)
            {
                DataResult<ACLinkTopUpModel> autoTopupResult = _topUpPaymentService.CheckAutoTopUp(new AutoTopUpReq()
                {
                    MID = cashierRequest.MID,
                    Amount = Convert.ToInt32(cashierRequest.Amount)
                });
                
                if (!autoTopupResult.IsSuccess)
                {
                    return _commonService.SetChckMacValueResult<CashierRes, DataResult<ACLinkTopUpModel>>(result, autoTopupResult);
                }

                if (autoTopupResult.RtnData.Amount > 0)
                {
                    CashierReq topupCashierReq = new CashierReq()
                    {
                        PlatformID = 0,
                        MerchantID = 0,
                        MerchantTradeNo = DateTime.Now.ToString("yyMMddHHmmss") + _topUpPaymentService.GetRandomNumber(),
                        MerchantTradeDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")),
                        TradeType = 1,
                        TradeModeID = 2,
                        MID = cashierRequest.MID,
                        Amount = autoTopupResult.RtnData.Amount,
                        PaymentTypeID = (int)ePaymentType.ACCOUNTLINK,
                        PaymentSubTypeID = _accountLinkService.GetACLinkBankInfo(autoTopupResult.RtnData.BankCode).PaymentSubTypeID,
                        AccountID = autoTopupResult.RtnData.AccountID,
                        Automation = 1  // 自動儲值
                    };

                    topupCashierReq.CheckMacValue = _paymentCommonService.GenerateCheckMacValue(
                                            topupCashierReq,
                                            GlobalConfigUtil.SYS_HashKey,
                                            GlobalConfigUtil.SYS_HashIV
                                        );

                    var topupResult = Payment(topupCashierReq);

                    if (!topupResult.IsSuccess)
                    {
                        return _commonService.SetChckMacValueResult<CashierRes, CashierRes>(result, topupResult);
                    }
                }
            }

            IPaymentType paymentType = _paymentProviderService.GetPaymentType(cashierRequest.PaymentTypeID);

            AddTradeDBReq addTradeDBReq = Mapper.Map<AddTradeDBReq>(cashierRequest);

            //### 建立訂單
            AddTradeDbRes addTradeResult = paymentType.AddTrade(addTradeDBReq);

            if (!addTradeResult.IsSuccess)
            {
                return _commonService.SetChckMacValueResult<CashierRes, AddTradeDbRes>(result, addTradeResult);
            }

            //### 往PaymentCenter送出交易付款            
            #region 往PaymentCenter送出交易付款
            PaymentCenterReq sendInfo = new PaymentCenterReq()
            {
                MID = cashierRequest.MID,
                PlatformID = cashierRequest.PlatformID,
                MerchantID = cashierRequest.MerchantID,
                TradeModeID = cashierRequest.TradeModeID,
                PaymentTypeID = cashierRequest.PaymentTypeID,
                PaymentSubTypeID = cashierRequest.PaymentSubTypeID,
                TradeID = addTradeResult.TradeID,
                TradeNo = addTradeResult.TradeNo,
                MerchantTradeNo = cashierRequest.MerchantTradeNo,
                Amount = cashierRequest.Amount,
                AccountID = cashierRequest.AccountID
            };

            _logger.Error($"傳送建立訂單資訊至PaymentCenter開始, MerchantTradeNo={ sendInfo.MerchantTradeNo }");

            string checkMacValue = _paymentCommonService.GenerateCheckMacValue(
                                        sendInfo,
                                        GlobalConfigUtil.SYS_HashKey,
                                        GlobalConfigUtil.SYS_HashIV
                                    );

            sendInfo.CheckMacValue = checkMacValue;

            NetworkHelper networkHelper = new NetworkHelper();

            string postUrl = $"{GlobalConfigUtil.Host_PaymentCenter_Domain}/api/PaymentCenter/PaymentCenter/GateWay";    //### PaymentCenter入口Url           

            string response = networkHelper.DoRequestWithUrlEncode(postUrl,
                                    sendInfo.GetType()
                                   .GetProperties()
                                   .Where(x => x.CanRead)
                                   .ToDictionary(x => x.Name, x => x.GetValue(sendInfo, null)?.ToString()), 600, null, null);

            _logger.Error($"傳送建立訂單資訊至PaymentCenter結束, MerchantTradeNo={ sendInfo.MerchantTradeNo }, 回傳結果={response}");

            PaymentCenterRes paymentCenterTradeResult = new PaymentCenterRes();
            paymentCenterTradeResult.SetError();

            if (!string.IsNullOrWhiteSpace(response))
            {
                response.TryParseJsonToObj<PaymentCenterRes>(out paymentCenterTradeResult);                
            }

            paymentCenterTradeResult.TradeID = addTradeResult.TradeID;
            paymentCenterTradeResult.TradeNo = addTradeResult.TradeNo;
            paymentCenterTradeResult.AccountID = cashierRequest.AccountID;

            UpdateTradeDBReq updateTradeDBReq = Mapper.Map<UpdateTradeDBReq>(paymentCenterTradeResult);

            //### PaymentCenter回傳後處理
            UpdateTradeDBRes updateTradeDBRes = paymentType.UpdateTrade(updateTradeDBReq);

            if (!paymentCenterTradeResult.IsSuccess)
            {
                return _commonService.SetChckMacValueResult<CashierRes, PaymentCenterRes>(result, paymentCenterTradeResult);
            }
            #endregion

            result = Mapper.Map<CashierRes>(updateTradeDBRes);

            return _commonService.SetChckMacValueResult<CashierRes, CashierRes>(result, result);
        }          
    }
}
