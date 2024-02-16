using AutoMapper;
using ICP.Infrastructure.Core;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Helpers;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Services.Payment;
using ICP.Modules.Api.Payment.Models;
using ICP.Modules.Api.Payment.Models.Cashier;
using ICP.Modules.Api.Payment.Repositories;
using Newtonsoft.Json;
using System;
using static ICP.Infrastructure.Core.Utils.GlobalConfigUtil;

namespace ICP.Modules.Api.Payment.Services
{
    public class AtmService
    {
        private readonly AtmRepository _atmRepository = null;

        public AtmService(AtmRepository atmRepository)
        {
            _atmRepository = atmRepository;
        }

        /// <summary>
        /// 產生要送到 payment 建訂單的資料
        /// </summary>
        /// <param name="topUpATMReq"></param>
        /// <returns></returns>
        public CashierReq GenerateCashierParameters(TopUpATMReq topUpATMReq, long MID, PaymentCommonService paymentCommonService)
        {
            var cashierReq = Mapper.Map<CashierReq>(topUpATMReq);
            cashierReq.MerchantID = 10010063;
            cashierReq.MID = MID;
            cashierReq.MerchantTradeNo = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            cashierReq.MerchantTradeDate = DateTime.Now;
            cashierReq.TradeType = 1;
            cashierReq.PaymentSubTypeID = 1;
            cashierReq.Ccy = "TWD";
            //cashierReq.TopUpAmt = topUpATMReq.Amount;
            cashierReq.CheckMacValue = paymentCommonService.GenerateCheckMacValue(cashierReq, SYS_HashKey, SYS_HashIV);

            return cashierReq;
        }

        /// <summary>
        /// 送到 payment 建立訂單
        /// </summary>
        /// <param name="paymentParameter"></param>
        /// <returns></returns>
        public CashierRes SendTradeToCashier(CashierReq cashierReq)
        {
            string postUrl = $"{Host_Payment_Domain}/api/Payment/Common/Cashier";
            string response = new NetworkHelper().DoRequestWithJson(postUrl, JsonConvert.SerializeObject(cashierReq, CustomJsonSerializerSettings.DecimalConvert), 300, null, null);

            return string.IsNullOrEmpty(response) ? new CashierRes() : JsonConvert.DeserializeObject<CashierRes>(response);
        }

        /// <summary>
        /// 轉換為 api 回傳格式
        /// </summary>
        /// <param name="paymentResult"></param>
        /// <returns></returns>
        public DataResult<TopUpATMRes> TransferToTopUpATMRes(CashierRes cashierRes)
        {
            var topUpATMRes = Mapper.Map<TopUpATMRes>(cashierRes);
            topUpATMRes.IcpTradeNo = cashierRes.TransactionID;
            topUpATMRes.CreateDate = cashierRes.PaymentDate.HasValue ? cashierRes.PaymentDate.Value.ToString("yyyy/MM/dd HH:mm") : string.Empty;
            topUpATMRes.ATMAccount = cashierRes.VirtualAccount;
            topUpATMRes.BankShortName = cashierRes.BankAppName;
            topUpATMRes.LimitDate = cashierRes.ATMExpireDate;

            return new DataResult<TopUpATMRes>()
            {
                IsSuccess = cashierRes.IsSuccess,
                RtnCode = cashierRes.RtnCode,
                RtnMsg = cashierRes.RtnMsg,
                RtnData = topUpATMRes
            };
        }

        /// <summary>
        /// 把從 PaymentCenter 傳來的 JsonData 轉為指定類別物件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="paymentCenterServerReturn"></param>
        /// <returns></returns>
        public T GetPaymentCenterServerReturnObjFromJsonData<T>(PaymentCenterServerReturn paymentCenterServerReturn)
        {
            return paymentCenterServerReturn.JsonData.TryParseJsonToObj(out T tObj) ? tObj : tObj;
        }

        /// <summary>
        /// 更新 ATM 在 Payment 的訂單狀態
        /// </summary>
        /// <param name="tradeInfoAtm"></param>
        /// <returns></returns>
        public BaseResult UpdateAtmPaymentTrade(TradeInfoAtm tradeInfoAtm)
        {
            return _atmRepository.UpdateAtmPaymentTrade(tradeInfoAtm);
        }

        /// <summary>
        /// 取得 PaymentCenter 的 ATM 交易資訊
        /// </summary>
        /// <param name="tradeInfoAtm"></param>
        /// <returns></returns>
        public TradeInfoAtm GetPaymentCenterAtmTradeInfo(TradeInfoAtm tradeInfoAtm)
        {
            var postData = new
            {
                tradeInfoAtm.MerchantID,
                tradeInfoAtm.MerchantTradeNo    // 需要加密編碼?
            };

            var postUrl = "http://localhost:3313/Query/GetAtmTradeInfo";
            string response = new NetworkHelper().DoRequestWithJson(postUrl, JsonConvert.SerializeObject(postData), 300, null, null);

            return !string.IsNullOrEmpty(response) ? JsonConvert.DeserializeObject<TradeInfoAtm>(response) : new TradeInfoAtm();
        }

        /// <summary>
        /// 更新銀行通知狀態
        /// </summary>
        /// <param name="notifyBankModel"></param>
        /// <returns></returns>
        public BaseResult UpdateNotifyBankStatus(NotifyBankModel notifyBankModel)
        {
            return _atmRepository.UpdateNotifyBankStatus(notifyBankModel);
        }
    }
}
