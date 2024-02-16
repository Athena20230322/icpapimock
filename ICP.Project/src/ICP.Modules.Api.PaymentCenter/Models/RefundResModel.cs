using ICP.Infrastructure.Core.Models;
using System;

namespace ICP.Modules.Api.PaymentCenter.Models
{
    public class RefundResModel : BaseResult
    {
        /// <summary>
        /// 退款單號
        /// </summary>
        public long PaymentCenterTradeID { get; set; }

        /// <summary>
        /// 退款金額
        /// </summary>
        public decimal RefundAmount { get; set; }

        /// <summary>
        /// 訂單剩餘金額
        /// </summary>
        public decimal LeftAmount { get; set; }

        /// <summary>
        /// 退款日期
        /// </summary>
        public DateTime TradeDate { get; set; }
    }
}
