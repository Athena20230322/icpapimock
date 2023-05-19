using AutoMapper;
using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Utils;
using ICP.Library.Models.AuthorizationApi;
using ICP.Library.Models.TopUp;
using ICP.Library.Services.Payment;
using ICP.Modules.Api.Payment.Models.Cashier;
using ICP.Modules.Api.Payment.Models.ChargeBack;
using ICP.Modules.Api.Payment.Models.Pos;
using ICP.Modules.Api.Payment.Services;
using ICP.Modules.Api.Payment.Services.Pos;
using System;

namespace ICP.Modules.Api.Payment.Commands
{
    public class PosCommand
    {
        private readonly PosService _posService = null;
        private readonly PaymentCommonService _paymentCommonService = null;
        private readonly CommonService _commonService = null;
        private readonly TopUpService _topUpService = null;
        private readonly ILogger _logger = null;

        public PosCommand(
            PosService posService,
            PaymentCommonService paymentCommonService,
            CommonService commonService,
            TopUpService topUpService,
            ILogger<PosCommand> logger
        )
        {
            _posService = posService;
            _paymentCommonService = paymentCommonService;
            _commonService = commonService;
            _topUpService = topUpService;
            _logger = logger;
        }

        /// <summary>
        /// 檢核參數
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="requestModel"></param>
        /// <param name="merchantID"></param>
        /// <returns></returns>
        public DataResult<T1> ModelValidate<T1, T2>(T2 requestModel, long merchantID)
            where T1 : BaseAuthorizationApiResult
            where T2 : BasePosReq
        {
            return _posService.ModelValidate<T1, T2>(requestModel, merchantID);
        }

        /// <summary>
        /// 檢核付款參數
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DataResult<CheckOutRes> CheckOutIsValid(CheckOutReq request, long merchantID)
        {
            DataResult<CheckOutRes> result = _posService.ModelValidate<CheckOutRes, CheckOutReq>(request, merchantID); //model驗證
            int rtnCode = 1;

            if (result.RtnCode == 1)
            {
                string headCode = request.Barcode.Substring(0, 2).ToUpper();

                if (headCode != "IC")
                {
                    rtnCode = 2070; //傳入錯誤的條碼
                }

                if (request.Amount != request.ItemAmt + request.UtilityAmt + request.CommAmt && result.RtnCode == 1)
                {
                    rtnCode = 2057; //交易金額有誤
                }
            }

            if (result.RtnCode == 1 && rtnCode != 1)
            {
                result.SetCode(rtnCode);
            }

            return result;
        }

        /// <summary>
        /// 檢核儲值參數
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DataResult<TopUpRes> TopUpIsValid(TopUpReq request, long merchantID)
        {
            DataResult<TopUpRes> result = _posService.ModelValidate<TopUpRes, TopUpReq>(request, merchantID); //model驗證
            int rtnCode = 1;

            if(result.RtnCode == 1)
            {
                string headCode = request.Barcode.Substring(0, 2);
                string thirdCode = request.Barcode.Substring(2, 1);

                if (headCode != "88")
                {
                    rtnCode = 2070; //傳入錯誤的條碼
                }

                if (thirdCode == "2" && request.TopUpAmt > 1000 && result.RtnCode == 1)
                {
                    rtnCode = 2073; //發票儲值金額超過單筆上限
                }

                if (thirdCode == "1" && result.RtnCode == 1)
                {
                    string hexCode = request.Barcode.Substring(5, 4).ToUpper();
                    string hexAmount = Convert.ToString(Convert.ToInt32(request.TopUpAmt), 16).PadLeft(4, '0').ToUpper();

                    if(hexCode != hexAmount)
                    {
                        rtnCode = 2074; //條碼儲值金額與POS機金額不符
                    }
                }
            }

            if (result.RtnCode == 1 && rtnCode != 1)
            {
                result.SetCode(rtnCode);
            }

            return result;
        }

        /// <summary>
        /// 檢核儲值退貨參數
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DataResult<TopUpRefundRes> TopUpRefundIsValid(TopUpRefundReq request, long merchantID)
        {
            DataResult<TopUpRefundRes> result = _posService.ModelValidate<TopUpRefundRes, TopUpRefundReq>(request, merchantID); //model驗證
            int rtnCode = 1;

            if (result.RtnCode == 1)
            {
                string headCode = request.Barcode.Substring(0, 3);

                if (headCode == "882")
                {
                    rtnCode = 2079; //發票儲值無法進行退貨
                }
            }

            if (result.RtnCode == 1 && rtnCode != 1)
            {
                result.SetCode(rtnCode);
            }

            return result;
        }

