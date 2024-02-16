using ICP.Modules.Api.PaymentCenter.Models;
using ICP.Modules.Api.PaymentCenter.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.PaymentCenter.Commands
{
    public class QueryCommand
    {
        private readonly QueryService _queryService = null;

        public QueryCommand(QueryService queryService)
        {
            _queryService = queryService;
        }

        /// <summary>
        /// 取得 ATM 交易資料
        /// </summary>
        /// <param name="merchantID"></param>
        /// <param name="merchantTradeNo"></param>
        /// <returns></returns>
        public TradeInfoAtm GetAtmTradeInfo(long merchantID, string merchantTradeNo)
        {
            return _queryService.GetAtmTradeInfo(merchantID, merchantTradeNo);
        }
    }
}
