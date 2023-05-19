using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Web.Controllers;
using ICP.Modules.Api.PaymentCenter.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ICP.Modules.Api.PaymentCenter.Controllers
{
    public class AtmController : BaseApiController
    {
        private readonly AtmCommand _atmCommand = null;

        public AtmController(AtmCommand atmCommand)
        {
            _atmCommand = atmCommand;
        }

        /// <summary>
        /// 取消轉帳儲值入口
        /// </summary>
        /// <param name="bankCode"></param>
        /// <param name="virtualAccount"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CancelTopUp(string bankCode, string virtualAccount)
        {
            if (string.IsNullOrWhiteSpace(bankCode) || string.IsNullOrWhiteSpace(virtualAccount))
            {
                return Json(new BaseResult() { RtnCode = 0, RtnMsg = "取消轉帳儲值輸入的參數不正確" });
            }

            switch (bankCode)
            {
                case "007":     // 第一銀行
                    return Json(FirstBankCancelTopUp(bankCode, virtualAccount));
                default:
                    return Json(new BaseResult { RtnCode = 0, RtnMsg = "查無相對應的銀行代碼" });
            }
        }

        /// <summary>
        /// 第一銀行取消轉帳儲值
        /// </summary>
        /// <param name="virtualAccount"></param>
        /// <returns></returns>
        private BaseResult FirstBankCancelTopUp(string bankCode, string virtualAccount)
        {
            BaseResult cancelTopUpResult = _atmCommand.FirstBankCancelTopUp(bankCode, virtualAccount);
            if (cancelTopUpResult.RtnCode == 1)
            {
                // 成功則變更回傳訊息
                cancelTopUpResult.RtnMsg = "取消轉帳儲值成功";
            }

            return cancelTopUpResult;
        }
    }
}
