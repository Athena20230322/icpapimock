using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.MemberModels;
using ICP.Library.Services.Payment;
using ICP.Modules.Api.Payment.Models;
using ICP.Modules.Api.Payment.Models.AccountLimitInfo;
using ICP.Modules.Api.Payment.Models.CreateBarcode;
using ICP.Modules.Api.Payment.Models.Cashier;
using ICP.Modules.Api.Payment.Models.GetMemberPaymentInfo;
using ICP.Modules.Api.Payment.Models.GetOnlineOrderInfo;
using ICP.Modules.Api.Payment.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using ICP.Library.Models.Payment;

namespace ICP.Modules.Api.Payment.Commands
{
    public class PaymentCommand
    {
        private readonly GetMemberPaymentInfoService _getMemberPaymentInfoService = null;
        private readonly GetOnlineOrderInfoService _getOnlineOrderInfoService = null;
        private readonly PaymentService _paymentService = null;
        private readonly AtmService _atmService = null;
        private readonly PaymentCommonService _paymentCommonService = null;

        public PaymentCommand(
            GetMemberPaymentInfoService getMemberPaymentInfoService,
            GetOnlineOrderInfoService getOnlineOrderInfoService,
            PaymentService paymentService,
            AtmService atmService,
            PaymentCommonService paymentCommonService
        )
        {
            _getMemberPaymentInfoService = getMemberPaymentInfoService;
            _getOnlineOrderInfoService = getOnlineOrderInfoService;
            _paymentService = paymentService;
            _atmService = atmService;
            _paymentCommonService = paymentCommonService;
        }

        /// <summary>
        /// 取得會員全付款方式
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public DataResult<GetMemberPaymentInfoRespose> GetMemberPaymentInfo(GetMemberPaymentInfoReq request, long mid)
        {
            DataResult<GetMemberPaymentInfoRespose> result = new DataResult<GetMemberPaymentInfoRespose>();
            result.SetError();

            BaseResult validateResult = _getMemberPaymentInfoService.ValidateGetMemberPaymentInfo(request);

            if (!validateResult.IsSuccess)
            {
                result.SetError(validateResult);

                return result;
            }

            List<AccountLinkRes> accountLinkInfos = null;
            iCashAccountInfo icashAccountInfo = null;

            accountLinkInfos = _getMemberPaymentInfoService.GetAccountLinks(mid);

            icashAccountInfo = _getMemberPaymentInfoService.GetiCashAccountInfo(mid);

            result.SetSuccess(new GetMemberPaymentInfoRespose
            {
                AcctLinkList = accountLinkInfos,
                IcashpayList = icashAccountInfo
            });

            return result;
        }