        /// <summary>
        /// 付款流程-取得條碼參數並組成付款model
        /// </summary>
        /// <param name="checkOutRequest"></param>
        /// <returns></returns>
        public DataResult<CashierReq> PreCheckOutProcess(CheckOutReq checkOutRequest)
        {
            DataResult<CashierReq> result = new DataResult<CashierReq>();
            result.SetError();

            //取得條碼資訊
            DataResult<BarcodeInfoDbRes> barcodeInfoDbRes = _posService.GetBarcodeInfo(checkOutRequest.Barcode, 1, 0);

            if (!barcodeInfoDbRes.IsSuccess)
            {
                result.SetCode(barcodeInfoDbRes.RtnCode);
                return result;
            }

            //將POS request資料mapping至交易要用的model
            CashierReq cashierRequest = Mapper.Map<CashierReq>(checkOutRequest);

            #region 組成參數

            _commonService.GetPaymentTypeIDByPayID(barcodeInfoDbRes.RtnData.PayID, out int paymentType, out int paymentSubType, out long accountID);

            cashierRequest.TradeModeID = 1;
            cashierRequest.TradeType = 2;
            cashierRequest.MID = barcodeInfoDbRes.RtnData.MID;
            cashierRequest.PaymentTypeID = paymentType;
            cashierRequest.PaymentSubTypeID = paymentSubType;
            cashierRequest.AccountID = accountID;
            cashierRequest.PayID = barcodeInfoDbRes.RtnData.PayID;

            //POS API傳入金額包含2位小數點 e.g. 30000 (NT$300)
            cashierRequest.Amount = cashierRequest.Amount / 100;
            cashierRequest.ItemAmt = cashierRequest.ItemAmt / 100;
            cashierRequest.UtilityAmt = cashierRequest.UtilityAmt / 100;
            cashierRequest.CommAmt = cashierRequest.CommAmt / 100;
            cashierRequest.ExceptAmt1 = cashierRequest.ExceptAmt1 / 100;
            cashierRequest.ExceptAmt2 = cashierRequest.ExceptAmt2 / 100;
            cashierRequest.BonusAmt = cashierRequest.BonusAmt / 100;
            cashierRequest.DebitPoint = cashierRequest.DebitPoint / 100;
            cashierRequest.NonRedeemAmt = cashierRequest.NonRedeemAmt / 100;
            cashierRequest.NonPointAmt = cashierRequest.NonPointAmt / 100;

            cashierRequest.CheckMacValue = _paymentCommonService.GenerateCheckMacValue(cashierRequest, GlobalConfigUtil.SYS_HashKey, GlobalConfigUtil.SYS_HashIV);

            #endregion

            result.SetSuccess(cashierRequest);

            return result;
        }

        /// <summary>
        /// 付款流程-整理付款回傳參數
        /// </summary>
        /// <param name="checkOutResult"></param>
        /// <returns></returns>
        public DataResult<CheckOutRes> SetCheckOutResponse(CashierRes checkOutResult)
        {
            DataResult<CheckOutRes> result = new DataResult<CheckOutRes>();
            result.SetError();

            if (!checkOutResult.IsSuccess)
            {
                result.SetError(checkOutResult);
            }
            else
            {
                result.SetSuccess(
                    new CheckOutRes
                    {
                        TransactionID = checkOutResult.TransactionID,
                        IcashAccount = checkOutResult.IcashAccount,
                        Amount = checkOutResult.Amount * 100, //POS API傳入金額包含2位小數點 e.g. 30000 (NT$300)
                        PaymentDate = checkOutResult.PaymentDate,
                        PaymentType = checkOutResult.PaymentTypeID.ToString(),
                        IcashAccountPayID = checkOutResult.PayID,
                        BankNo = checkOutResult.BankCode,
                        BankName = checkOutResult.BankName
                    }
                );
            }

            return result;
        }

        /// <summary>
        /// 退貨流程-組成退貨model
        /// </summary>
        /// <param name="refundRequest"></param>
        /// <returns></returns>
        public DataResult<ChargeBackReq> PreRefundProcess(RefundReq refundRequest)
        {
            DataResult<ChargeBackReq> result = new DataResult<ChargeBackReq>();
            result.SetCode(2031);

            //將POS request資料mapping至交易要用的model
            ChargeBackReq chargeBackRequest = Mapper.Map<ChargeBackReq>(refundRequest);
            chargeBackRequest.Amount = chargeBackRequest.Amount / 100; //POS API傳入金額包含2位小數點 e.g. 30000 (NT$300)

            chargeBackRequest.CheckMacValue = _paymentCommonService.GenerateCheckMacValue(chargeBackRequest, GlobalConfigUtil.SYS_HashKey, GlobalConfigUtil.SYS_HashIV);

            result.SetSuccess(chargeBackRequest);

            return result;
        }

