using ICP.Infrastructure.Abstractions.Authorization;
using ICP.Infrastructure.Abstractions.FilterProxy;
using ICP.Infrastructure.Core.Web.Attributes;
using ICP.Infrastructure.Core.Web.Controllers;
using ICP.Modules.Api.Payment.Commands;
using ICP.Modules.Api.Payment.Models.TopUp;
using System.Web.Mvc;

namespace ICP.Modules.Api.Payment.Controllers
{
    public class TopUpPaymentController : BaseApiController
    {
        private readonly IUserManager _userManager = null;
        private readonly TopUpPaymentCommand _topupPaymentCommand = null;

        public TopUpPaymentController(
            IAuthorizationFactory authorizationFactory, 
            TopUpPaymentCommand topupPaymentCommand)
        {
            _userManager = authorizationFactory.Create(AuthorizationType.Api);
            _topupPaymentCommand = topupPaymentCommand;
        }

        #region 取得通路儲值條碼
        /// <summary>
        /// P0007 取得通路儲值條碼
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequestByActionName(LogTextResponse = true)]
        public ActionResult GetTopUpBarCode(TopUpBarcodeReq request)
        {
            request.MID = _userManager.MID;
            var result = _topupPaymentCommand.GetTopUpBarCode(request);

            return AppResult(result);
        }
        #endregion

        #region 自動儲值設定
        /// <summary>
        /// P0013 設定自動儲值條件
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequestByActionName(LogTextResponse = true)]
        public ActionResult SetAutoTopUpCondition(AutoTopUpConditionReq request)
        {
            request.MID = _userManager.MID;
            var result = _topupPaymentCommand.SetAutoTopUpCondition(request);

            return AppResult(result);
        }

        /// <summary>
        /// P0012 設定自動儲值開關
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequestByActionName(LogTextResponse = true)]
        public ActionResult SetAutoTopUpSwitch(AutoTopUpSwitchReq request)
        {
            request.MID = _userManager.MID;
            var result = _topupPaymentCommand.SetAutoTopUpSwitch(request);

            return AppResult(result);
        }

        /// <summary>
        /// P0014 取得自動儲值設定
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequestByActionName(LogTextResponse = true)]
        public ActionResult GetAutoTopUpInfo(QryMemberInfoReq request)
        {
            request.MID = _userManager.MID;
            var result = _topupPaymentCommand.GetAutoTopUpInfo(request);

            return AppResult(result);
        }
        #endregion

        #region 虛擬帳號儲值
        /// <summary>
        /// P0011 虛擬帳號儲值
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequestByActionName(LogTextResponse = true)]
        public ActionResult TopUpByATM(TopUpByATMReq request)
        {
            request.MID = _userManager.MID;
            var result = _topupPaymentCommand.TopUpByATM(request);

            return AppResult(result);
        }
        #endregion

        #region 取得儲值頁資料
        /// <summary>
        /// P0009 取得虛擬帳號儲值頁資料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequestByActionName(LogTextResponse = true)]
        public ActionResult GetTopUpByATMInfo(GetTopUpDataReq request)
        {
            request.MID = _userManager.MID;
            var result = _topupPaymentCommand.GetTopUpByATMInfo(request);

            return AppResult(result);
        }

        /// <summary>
        /// P0008 取得連結帳戶儲值頁資料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequestByActionName(LogTextResponse = true)]
        public ActionResult GetTopUpByAccountLinkInfo(GetTopUpDataReq request)
        {
            request.MID = _userManager.MID;
            var result = _topupPaymentCommand.GetTopUpByAccountLinkInfo(request);

            return AppResult(result);
        }

        /// <summary>
        /// P0006 取得儲值通路清單
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequestByActionName(LogTextResponse = true)]
        public ActionResult GetListChannelInfo(GetTopUpDataReq request)
        {
            request.MID = _userManager.MID;
            var result = _topupPaymentCommand.GetListChannelInfo(request);

            return AppResult(result);
        }
        #endregion

    }
}
