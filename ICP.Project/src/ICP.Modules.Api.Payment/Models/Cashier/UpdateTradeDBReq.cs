using ICP.Infrastructure.Core.Models;

namespace ICP.Modules.Api.Payment.Models.Cashier
{
    public class UpdateTradeDBReq : BaseResult
    {
        public PaymentCenterEncDataModel RtnData { get; set; }

        /// <summary>
        /// 訂單流水碼
        /// </summary>
        public long TradeID { get; set; }

        /// <summary>
        /// 交易編號
        /// </summary>
        public string TradeNo { get; set; }

        /// <summary>
        /// 銀行帳號識別碼
        /// </summary>
        public string AccountID { get; set; }
    }
}
