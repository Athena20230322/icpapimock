using ICP.Infrastructure.Core.Models;
using ICP.Modules.Api.PaymentCenter.Enums;
using ICP.Modules.Api.PaymentCenter.Models;
using ICP.Modules.Api.PaymentCenter.Models.TransactionMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.PaymentCenter.Interface
{
    public interface ITransactionMethod
    {
        /// <summary>
        /// 驗證
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        BaseResult Validate(object tradeRequestModel);

        /// <summary>
        /// 新增訂單
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        AddTradeResult AddTrade(TradeReqModel tradeRequestModel);

        /// <summary>
        /// 更新訂單
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        BaseResult UpdateTrade(DataResult<TradeResModel> tradeResponseModel);
    }
}
