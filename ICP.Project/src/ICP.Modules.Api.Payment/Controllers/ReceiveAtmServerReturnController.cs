using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Web.Controllers;
using ICP.Modules.Api.Payment.Commands;
using ICP.Modules.Api.Payment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ICP.Modules.Api.Payment.Controllers
{
    public class ReceiveAtmServerReturnController : BaseApiController
    {
        private readonly ReceiveAtmServerReturnCommand _receiveAtmServerReturnCommand = null;

        public ReceiveAtmServerReturnController(ReceiveAtmServerReturnCommand receiveAtmServerReturnCommand)
        {
            _receiveAtmServerReturnCommand = receiveAtmServerReturnCommand;
        }

        /// <summary>
        /// 接收從 PaymentCenter 傳來的資料，並更新訂單
        /// </summary>
        /// <param name="paymentCenterServerReturn"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Result(PaymentCenterServerReturn paymentCenterServerReturn)
        {
            // todo 寫 log


            // 更新訂單資料
            BaseResult baseResult = _receiveAtmServerReturnCommand.ProcessReceiveData(paymentCenterServerReturn);

            return Content(baseResult.IsSuccess ? "1|OK" : "0|0-TradeError");
        }

        /// <summary>
        /// 接收從 PaymentCenter 傳來的資料，並更新銀行通知狀態
        /// </summary>
        /// <param name="paymentCenterServerReturn"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateNotifyBankStatus(PaymentCenterServerReturn paymentCenterServerReturn)
        {
            BaseResult processResult = _receiveAtmServerReturnCommand.ProcessNotifyBankData(paymentCenterServerReturn);
            return Json(processResult);
        }
    }
}
