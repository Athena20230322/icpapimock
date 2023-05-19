using ICP.Infrastructure.Abstractions.Authorization;
using ICP.Infrastructure.Abstractions.FilterProxy;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Web.Attributes;
using ICP.Infrastructure.Core.Web.Controllers;
using ICP.Library.Models.AuthorizationApi;
using ICP.Modules.Api.Payment.Commands;
using ICP.Modules.Api.Payment.Models;
using ICP.Modules.Api.Payment.Models.AccountLimitInfo;
using ICP.Modules.Api.Payment.Models.CreateBarcode;
using ICP.Modules.Api.Payment.Models.GetMemberPaymentInfo;
using ICP.Modules.Api.Payment.Models.GetOnlineOrderInfo;
using System.Web.Mvc;

namespace ICP.Modules.Api.Payment.Controllers
{
    public class PaymentController : BaseApiController
    {
        private readonly IUserManager _userManager = null;
        private readonly PaymentCommand _paymentCommand = null;

        public PaymentController(
            IAuthorizationFactory authorizationFactory,
            PaymentCommand paymentCommand
        )
        {
            _userManager = authorizationFactory.Create(AuthorizationType.Api);
            _paymentCommand = paymentCommand;
        }

        /// <summary>
        /// 取得會員全付款方式
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequestByActionNameAttribute(LogTextResponse = true)]
        public ActionResult GetMemberPaymentInfo(GetMemberPaymentInfoReq request)
        {
            long mid = _userManager.GetData<long>(UserDataType.MID); //### 若沒有加入Filter:ActionFilterProxy,此方式無效

            DataResult<GetMemberPaymentInfoRespose> result = _paymentCommand.GetMemberPaymentInfo(request, mid);

            return AppResult(result);
        }

        /// <summary>
        /// 取得支付指示頁資料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequestByActionNameAttribute(LogTextResponse = true)]
        public ActionResult GetOnlineOrderInfo(GetOnlineOrderInfoReq request)
        {
            long mid = _userManager.GetData<long>(UserDataType.MID); //### 若沒有加入Filter:ActionFilterProxy,此方式無效

            DataResult<GetOnlineOrderInfoRes> result = _paymentCommand.GetOnlineOrderInfo(request, mid);

            return AppResult(result);
        }

        /// <summary>
        /// P0001 產生付款條碼
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequestByActionName(LogTextResponse = true)]
        public ActionResult CreateBarcode(TradeBarcodeReq request)
        {
            DataResult<TradeBarcodeRes> result = new DataResult<TradeBarcodeRes>();

            if (!request.IsValid())
            {
                result.SetFormatError(request.GetFirstErrorMessage());
            }
            else
            {
                result = _paymentCommand.CreateTradeBarcode(request, _userManager.MID); //### 若沒有加入Filter:ActionFilterProxy, _userManager.MID無效
            }

            return AppResult(result);
        }


        /// <summary>
        /// ATM 虛擬帳號儲值
        /// </summary>
        /// <param name="topUpATMReq"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult TopUpByATM(TopUpATMReq topUpATMReq)
        {

            if (!topUpATMReq.IsValid())
            {
                var validateResult = new DataResult<TopUpATMRes>();
                validateResult.SetCode(7503, topUpATMReq.GetFirstErrorMessage());
                return AppResult(validateResult);
            }

            long MID = 10010069; //_userManager.MID;

            DataResult<TopUpATMRes> result = _paymentCommand.TopUpByATM(topUpATMReq, MID);

            return AppResult(result);
        }

        /// <summary>
        /// P0015 取得交易限額資料
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequestByActionName(LogTextResponse = true)]
        public ActionResult GetAccountLimitInfo()
        {
            long mid = _userManager.MID; //### 若沒有加入Filter:ActionFilterProxy,此方式無效

            DataResult<AccountLimitInfoRes> result = _paymentCommand.GetAccountLimitInfo(mid);

            return AppResult(result);
        }
    }
}