        /// <summary>
        /// 取得支付指示頁資料
        /// </summary>
        /// <param name="request"></param>
        /// <param name="mid"></param>
        /// <returns></returns>
        public DataResult<GetOnlineOrderInfoRes> GetOnlineOrderInfo(GetOnlineOrderInfoReq request, long mid)
        {
            DataResult<GetOnlineOrderInfoRes> result = new DataResult<GetOnlineOrderInfoRes>();

            MemberDataModel memberInfo = null;
            AccountLinkRes memberAccountLink = null;
            MerchantDataModel merchantData = null;

            BaseResult validateResult = _getOnlineOrderInfoService.ValidateGetOnlineOrderInfo(request, mid, ref memberInfo, ref memberAccountLink, ref merchantData);

            //### 撿查參數
            if (!validateResult.IsSuccess)
            {
                result.SetError(validateResult);

                return result;
            }

            //### 利用訂單編號至OW端撈取定單資料
            DataResult<AddTempTradeDbReq> dataResult = _getOnlineOrderInfoService.RedirectToOWGetTempTrade(request);

            if (!dataResult.IsSuccess)
            {
                result.SetError(dataResult);

                return result;
            }

            AddTempTradeDbReq addTempTrade = dataResult.RtnData;
            addTempTrade.MID = mid;
            addTempTrade.PayID = request.PayID;
            addTempTrade.ts = ((addTempTrade.MerchantTradeDate.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString();

            //### Insert訂單資料至Temp Table
            AddTempTradeDbRes addTempTradeTradeResult = _getOnlineOrderInfoService.AddTempTrade(addTempTrade);

            if (!addTempTradeTradeResult.IsSuccess)
            {
                result.SetError(addTempTradeTradeResult);

                return result;
            }

            result.SetSuccess(
                    new GetOnlineOrderInfoRes()
                    {
                        MerchantID = merchantData.MerchantIcpMID,
                        MerchantName = merchantData.MerchantName,
                        MerchantIconUrl = merchantData.MerchantIconUrl,
                        PayerIcpMID = memberInfo.basic.ICPMID,
                        PayerName = memberInfo.basic.CName,
                        PaymentType = addTempTrade.PayID.Substring(0, 1),
                        BankShortName = memberAccountLink != null && !string.IsNullOrWhiteSpace(memberAccountLink.BankShortName) ? memberAccountLink.BankShortName : "",
                        AccountLast5No = memberAccountLink != null && !string.IsNullOrWhiteSpace(memberAccountLink.AccountLastNo) ? memberAccountLink.AccountLastNo : "",
                        TradeAmount = addTempTrade.Amount,
                        RealAmount = addTempTrade.Amount - addTempTrade.BonusAmt,
                        BonusAmt = addTempTrade.BonusAmt,
                        AppData = JsonConvert.SerializeObject(
                                  new
                                  {
                                      EncData = new
                                      {
                                          MerchantTradeNo = addTempTrade.MerchantTradeNo,
                                          ts = addTempTrade.ts
                                      }
                                  })
                    }
                );

            return result;
        }

        /// <summary>
        /// P0001 產生付款條碼流程
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public DataResult<TradeBarcodeRes> CreateTradeBarcode(TradeBarcodeReq request, long mid)
        {
            DataResult<TradeBarcodeRes> result = new DataResult<TradeBarcodeRes>();
            result.SetError();

            AddBarcodeDBReq addBarcodeDBReq = new AddBarcodeDBReq
            {
                MID = mid,
                BarcodeType = "IC",
                Timestamp = request.Timestamp,
                PaymentType = request.PaymentType,
                PayID = request.PayID
            };

            AddBarcodeDBRes addBarcodeDBRes = _paymentService.AddBarcode(addBarcodeDBReq);

            if (!addBarcodeDBRes.IsSuccess)
            {
                result.SetCode(addBarcodeDBRes.RtnCode);

                return result;
            }

            result.SetSuccess(new TradeBarcodeRes()
            {
                Timestamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                Barcode = addBarcodeDBRes.Barcode
            });

            return result;
        }


        /// <summary>
        /// ATM 虛擬帳號儲值
        /// </summary>
        /// <param name="topUpATMReq"></param>
        /// <returns></returns>
        public DataResult<TopUpATMRes> TopUpByATM(TopUpATMReq topUpATMReq, long MID)
        {
            // 產生要送到 payment 建訂單的資料
            CashierReq cashierReq = _atmService.GenerateCashierParameters(topUpATMReq, MID, _paymentCommonService);

            // 傳送到 payment 建訂單並取得虛擬帳號
            CashierRes cashierRes = _atmService.SendTradeToCashier(cashierReq);

            // 轉換為 api 回傳指定的參數
            var result = new DataResult<TopUpATMRes>();
            result = _atmService.TransferToTopUpATMRes(cashierRes);

            return result;
        }

        /// <summary>
        /// P0015 取得交易限額資料
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public DataResult<AccountLimitInfoRes> GetAccountLimitInfo(long mid)
        {
            var result = new DataResult<AccountLimitInfoRes>();

            var infoResult = _paymentService.GetAccountLimitInfo(mid);

            if (!infoResult.IsSuccess)
            {
                result.SetCode(0);
                return result;
            }

            result.SetSuccess(infoResult.RtnData);

            return result;
        }

    }
}
