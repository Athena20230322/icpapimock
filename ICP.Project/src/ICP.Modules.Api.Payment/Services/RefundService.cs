using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.Payment;
using ICP.Modules.Api.Payment.Models.ChargeBack;
using ICP.Modules.Api.Payment.Models.Payment;
using ICP.Modules.Api.Payment.Repositories;
using PaymentRepository = ICP.Library.Repositories.Payment.PaymentRepository;

namespace ICP.Modules.Api.Payment.Services
{
    public class RefundService
    {
        private readonly RefundRepository _refundRepository = null;
        private readonly PaymentRepository _paymentRepository = null;
        private readonly ILogger _logger = null;

        public RefundService(
            RefundRepository refundRepository,
            PaymentRepository paymentRepository,
            ILogger<RefundService> logger
        )
        {
            _refundRepository = refundRepository;
            _paymentRepository = paymentRepository;
            _logger = logger;
        }

        /// <summary>
        /// 退款參數驗證
        /// </summary>
        /// <param name="paymentParameter"></param>
        /// <returns></returns>
        public BaseResult ValidateRefundParameter(ChargeBackReq chargeBackRequest)
        {
            BaseResult result = new BaseResult();

            _logger.Trace($"退款參數驗證開始");

            var tradeInfo = _paymentRepository.GetTradeInfo(new GetTradeInfoDbReq()
            {
                PlatformID = chargeBackRequest.PlatformID,
                MerchantID = chargeBackRequest.MerchantID,
                MerchantTradeNo = chargeBackRequest.MerchantTradeNo
            });

            if(tradeInfo == null)
            {
                result.SetCode(2025);
            }
            //### 廠商編號驗證
            else if (chargeBackRequest.MerchantID <= 0 && (tradeInfo.TradeType == 2 || (tradeInfo.TradeType != 2 && eTradeMode.Topup != (eTradeMode)tradeInfo.TradeModeID)))
            {
                result.SetCode(2001);
            }
            //### 愛金交易序號驗證
            else if (string.IsNullOrWhiteSpace(chargeBackRequest.TransactionID))
            {
                result.SetCode(2029);
            }
            //### 退貨金額驗證，非取消交易的退貨金額不得為0
            else if (chargeBackRequest.Amount == 0 && chargeBackRequest.ForCancel == 0 && tradeInfo.TradeModeID != 2)
            {
                result.SetCode(2030);
            }
            //### 交易編號驗證
            else if (string.IsNullOrWhiteSpace(chargeBackRequest.MerchantTradeNo))
            {
                result.SetCode(2002);
            }
            //### 交易日期驗證
            else if (!chargeBackRequest.MerchantTradeDate.HasValue)
            {
                result.SetCode(2039);
            }
            else
            {
                result.SetSuccess();
            }

            _logger.Trace($"退款參數驗證結束, 回傳代碼={result.RtnCode}");

            return result;
        }

        /// <summary
        /// 建立退款訂單
        /// </summary>
        /// <param name="paymentPara"></param>
        /// <returns></returns>
        public AddChargeBackDbRes AddChargeBack(ChargeBackReq chargeBackRequest)
        {
            AddChargeBackDbRes result = new AddChargeBackDbRes();
            result.SetError();

            _logger.Trace($"新增DB退款訂單開始，MerchantTradeNo={ chargeBackRequest.MerchantTradeNo }，愛金卡交易編號(TransactionID)={chargeBackRequest.TransactionID}");

            result = _refundRepository.AddChargeBack(chargeBackRequest);

            result.SetCode(result.RtnCode);

            _logger.Trace($"新增DB退款訂單結束，MerchantTradeNo={ chargeBackRequest.MerchantTradeNo }，愛金卡交易編號(TransactionID)={chargeBackRequest.TransactionID}，結果代碼={result.RtnCode}");

            return result;
        }

        /// <summary>
        /// 更新退款訂單
        /// </summary>
        /// <param name="sendToPaymentCenterResponse"></param>
        /// <param name="forCancel"></param>
        /// <returns></returns>
        public UpdateChargeBackDbRes UpdateRefundTrade(ChargeBackSendToPaymentCenterRes sendToPaymentCenterResponse)
        {
            UpdateChargeBackDbRes result = new UpdateChargeBackDbRes();
            result.SetError();

            result = _refundRepository.UpdateChargeBack(sendToPaymentCenterResponse);

            result.SetCode(result.RtnCode);

            return result;
        }
    }
}
