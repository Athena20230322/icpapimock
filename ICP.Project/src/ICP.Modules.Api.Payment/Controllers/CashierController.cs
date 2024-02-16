using ICP.Infrastructure.Abstractions.Authorization;
using ICP.Infrastructure.Abstractions.FilterProxy;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Web.Attributes;
using ICP.Infrastructure.Core.Web.Controllers;
using ICP.Library.Models.AuthorizationApi;
using ICP.Modules.Api.Payment.Commands;
using ICP.Modules.Api.Payment.Models;
using ICP.Modules.Api.Payment.Models.Cashier;
using ICP.Modules.Api.Payment.Models.ChargeBack;
using ICP.Modules.Api.Payment.Models.ChargeOnline;
using ICP.Modules.Api.Payment.Models.QueryOnlinecharge;
using ICP.Modules.Api.Payment.Models.RefundOnlinetransaction;
using System;
using System.Web.Mvc;

namespace ICP.Modules.Api.Payment.Controllers
{
    public class CashierController : BaseApiController
    {
        private readonly CashierCommand _cashierCommand = null;
        private readonly ChargeCommand _chargeCommand = null;
        private readonly RefundCommand _refundCommand = null;
        private readonly IUserManager _userManager = null;


        public CashierController(
            CashierCommand cashierCommand,
            ChargeCommand chargeCommand,
            RefundCommand refundCommand,
            IAuthorizationFactory authorizationFactory)
        {
            _cashierCommand = cashierCommand;
            _chargeCommand = chargeCommand;
            _refundCommand = refundCommand;
            _userManager = authorizationFactory.Create(AuthorizationType.Api);
        }

        /// <summary>
        /// 線上交易
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequestByActionNameAttribute(LogTextResponse = true)]
        public ActionResult ChargeOnline(ChargeOnlineRequest request)
        {
            long merchantID = _userManager.GetData<long>(UserDataType.MID); //### 若沒有加入Filter:ActionFilterProxy,此方式無效

            DataResult<ChargeOnlineResponse> result = new DataResult<ChargeOnlineResponse>();

            //### 線上交易前置處理
            DataResult<CashierReq> preChargeProcessResult = _cashierCommand.PreChargeProcess(request, merchantID);

            if (!preChargeProcessResult.IsSuccess)
            {
                result.SetError(preChargeProcessResult);

                return CustomResult<ICPPaymentRes>(result);
            }

            //### 呼叫共用建立訂單Function
            CashierRes chargeResult = _chargeCommand.Payment(preChargeProcessResult.RtnData);

            if (!chargeResult.IsSuccess)
            {
                result.SetError(chargeResult);

                return CustomResult<ICPPaymentRes>(result);
            }

            //### 處理回傳資訊
            result.SetSuccess(
                new ChargeOnlineResponse
                {
                    TransactionID = chargeResult.TransactionID,
                    Amount = Convert.ToInt32(chargeResult.Amount * 100),
                    PaymentDate = chargeResult.PaymentDate,
                    PaymentType = chargeResult.PaymentTypeID.ToString(),
                    IcashAccountPayID = chargeResult.PayID.ToString(),
                    BankNo = chargeResult.BankCode,
                    BankName = chargeResult.BankName,
                    IcashAccount = chargeResult.IcashAccount
                }
            );

            return CustomResult<ICPPaymentRes>(result);
        }    

        /// <summary>
        /// 交易授權結果查詢
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequestByActionNameAttribute(LogTextResponse = true)]
        public ActionResult QueryOnlineCharge(QueryOnlinechargeReq request)
        {
            DataResult<QueryOnlinechargeRes> dataResult = _cashierCommand.GetTradeInfo(request);

            return CustomResult<ICPPaymentRes>(dataResult);
        }

        /// <summary>
        /// 退款申請
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequestByActionNameAttribute(LogTextResponse = true)]
        public ActionResult RefundOnline(RefundOnlinetransactionReq request)
        {
            long merchantID = _userManager.GetData<long>(UserDataType.MID); //### 若沒有加入Filter:ActionFilterProxy,此方式無效

            DataResult<RefundOnlinetransactionRes> result = new DataResult<RefundOnlinetransactionRes>();

            //### 線上退款申請前置處理
            DataResult<ChargeBackReq> preRefundProcessResult = _cashierCommand.PreRefundProcess(request, merchantID);

            if (!preRefundProcessResult.IsSuccess)
            {
                result.SetError(preRefundProcessResult);

                return CustomResult<ICPPaymentRes>(result);
            }

            //### 呼叫共用退款Function
            ChargeBackRes chargeBackRes = _refundCommand.Refund(preRefundProcessResult.RtnData);

            if (!chargeBackRes.IsSuccess)
            {
                result.SetError(chargeBackRes);

                return CustomResult<ICPPaymentRes>(result);
            }

            //### 處理回傳資訊
            result.SetSuccess(
                    new RefundOnlinetransactionRes()
                    {
                        TransactionID = chargeBackRes.TransactionID,
                        PaymentDate = chargeBackRes.PaymentDate,
                    });

            return CustomResult<ICPPaymentRes>(result);
        }
    }
}
