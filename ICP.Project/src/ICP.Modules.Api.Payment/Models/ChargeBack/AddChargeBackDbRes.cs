using ICP.Infrastructure.Core.Models;

namespace ICP.Modules.Api.Payment.Models.ChargeBack
{
    public class AddChargeBackDbRes : BaseResult
    {
        /// <summary>
        /// 退款訂單流水號
        /// </summary>
        public long ChargeBackID { get; set; }

        /// <summary>
        /// PaymentCenter的訂單流水號
        /// </summary>
        public long PaymentCenterTradeID { get; set; }

        /// <summary>
        /// 愛金卡退款交易序號
        /// </summary>
        public string RefundTradeNo { get; set; }
    }
}