        /// <summary>
        /// 退貨流程-整理退款回傳參數
        /// </summary>
        /// <param name="refundResult"></param>
        /// <returns></returns>
        public DataResult<RefundRes> SetRefundResponse(ChargeBackRes refundResult)
        {
            DataResult<RefundRes> result = new DataResult<RefundRes>();
            result.SetCode(2031);

            if (!refundResult.IsSuccess)
            {
                result.SetError(refundResult);
            }
            else
            {
                result.SetSuccess(
                    new RefundRes()
                    {
                        TransactionID = refundResult.TransactionID,
                        PaymentDate = refundResult.PaymentDate
                    });
            }

            return result;
        }

        /// <summary>
        /// 取消交易流程-組成model
        /// </summary>
        /// <param name="cancelRequest"></param>
        /// <returns></returns>
        public DataResult<ChargeBackReq> PreCancelProcess(CancelReq cancelRequest)
        {
            DataResult<ChargeBackReq> result = new DataResult<ChargeBackReq>();
            result.SetCode(2031);

            //將POS request資料mapping至交易要用的model
            ChargeBackReq chargeBackRequest = Mapper.Map<ChargeBackReq>(cancelRequest);
            chargeBackRequest.ForCancel = 1;

            chargeBackRequest.CheckMacValue = _paymentCommonService.GenerateCheckMacValue(chargeBackRequest, GlobalConfigUtil.SYS_HashKey, GlobalConfigUtil.SYS_HashIV);

            result.SetSuccess(chargeBackRequest);

            return result;
        }

        /// <summary>
        /// 取消交易流程-整理取消回傳參數
        /// </summary>
        /// <param name="cancelResult"></param>
        /// <returns></returns>
        public DataResult<CancelRes> SetCancelResponse(ChargeBackRes cancelResult)
        {
            DataResult<CancelRes> result = new DataResult<CancelRes>();
            result.SetCode(2031);

            if (!cancelResult.IsSuccess)
            {
                result.SetError(cancelResult);
                return result;
            }

            result.SetSuccess(new CancelRes());

            return result;
        }

        /// <summary>
        /// 條碼查詢流程
        /// </summary>
        /// <param name="qryMemberInfoRequest"></param>
        /// <returns></returns>
        public DataResult<QueryMemberInfoRes> QueryMemberInfoProcess(QueryMemberInfoReq qryMemberInfoRequest)
        {
            DataResult<QueryMemberInfoRes> result = new DataResult<QueryMemberInfoRes>();
            result.SetError();

            //取得條碼資訊
            DataResult<BarcodeInfoDbRes> barcodeInfoDbRes = _posService.GetBarcodeInfo(qryMemberInfoRequest.Barcode, 3, 0);

            if (!barcodeInfoDbRes.IsSuccess)
            {
                result.SetCode(barcodeInfoDbRes.RtnCode);
                return result;
            }

            //將POS request資料mapping至交易要用的model
            QueryMemberInfoRes queryMemberInfoRes = Mapper.Map<QueryMemberInfoRes>(barcodeInfoDbRes.RtnData);

            if(barcodeInfoDbRes.RtnData.BarcodeType == "IC")
            {
                queryMemberInfoRes.BankNo = "ICP";
                queryMemberInfoRes.BankName = "愛金卡電子支付";
            }

            result.SetSuccess(queryMemberInfoRes);

            return result;
        }

        /// <summary>
        /// 儲值流程-取得條碼參數並組成儲值model
        /// </summary>
        /// <param name="topUpRequest"></param>
        /// <returns></returns>
        public DataResult<CashierReq> PreTopUpProcess(TopUpReq topUpRequest, out TopUpLimitModel topUpLimitModel)
        {
            DataResult<CashierReq> result = new DataResult<CashierReq>();
            topUpLimitModel = null;
            result.SetError();

            //取得條碼資訊
            DataResult<BarcodeInfoDbRes> barcodeInfoDbRes = _posService.GetBarcodeInfo(topUpRequest.Barcode, 2, topUpRequest.TopUpAmt);

            if (!barcodeInfoDbRes.IsSuccess)
            {
                result.SetCode(barcodeInfoDbRes.RtnCode);
                return result;
            }

            //檢核會員儲值額度
            topUpLimitModel = _topUpService.GetMemberTopUpInfo(barcodeInfoDbRes.RtnData.MID);
            if(topUpLimitModel == null)
            {
                result.SetCode(2022); //儲值訂單建立失敗
                return result;
            }

            if (topUpLimitModel.AvailableTopUp < topUpRequest.TopUpAmt)
            {
                result.SetCode(2080); //儲值金額超過可儲值額度
                _logger.Trace($"超過儲值上限: mid={barcodeInfoDbRes.RtnData.MID},tradeAmount={topUpRequest.TopUpAmt},AvaliableCash={topUpLimitModel.AvailableTopUp}");
                return result;
            }

            //取得會員狀態(暫無)

            //將POS request資料mapping至交易要用的model
            CashierReq cashierRequest = Mapper.Map<CashierReq>(topUpRequest);

            #region 組成參數

            _posService.GetPaymentTypeIDByTopUpBarcode(topUpRequest.Barcode, out int paymentType, out int paymentSubType);

            cashierRequest.TradeModeID = 2;
            cashierRequest.TradeType = 2;
            cashierRequest.MID = barcodeInfoDbRes.RtnData.MID;
            cashierRequest.PaymentTypeID = paymentType;
            cashierRequest.PaymentSubTypeID = paymentSubType;
            cashierRequest.Amount = topUpRequest.TopUpAmt;

            cashierRequest.CheckMacValue = _paymentCommonService.GenerateCheckMacValue(cashierRequest, GlobalConfigUtil.SYS_HashKey, GlobalConfigUtil.SYS_HashIV);

            #endregion

            result.SetSuccess(cashierRequest);

            return result;
        }

