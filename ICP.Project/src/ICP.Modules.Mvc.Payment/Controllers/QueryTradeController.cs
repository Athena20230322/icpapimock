using ICP.Infrastructure.Abstractions.Authorization;
using ICP.Infrastructure.Abstractions.FilterProxy;
using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Helpers;
using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Utils;
using ICP.Infrastructure.Core.Web.Attributes;
using ICP.Infrastructure.Core.Web.Controllers;
using ICP.Modules.Mvc.Payment.Commands;
using ICP.Modules.Mvc.Payment.Models.QueryTrade;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Payment.Controllers
{
    public class QueryTradeController : BaseMvcController
    {
        private readonly ILogger _logger = null;
        QueryTradeCommand _queryTradeCommand = null;
        private readonly IUserManager _userManager = null;

        public QueryTradeController(
            ILogger<BaseMemberController> logger,
            QueryTradeCommand queryTradeCommand,
            IAuthorizationFactory authorizationFactory
            )
        {
            _logger = logger;
            _queryTradeCommand = queryTradeCommand;
            _userManager = authorizationFactory.Create(AuthorizationType.Mvc);
        }

        /// <summary>
        /// ATM儲值訂單明細
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationMvc)]
        [LogRequestByActionName(LogTextResponse = true)]
        public ActionResult ATMTopUpDetail(TradeReq request)
        {
            request.MID = _userManager.MID;

            var result = _queryTradeCommand.ATMTopUpDetail(request);
            if (!result.IsSuccess)
            {
                ViewBag.errMsg = result.RtnMsg;
                return View();
            }

            return View(result.RtnData);
        }

        /// <summary>
        /// AccountLink儲值訂單明細
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationMvc)]
        [LogRequestByActionName(LogTextResponse = true)]
        public ActionResult ACLinkTopUpDetail(TradeReq request)
        {
            request.MID = _userManager.MID;

            var result = _queryTradeCommand.ACLinkTopUpDetail(request);

            if (!result.IsSuccess)
            {
                ViewBag.errMsg = result.RtnMsg;
                return View();
            }

            return View(result.RtnData);
        }

        /// <summary>
        /// 現金儲值訂單明細
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationMvc)]
        [LogRequestByActionName(LogTextResponse = true)]
        public ActionResult CashTopUpDetail(TradeReq request)
        {
            request.MID = _userManager.MID;

            var result = _queryTradeCommand.CashTopUpDetail(request);
            
            if (!result.IsSuccess)
            {
                ViewBag.errMsg = result.RtnMsg;
                return View();
            }

            return View(result.RtnData);
        }

        /// <summary>
        /// 送至PaymentCenter取消儲值
        /// </summary>
        /// <param name="bankCode"></param>
        /// <param name="virtualAccount"></param>
        /// <returns></returns>
        [LogRequestByActionName(LogTextResponse = true)]
        public ActionResult CancelTopUp(string bankCode, string virtualAccount)
        {
            string postUrl = $"{GlobalConfigUtil.Host_PaymentCenter_Domain}/Atm/CancelTopUp";
            var obj = new { bankCode, virtualAccount };
            string result = new NetworkHelper().DoRequestWithJson(postUrl, JsonConvert.SerializeObject(obj));

            return Json(JsonConvert.DeserializeObject<BaseResult>(result));
        }

        /// <summary>
        /// 付款訂單明細
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationMvc)]
        [LogRequestByActionName(LogTextResponse = true)]
        public ActionResult PaidDetail(TradeReq request)
        {
            request.MID = _userManager.MID;

            var result = _queryTradeCommand.PaidDetail(request);

            if (!result.IsSuccess)
            {
                ViewBag.errMsg = result.RtnMsg;
                return View();
            }

            return View(result.RtnData);
        }

        /// <summary>
        /// 轉帳訂單明細
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationMvc)]
        [LogRequestByActionName(LogTextResponse = true)]
        public ActionResult TransferDetail(TransferTradeReq request)
        {
            request.MID = _userManager.MID;

            var result = _queryTradeCommand.TransferDetail(request);

            if (!result.IsSuccess)
            {
                ViewBag.errMsg = result.RtnMsg;
                return View();
            }

            return View(result.RtnData);
        }

        /// <summary>
        /// 提領訂單明細
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationMvc)]
        [LogRequestByActionName(LogTextResponse = true)]
        public ActionResult BankTransferDetail(TradeReq request)
        {
            request.MID = _userManager.MID;

            var result = _queryTradeCommand.BankTransferDetail(request);

            if (!result.IsSuccess)
            {
                ViewBag.errMsg = result.RtnMsg;
                return View();
            }

            return View(result.RtnData);
        }
    }
}