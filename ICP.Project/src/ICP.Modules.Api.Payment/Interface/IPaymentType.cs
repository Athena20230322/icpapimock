using ICP.Modules.Api.Payment.Models.Cashier;

namespace ICP.Modules.Api.Payment.Interface
{
    public interface IPaymentType
    {
        /// <summary>
        /// 新增訂單
        /// </summary>
        /// <param name="addTradeDBReq"></param>
        /// <returns></returns>
        AddTradeDbRes AddTrade(AddTradeDBReq addTradeDBReq);

        /// <summary>
        /// 更新訂單
        /// </summary>
        /// <param name="updateTradeDBReq"></param>
        /// <returns></returns>
        UpdateTradeDBRes UpdateTrade(UpdateTradeDBReq updateTradeDBReq);

    }
}
