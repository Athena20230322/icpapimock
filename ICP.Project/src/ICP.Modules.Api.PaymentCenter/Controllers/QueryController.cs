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
    public class QueryController : BaseApiController
    {
        private readonly QueryCommand _queryCommand = null;

        public QueryController(QueryCommand queryCommand)
        {
            _queryCommand = queryCommand;
        }

        /// <summary>
        /// 取得 ATM 交易資料
        /// </summary>
        /// <param name="merchantID"></param>
        /// <param name="merchantTradeNo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAtmTradeInfo(long merchantID, string merchantTradeNo)
        {
            if (string.IsNullOrEmpty(merchantTradeNo))
            {
                return Content("參數錯誤");
            }

            return Json(_queryCommand.GetAtmTradeInfo(merchantID, merchantTradeNo));
        }
    }
}
