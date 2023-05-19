using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Api.PaymentCenter.Enums;
using ICP.Modules.Api.PaymentCenter.Interface;
using ICP.Modules.Api.PaymentCenter.Models;
using ICP.Modules.Api.PaymentCenter.Models.TransactionMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.PaymentCenter.Services
{
    public class RefundService : ITransactionMethod
    {
        /// <summary>
        /// 驗證
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        public BaseResult Validate(object tradeRequestModel)
        {
            var result = new BaseResult().SetSuccess();
            return result;
        }

        /// <summary>
        /// 新增訂單
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        public AddTradeResult AddTrade(TradeReqModel tradeRequestModel)
        {
            throw new ArgumentNullException("未實作");
        }

        /// <summary>
        /// 更新訂單
        /// </summary>
        /// <param name="tradeResponseModel"></param>
        /// <returns></returns>
        public BaseResult UpdateTrade(DataResult<TradeResModel> tradeResponseModel)
        {
            throw new ArgumentNullException("未實作");
        }
    }
}
