using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Web.Attributes;
using ICP.Infrastructure.Core.Web.Controllers;
using ICP.Modules.Api.PaymentCenter.Commands;
using System;
using System.Linq;
using System.Web.Mvc;
using ICP.Modules.Api.PaymentCenter.Models;
using ICP.Library.Services.Payment;
using ICP.Infrastructure.Core.Utils;
using ICP.Infrastructure.Core.Helpers;
using ICP.Modules.Api.PaymentCenter.Interface;
using ICP.Infrastructure.Core.Extensions;
using ICP.Modules.Api.PaymentCenter.Enums;

namespace ICP.Modules.Api.PaymentCenter.Controllers
{
    public class PaymentCenterController : BaseApiController
    {
        private readonly ITradeCommandFactory _tradeCommandFactory = null;
        
        public PaymentCenterController(ITradeCommandFactory tradeCommandFactory)
        {
            _tradeCommandFactory = tradeCommandFactory;
        }

        // 交易
        [HttpPost]
        [LogRequest(LogTextResponse = true)]
        public ActionResult GateWay(TradeReqModel requestModel)
        {
            var command = _tradeCommandFactory.Create((eTradeMode)requestModel.TradeModeID);
            var result = command?.Transaction(requestModel) ?? 
                         new DataResult<TradeResModel>().SetCode(7002);

            return Json(result);
        }

        // 退款
        [HttpPost]
        [LogRequest(LogTextResponse = true)]
        public ActionResult Refund(RefundReqModel requestModel)
        {
            var command = _tradeCommandFactory.Create(eTradeMode.Refund);
            var result = command?.Transaction(requestModel) ??
                         new DataResult<TradeResModel>().SetCode(7002);

            return Json(result);
        }

        // 取消
        [HttpPost]
        [LogRequest(LogTextResponse = true)]
        public ActionResult Reversal(ReversalReqModel requestModel)
        {
            var command = _tradeCommandFactory.Create(eTradeMode.Reversal);
            var result = command?.Transaction(requestModel) ??
                         new DataResult<TradeResModel>().SetCode(7002);

            return Json(result);
        }

        // 查詢
        [LogRequest(LogTextResponse = true)]
        public ActionResult Query(string data)
        {
            //var result = _paymentCenterCommand.GateWay(data);

            return Json("");
        }
    }
}
