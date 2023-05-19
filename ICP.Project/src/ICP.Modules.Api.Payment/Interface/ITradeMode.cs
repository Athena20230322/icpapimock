using ICP.Infrastructure.Core.Models;
using ICP.Modules.Api.Payment.Models.Cashier;

namespace ICP.Modules.Api.Payment.Interface
{
    public interface ITradeMode
    {
        /// <summary>
        /// 驗證
        /// </summary>
        /// <param name="paymentParameter"></param>
        /// <returns></returns>
        BaseResult Validate(CashierReq paymentParameter);
    }
}
