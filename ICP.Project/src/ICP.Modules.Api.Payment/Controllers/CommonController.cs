using ICP.Infrastructure.Core.Web.Attributes;
using ICP.Infrastructure.Core.Web.Controllers;
using ICP.Modules.Api.Payment.Commands;
using ICP.Modules.Api.Payment.Models.Cashier;
using ICP.Modules.Api.Payment.Models.ChargeBack;
using System.Web.Mvc;

namespace ICP.Modules.Api.Payment.Controllers
{
    public class CommonController : BaseApiController
    {
        private readonly ChargeCommand _chargeCommand = null;
        private readonly RefundCommand _refundCommand = null;


        public CommonController
        (
            ChargeCommand chargeCommand,
            RefundCommand refundCommand
        )
        {
            _chargeCommand = chargeCommand;
            _refundCommand = refundCommand;
        }
        
        /// <summary>
        /// 內部交易共用程式
        /// </summary>
        /// <param name="request"></param>
        /// <returns>回覆Json格式,包含CheckMacValue檢核碼</returns>
        [HttpPost]
        [LogRequestByActionNameAttribute(LogTextResponse = true)]
        public ActionResult Cashier(CashierReq request)
        {
            CashierRes response = _chargeCommand.Payment(request, Request.Form);

            return Json(response);
        }

        /// <summary>
        /// 內部退款共用程式
        /// </summary>
        /// <param name="request"></param>
        /// <returns>回覆Json格式,包含CheckMacValue檢核碼</returns>
        [HttpPost]
        [LogRequestByActionNameAttribute(LogTextResponse = true)]
        public ActionResult ChargeBack(ChargeBackReq request)
        {
            ChargeBackRes response = _refundCommand.Refund(request, Request.Form);

            return Json(response);
        }

        /// <summary>
        /// 內部取消交易共用程式
        /// </summary>
        /// <param name="request"></param>
        /// <returns>回覆Json格式,包含CheckMacValue檢核碼</returns>
        [HttpPost]
        [LogRequestByActionNameAttribute(LogTextResponse = true)]
        public ActionResult Cancel(ChargeBackReq request)
        {
            request.ForCancel = 1;
            ChargeBackRes response = _refundCommand.Refund(request, Request.Form);

            return Json(response);
        }
    }
}
