using ICP.Infrastructure.Abstractions.Authorization;
using ICP.Infrastructure.Abstractions.FilterProxy;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Web.Attributes;
using ICP.Infrastructure.Core.Web.Controllers;
using ICP.Library.Models.TopUp;
using ICP.Modules.Api.Payment.Commands;
using ICP.Modules.Api.Payment.Models;
using ICP.Modules.Api.Payment.Models.Cashier;
using ICP.Modules.Api.Payment.Models.ChargeBack;
using ICP.Modules.Api.Payment.Models.Pos;
using System.Web.Mvc;

namespace ICP.Modules.Api.Payment.Controllers
{
    public class PosController : BaseApiController
    {
        private readonly PosCommand _posCommand = null;
        private readonly IUserManager _userManager = null;
        private readonly ChargeCommand _chargeCommand = null;
        private readonly RefundCommand _refundCommand = null;

        public PosController(
            PosCommand PosCommand,
            ChargeCommand chargeCommand,
            RefundCommand refundCommand,
            IAuthorizationFactory authorizationFactory
        )
        {
            _posCommand = PosCommand;
            _chargeCommand = chargeCommand;
            _refundCommand = refundCommand;
            _userManager = authorizationFactory.Create(AuthorizationType.Api);
        }

        /// <summary>
        /// 支付
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequestByActionName(LogTextResponse = true)]
        public ActionResult CheckOut(CheckOutReq request)
        {
            //檢核參數, 若沒有加入Filter:ActionFilterProxy,則_userManager.MID無效
            DataResult<CheckOutRes> result = _posCommand.CheckOutIsValid(request, _userManager.MID);

            if (!result.IsSuccess)
            {
                return CustomResult<ICPPaymentRes>(result);
            }

            //準備付款參數
            DataResult<CashierReq> dataCashierReq = _posCommand.PreCheckOutProcess(request);

            if (!dataCashierReq.IsSuccess)
            {
                result.SetError(dataCashierReq);
                return CustomResult<ICPPaymentRes>(result);
            }

            //付款
            CashierRes checkOutResult = _chargeCommand.Payment(dataCashierReq.RtnData);

            //整理付款回傳參數
            result = _posCommand.SetCheckOutResponse(checkOutResult);

            return CustomResult<ICPPaymentRes>(result);
        }