        /// <summary>
        /// 儲值流程-整理儲值回傳參數
        /// </summary>
        /// <param name="topUpRequest"></param>
        /// <returns></returns>
        public DataResult<TopUpRes> SetTopUpResponse(CashierRes topUpResult, TopUpReq topUpRequest, TopUpLimitModel topUpLimitModel)
        {
            DataResult<TopUpRes> result = new DataResult<TopUpRes>();
            result.SetError();

            if (!topUpResult.IsSuccess)
            {
                result.SetError(topUpResult);

                return result;
            }

            result.SetSuccess(
                new TopUpRes
                {
                    TransactionID = topUpResult.TransactionID,
                    IcashAccount = topUpResult.IcashAccount,
                    TopUpAmt = topUpRequest.TopUpAmt,
                    OriginalAmt = topUpLimitModel.TotalCoins,
                    CurrentAmt = topUpLimitModel.TotalCoins + topUpRequest.TopUpAmt,
                    TopUpDate = topUpResult.PaymentDate,
                    TopUpAmtAvailable = topUpLimitModel.AvailableTopUp - topUpRequest.TopUpAmt
                }
            );

            return result;
        }

        /// <summary>
        /// 儲值退貨流程-組成儲值退貨model
        /// </summary>
        /// <param name="refundRequest"></param>
        /// <returns></returns>
        public DataResult<ChargeBackReq> PreTopUpRefundProcess(TopUpRefundReq refundRequest)
        {
            DataResult<ChargeBackReq> result = new DataResult<ChargeBackReq>();
            result.SetCode(2031);

            //將POS request資料mapping至交易要用的model
            ChargeBackReq chargeBackRequest = Mapper.Map<ChargeBackReq>(refundRequest);

            chargeBackRequest.CheckMacValue = _paymentCommonService.GenerateCheckMacValue(chargeBackRequest, GlobalConfigUtil.SYS_HashKey, GlobalConfigUtil.SYS_HashIV);

            result.SetSuccess(chargeBackRequest);

            return result;
        }

        /// <summary>
        /// 儲值退貨流程-整理儲值回傳參數
        /// </summary>
        /// <param name="topupRefundResult"></param>
        /// <returns></returns>
        public DataResult<TopUpRefundRes> SetTopUpRefundResponse(ChargeBackRes topupRefundResult)
        {
            DataResult<TopUpRefundRes> result = new DataResult<TopUpRefundRes>();
            result.SetCode(2031);

            //取得會員帳戶資料
            TopUpLimitModel topUpLimitModel = _topUpService.GetMemberTopUpInfo(topupRefundResult.MID);
            if (topUpLimitModel == null)
            {
                _logger.Info($"[{System.Reflection.MethodBase.GetCurrentMethod().Name}][Error] 取得會員{topupRefundResult.MID}帳戶資料失敗，不影響退貨結果");
                topUpLimitModel = new TopUpLimitModel();
            }

            result.SetSuccess(
                new TopUpRefundRes()
                {
                    TransactionID = topupRefundResult.TransactionID,
                    IcashAccount = topupRefundResult.IcashAccount,
                    TopUpAmt = topupRefundResult.TopUpAmt,
                    OriginalAmt = topUpLimitModel.TotalCoins + topupRefundResult.TopUpAmt,
                    CurrentAmt = topUpLimitModel.TotalCoins,
                    TopUpDiscardDate = topupRefundResult.PaymentDate,
                    TopUpAmtAvailable = topUpLimitModel.AvailableTopUp
                });

            return result;
        }
    }
}