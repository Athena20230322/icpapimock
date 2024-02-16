using ICP.Modules.Api.PaymentCenter.Models;
using ICP.Modules.Api.PaymentCenter.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.PaymentCenter.Services
{
    public class QueryService
    {
        private readonly QueryRepository _queryRepository = null;

        public QueryService(QueryRepository queryRepository)
        {
            _queryRepository = queryRepository;
        }

        /// <summary>
        /// 取得 ATM 交易資料
        /// </summary>
        /// <param name="merchantID"></param>
        /// <param name="merchantTradeNo"></param>
        /// <returns></returns>
        public TradeInfoAtm GetAtmTradeInfo(long merchantID, string merchantTradeNo)
        {
            return _queryRepository.GetAtmTradeInfo(merchantID, merchantTradeNo);
        }

        /// <summary>
        /// 以虛擬帳號查詢交易資料
        /// </summary>
        /// <param name="virtualAccount"></param>
        /// <returns></returns>
        public TradeInfo GetTradeInfoByVirtualAccount(string virtualAccount)
        {
            return _queryRepository.GetTradeInfoByVirtualAccount(virtualAccount);
        }
    }
}