        /// <summary>
        /// 退款申請
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequestByActionName(LogTextResponse = true)]
        public ActionResult Refund(RefundReq request)
        {
            //檢核參數, 若沒有加入Filter:ActionFilterProxy,則_userManager.MID無效
            DataResult<RefundRes> result = _posCommand.ModelValidate<RefundRes, RefundReq>(request, _userManager.MID);

            if (!result.IsSuccess)
            {
                return CustomResult<ICPPaymentRes>(result);
            }

            //準備退款參數
            DataResult<ChargeBackReq> dataChargeBackReq = _posCommand.PreRefundProcess(request);

            if (!dataChargeBackReq.IsSuccess)
            {
                result.SetError(dataChargeBackReq);
                return CustomResult<ICPPaymentRes>(result);
            }

            //退款
            ChargeBackRes response = _refundCommand.Refund(dataChargeBackReq.RtnData);

            //整理退款回傳參數
            result = _posCommand.SetRefundResponse(response);

            return CustomResult<ICPPaymentRes>(result);
        }

        /// <summary>
        /// 取消交易
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequestByActionName(LogTextResponse = true)]
        public ActionResult Cancel(CancelReq request)
        {
            //檢核參數, 若沒有加入Filter:ActionFilterProxy,則_userManager.MID無效
            DataResult<CancelRes> result = _posCommand.ModelValidate<CancelRes, CancelReq>(request, _userManager.MID);

            if (!result.IsSuccess)
            {
                return CustomResult<ICPPaymentRes>(result);
            }

            //準備取消參數
            DataResult<ChargeBackReq> dataCancelReq = _posCommand.PreCancelProcess(request);

            if (!dataCancelReq.IsSuccess)
            {
                result.SetError(dataCancelReq);
                return CustomResult<ICPPaymentRes>(result);
            }

            //取消
            ChargeBackRes response = _refundCommand.Refund(dataCancelReq.RtnData);

            //整理取消回傳參數
            result = _posCommand.SetCancelResponse(response);

            return CustomResult<ICPPaymentRes>(result);
        }

        /// <summary>
        /// 條碼查詢
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequestByActionName(LogTextResponse = true)]
        public ActionResult QueryMemberInfo(QueryMemberInfoReq request)
        {
            //檢核參數, 若沒有加入Filter:ActionFilterProxy,則_userManager.MID無效
            DataResult<QueryMemberInfoRes> result = _posCommand.ModelValidate<QueryMemberInfoRes, QueryMemberInfoReq>(request, _userManager.MID);

            if (result.IsSuccess)
            {
                result = _posCommand.QueryMemberInfoProcess(request);
            }

            return CustomResult<ICPPaymentRes>(result);
        }

        /// <summary>
        /// 儲值
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequestByActionName(LogTextResponse = true)]
        public ActionResult TopUp(TopUpReq request)
        {
            //檢核參數, 若沒有加入Filter:ActionFilterProxy,則_userManager.MID無效
            DataResult<TopUpRes> result = _posCommand.TopUpIsValid(request, _userManager.MID);

            if (!result.IsSuccess)
            {
                return CustomResult<ICPPaymentRes>(result);
            }

            //準備儲值參數
            TopUpLimitModel topUpLimitModel = new TopUpLimitModel();
            DataResult<CashierReq> dataTopupReq = _posCommand.PreTopUpProcess(request, out topUpLimitModel);

            if (!dataTopupReq.IsSuccess)
            {
                result.SetError(dataTopupReq);
                return CustomResult<ICPPaymentRes>(result);
            }

            //儲值
            CashierRes topupResult = _chargeCommand.Payment(dataTopupReq.RtnData);

            //整理儲值回傳參數
            result = _posCommand.SetTopUpResponse(topupResult, request, topUpLimitModel);

            return CustomResult<ICPPaymentRes>(result);
        }

        /// <summary>
        /// 儲值退貨
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequestByActionName(LogTextResponse = true)]
        public ActionResult TopUpRefund(TopUpRefundReq request)
        {
            //檢核參數, 若沒有加入Filter:ActionFilterProxy,則_userManager.MID無效
            DataResult<TopUpRefundRes> result = _posCommand.TopUpRefundIsValid(request, _userManager.MID);

            if (!result.IsSuccess)
            {
                return CustomResult<ICPPaymentRes>(result);
            }

            //準備儲值退貨參數
            DataResult<ChargeBackReq> dataChargeBackReq = _posCommand.PreTopUpRefundProcess(request);

            if (!dataChargeBackReq.IsSuccess)
            {
                result.SetError(dataChargeBackReq);
                return CustomResult<ICPPaymentRes>(result);
            }

            //儲值退貨
            ChargeBackRes response = _refundCommand.Refund(dataChargeBackReq.RtnData);

            //整理儲值退貨回傳參數
            result = _posCommand.SetTopUpRefundResponse(response);

            return CustomResult<ICPPaymentRes>(result);
        }

        /// <summary>
        /// 儲值取消
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequestByActionName(LogTextResponse = true)]
        public ActionResult TopUpCancel(TopUpCancelReq request)
        {
            // 若沒有加入Filter:ActionFilterProxy,則_userManager.MID無效
            DataResult<CancelRes> result = _posCommand.ModelValidate<CancelRes, TopUpCancelReq>(request, _userManager.MID);

            if (!result.IsSuccess)
            {
                return CustomResult<ICPPaymentRes>(result);
            }

            //準備取消參數
            DataResult<ChargeBackReq> dataCancelReq = _posCommand.PreCancelProcess(request);

            if (!dataCancelReq.IsSuccess)
            {
                result.SetError(dataCancelReq);
                return CustomResult<ICPPaymentRes>(result);
            }

            //取消
            ChargeBackRes response = _refundCommand.Refund(dataCancelReq.RtnData);

            //整理取消回傳參數
            result = _posCommand.SetCancelResponse(response);

            return CustomResult<ICPPaymentRes>(result);
        }
    }
}