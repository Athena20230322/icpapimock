using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Helpers;
using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Utils;
using ICP.Library.Services.Payment;
using ICP.Modules.Api.Payment.Models.ChargeBack;
using ICP.Modules.Api.Payment.Services;
using System.Collections.Specialized;
using System.Linq;

namespace ICP.Modules.Api.Payment.Commands
{
    public class RefundCommand
    {
        private readonly RefundService _refundService = null;
        private readonly CommonService _commonService = null;
        private readonly PaymentCommonService _paymentCommonService = null;
        private readonly ILogger _logger = null;

        public RefundCommand(
            RefundService refundService,            
            CommonService commonService,
            PaymentCommonService paymentCommonService,
            ILogger<RefundCommand> logger
        )
        {
            _refundService = refundService;
            _commonService = commonService;
            _paymentCommonService = paymentCommonService;
            _logger = logger;
        }

        /// <summary>
        /// 執行退款
        /// </summary>
        /// <param name="chargeBackRequest"></param>
        /// <returns></returns>
        public ChargeBackRes Refund(ChargeBackReq chargeBackRequest,NameValueCollection queryString = null)
        {
            ChargeBackRes result = new ChargeBackRes();
            result.SetCode(2031);

            if (!chargeBackRequest.IsValid())
            {
                string msg = chargeBackRequest.GetFirstErrorMessage();
                result.SetFormatError(msg);
                return result;
            }

            //### 撿查驗證碼
            if (!_commonService.ValidateCheckMacValue(chargeBackRequest, chargeBackRequest.CheckMacValue, queryString))
            {
                return _commonService.SetChckMacValueResult<ChargeBackRes, ChargeBackRes>(result, result);
            }

            //### 退款參數驗證
            BaseResult validteResult = _refundService.ValidateRefundParameter(chargeBackRequest);

            if (!validteResult.IsSuccess)
            {
                return _commonService.SetChckMacValueResult<ChargeBackRes, BaseResult>(result, validteResult);
            }

            //### 建立退款訂單
            AddChargeBackDbRes addRefundTradeResult = _refundService.AddChargeBack(chargeBackRequest);

            if (!addRefundTradeResult.IsSuccess)
            {
                return _commonService.SetChckMacValueResult<ChargeBackRes, AddChargeBackDbRes>(result, addRefundTradeResult);
            }

            chargeBackRequest.PaymentCenterTradeID = addRefundTradeResult.PaymentCenterTradeID;

            //### 送退款訂單至PaymentCenter          
            #region 送退款訂單至PaymentCenter
            string postUrl = $"{GlobalConfigUtil.Host_PaymentCenter_Domain}/PaymentCenter/";    //### PaymentCenter入口Url domain，再由ForCancel參數決定最後API入口

            string logText = "";

            CancelSendToPaymentCenterReq sendInfo = new CancelSendToPaymentCenterReq()
            {
                MerchantID = chargeBackRequest.MerchantID,
                PlatformID = chargeBackRequest.PlatformID,
                TradeNo = chargeBackRequest.TransactionID,
                MerchantTradeNo = chargeBackRequest.MerchantTradeNo,
                PaymentCenterTradeID = chargeBackRequest.PaymentCenterTradeID
            };

            if (chargeBackRequest.ForCancel == 1)
            {
                logText = "取消";
                postUrl += "Reversal";
            }
            else
            {
                sendInfo = new ChargeBackSendToPaymentCenterReq()
                {
                    MerchantID = chargeBackRequest.MerchantID,
                    PlatformID = chargeBackRequest.PlatformID,
                    TradeNo = chargeBackRequest.TransactionID,
                    MerchantTradeNo = chargeBackRequest.MerchantTradeNo,
                    PaymentCenterTradeID = chargeBackRequest.PaymentCenterTradeID,
                    Amount = chargeBackRequest.Amount
                };

                logText = "退款";
                postUrl += "Refund";
            }

            string checkMacValue = _paymentCommonService.GenerateCheckMacValue(
                                       sendInfo,
                                       GlobalConfigUtil.SYS_HashKey,
                                       GlobalConfigUtil.SYS_HashIV
                                   );

            sendInfo.CheckMacValue = checkMacValue;

            NetworkHelper networkHelper = new NetworkHelper();

            _logger.Trace($"傳送{ logText }到PaymentCenter開始, MerchantTradeNo={ sendInfo.MerchantTradeNo }");

            string response = networkHelper.DoRequestWithUrlEncode(postUrl,
                                    sendInfo.GetType()
                                   .GetProperties()
                                   .Where(x => x.CanRead)
                                   .ToDictionary(x => x.Name, x => x.GetValue(sendInfo, null)?.ToString()));

            _logger.Trace($"傳送{ logText }到PaymentCenter結束, MerchantTradeNo={ sendInfo.MerchantTradeNo }, 回傳結果={response}");

            ChargeBackSendToPaymentCenterRes sendPaymentCenterResult = new ChargeBackSendToPaymentCenterRes();
            sendPaymentCenterResult.SetError();

            if (!string.IsNullOrWhiteSpace(response))
            {
                response.TryParseJsonToObj<ChargeBackSendToPaymentCenterRes>(out sendPaymentCenterResult);
            }

            if (!sendPaymentCenterResult.IsSuccess)
            {
                return _commonService.SetChckMacValueResult<ChargeBackRes, ChargeBackSendToPaymentCenterRes>(result, sendPaymentCenterResult);
            }

            sendPaymentCenterResult.RtnData.ChargeBackID = addRefundTradeResult.ChargeBackID;
            sendPaymentCenterResult.RtnData.ForCancel = chargeBackRequest.ForCancel;
            #endregion

            //### 退款成功後更新訂單
            UpdateChargeBackDbRes updateRefundTradeResult = _refundService.UpdateRefundTrade(sendPaymentCenterResult);

            if (!updateRefundTradeResult.IsSuccess)
            {
                return _commonService.SetChckMacValueResult<ChargeBackRes, UpdateChargeBackDbRes>(result, updateRefundTradeResult);
            }

            //### 成功訊息回傳
            result.SetSuccess();

            if(chargeBackRequest.ForCancel != 1) //取消交易不需回傳這些參數
            {
                result.TransactionID = addRefundTradeResult.RefundTradeNo;
                result.PaymentDate = updateRefundTradeResult.PaymentDate;
            }

            #region 儲值退貨專用

            result.IcashAccount = updateRefundTradeResult.IcashAccount;
            result.TopUpAmt = sendPaymentCenterResult.RtnData.RefundAmount;
            result.MID = updateRefundTradeResult.MID;

            #endregion

            return _commonService.SetChckMacValueResult<ChargeBackRes, ChargeBackRes>(result, result);
        }        
    }
}
