using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Web.Controllers;
using ICP.Modules.Api.PaymentCenter.Commands;
using ICP.Modules.Api.PaymentCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ICP.Modules.Api.PaymentCenter.Controllers
{
    public class AtmWriteOffController : BaseApiController
    {
        private readonly AtmFirstBankCommand _atmFirstBankCommand = null;

        public AtmWriteOffController(AtmFirstBankCommand atmFirstBankCommand)
        {
            _atmFirstBankCommand = atmFirstBankCommand;
        }

        /// <summary>
        /// 第一銀行 ATM 銷帳
        /// </summary>
        /// <param name="atmFirstBankWriteOffDataReq"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult FirstBankWriteOff(AtmFirstBankWriteOffDataReq atmFirstBankWriteOffDataReq)
        {
            if (string.IsNullOrWhiteSpace(atmFirstBankWriteOffDataReq.Content))
            {
                return Content("9999");
            }

            try
            {
                BaseResult baseResult = _atmFirstBankCommand.WriteOff(atmFirstBankWriteOffDataReq);
                return Content(baseResult.RtnMsg);
            }
            catch(Exception ex)
            {
                return Content("9999");
            }
        }
    }
}
