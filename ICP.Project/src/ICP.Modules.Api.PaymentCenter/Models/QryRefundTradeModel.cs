using ICP.Infrastructure.Core.Models;

namespace ICP.Modules.Api.PaymentCenter.Models
{
    public class QryRefundTradeModel : BaseResult
    {
        /// <summary>
        /// 訂單編號
        /// </summary>
        public string TradeNo { get; set; }

        /// <summary>
        /// 退款會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 退款廠商編號
        /// </summary>
        public long MerchantID { get; set; }

        /// <summary>
        /// 退款金額
        /// </summary>
        public long RefundAmount { get; set; }

        /// <summary>
        /// 交易模式
        /// </summary>
        public int TradeModeID { get; set; }

        /// <summary>
        /// 付款方式
        /// </summary>
        public int PaymentTypeID { get; set; }

        /// <summary>
        /// 收單行名稱代碼
        /// </summary>
        public int PaymentSubTypeID { get; set; }
    }
}
