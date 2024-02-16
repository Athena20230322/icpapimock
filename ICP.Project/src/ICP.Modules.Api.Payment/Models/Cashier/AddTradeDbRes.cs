using ICP.Infrastructure.Core.Models;

namespace ICP.Modules.Api.Payment.Models.Cashier
{
    public class AddTradeDbRes : BaseResult
    {
        /// <summary>
        /// 訂單流水碼
        /// </summary>
        public long TradeID { get; set; }

        /// <summary>
        /// 交易編號
        /// </summary>
        public string TradeNo { get; set; }
    }
}
