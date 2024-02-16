using AutoMapper;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Frameworks.ResultMapper;
using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Utils;
using ICP.Library.Models.Payment;
using ICP.Library.Services.Payment;
using ICP.Modules.Api.Payment.Models.Cashier;
using ICP.Modules.Api.Payment.Models.ChargeBack;
using ICP.Modules.Api.Payment.Models.ChargeOnline;
using ICP.Modules.Api.Payment.Models.QueryOnlinecharge;
using ICP.Modules.Api.Payment.Models.RefundOnlinetransaction;
using ICP.Modules.Api.Payment.Services;
using System;

namespace ICP.Modules.Api.Payment.Commands
{
    public class CashierCommand
    {
        private readonly PaymentService _paymentService = null;
        private readonly PaymentCommonService _paymentCommonService = null;
        private readonly ChargeOnlineService _chargeOnlineService = null;

        public CashierCommand(
            PaymentService paymentService,
            PaymentCommonService paymentCommonService,
            ChargeOnlineService chargeOnlineService            
        )
        {
            _paymentService = paymentService;
            _paymentCommonService = paymentCommonService;
            _chargeOnlineService = chargeOnlineService;
        }

        /// <summary>
        /// 線上交易前置處理
        /// </summary>
        /// <param name="chargeOnlineRequest"></param>
        /// <param name="merchantID"></param>
        /// <returns></returns>
        public DataResult<CashierReq> PreChargeProcess(ChargeOnlineRequest chargeOnlineRequest, long merchantID)
        {
            DataResult<CashierReq> result = new DataResult<CashierReq>();
            result.SetError();

            //### Model自訂驗證
            if (!chargeOnlineRequest.IsValid())
            {
                string msg = chargeOnlineRequest.GetFirstErrorMessage();
                result.SetFormatError(msg);
                return result;
            }

            //### 驗證參數PlatfromID以及MerchantID是否為觸發功能的廠商
            if(!chargeOnlineRequest.MerchantID.Equals(merchantID) && !chargeOnlineRequest.PlatformID.Equals(merchantID))
            {
                //### 廠商編號錯誤
                result.SetCode(2061);
                return result;
            }

            if (!string.IsNullOrWhiteSpace(chargeOnlineRequest.AppData))
            {
                //### 取得向OW線上交易訂單查詢的訂單資訊(從Temp Table取得)
                DataResult<CashierReq> cashierRequest = _chargeOnlineService.GetCashierReq(chargeOnlineRequest);

                if (!cashierRequest.IsSuccess)
                {
                    result.SetError(cashierRequest);
                    return result;
                }

                cashierRequest.RtnData.CheckMacValue = _paymentCommonService.GenerateCheckMacValue(cashierRequest.RtnData, GlobalConfigUtil.SYS_HashKey, GlobalConfigUtil.SYS_HashIV);

                result.SetSuccess(cashierRequest.RtnData);
            }

            return result;
        }

        /// <summary>
        /// 撈取訂單資訊處理
        /// </summary>
        /// <param name="paymentParameter"></param>
        /// <returns></returns>
        public DataResult<QueryOnlinechargeRes> GetTradeInfo(QueryOnlinechargeReq queryOnlinechargeRequest)
        {
            DataResult<QueryOnlinechargeRes> result = new DataResult<QueryOnlinechargeRes>();
            result.SetError();

            if (!queryOnlinechargeRequest.IsValid())
            {
                string msg = queryOnlinechargeRequest.GetFirstErrorMessage();
                result.SetFormatError(msg);
                return result;
            }

            GetTradeInfoDbReq getTradeInfoDbReq = Mapper.Map<GetTradeInfoDbReq>(queryOnlinechargeRequest);

            DataResult<GetTradeInfoDbRes> getTradeInfoDbRes = _paymentService.GetTradeInfo(getTradeInfoDbReq);

            if (!getTradeInfoDbRes.IsSuccess)
            {
                result.SetResult(getTradeInfoDbRes);
                return result;
            }

            ResultMapper resultHelper = new ResultMapper();

            result.SetSuccess(new QueryOnlinechargeRes()
            {
                RtnCode = getTradeInfoDbRes.RtnData.PaymentStatus.ToString().PadLeft(4, '0'),
                RtnMsg = resultHelper.GetResultMsg(getTradeInfoDbRes.RtnData.PaymentStatus),
                TransactionID = getTradeInfoDbRes.RtnData.TradeNo,
                IcashAccount = getTradeInfoDbRes.RtnData.ICPMID,
                Amount = Convert.ToInt32(getTradeInfoDbRes.RtnData.Amount),
                PaymentDate = getTradeInfoDbRes.RtnData.PaymentDate,
                PaymentType = getTradeInfoDbRes.RtnData.PaymentTypeID.ToString(),
                IcashAccountPayID = getTradeInfoDbRes.RtnData.PayID,
                BankNo = getTradeInfoDbRes.RtnData.BankCode,
                BankName = getTradeInfoDbRes.RtnData.BankName
            });

            return result;
        }

        /// <summary>
        /// 退款申請處理
        /// </summary>
        /// <param name="request"></param>
        /// <param name="merchantID"></param>
        /// <returns></returns>
        public DataResult<ChargeBackReq> PreRefundProcess(RefundOnlinetransactionReq request, long merchantID)
        {
            DataResult<ChargeBackReq> result = new DataResult<ChargeBackReq>();
            result.SetCode(2031);

            if (!request.IsValid())
            {
                string msg = request.GetFirstErrorMessage();
                result.SetFormatError(msg);
                return result;
            }

            if (!request.MerchantID.Equals(merchantID) && !request.PlatformID.Equals(merchantID))
            {
                //### 廠商編號錯誤
                result.SetCode(2061);
                return result;
            }

            ChargeBackReq chargeBackRequest = new ChargeBackReq()
            {
                PlatformID = request.PlatformID,
                MerchantID = request.MerchantID,
                TransactionID = request.TransactionID,
                Amount = request.Amount / 100,
                StoreID = request.StoreID,
                StoreName = request.StoreName,
                MerchantTradeNo = request.MerchantTradeNo,
                MerchantTradeDate = request.MerchantTradeDate,
            };

            chargeBackRequest.CheckMacValue = _paymentCommonService.GenerateCheckMacValue(chargeBackRequest, GlobalConfigUtil.SYS_HashKey, GlobalConfigUtil.SYS_HashIV);

            result.SetSuccess(chargeBackRequest);

            return result;
        }
    }
}
