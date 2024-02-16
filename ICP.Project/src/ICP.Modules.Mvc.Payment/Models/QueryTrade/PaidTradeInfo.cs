using System;
using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Mvc.Payment.Models.QueryTrade
{
    public class PaidTradeInfo: TradeInfo
    {
        /// <summary>
        /// 退款時間
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}")]
        public DateTime RefundDate { get; set; }

        /// <summary>
        /// 退款金額
        /// </summary>
        [DisplayFormat(DataFormatString = "NT${0:N0}")]
        public int RefundAmt { get; set; }

        /// <summary>
        /// 紅利折抵金額
        /// </summary>
        public decimal BonusAmt { get; set; }

        /// <summary>
        /// 實際付款金額
        /// </summary>
        public decimal TradeAmt { get; set; }

        /// <summary>
        /// 總金額
        /// </summary>
        public decimal TotalTradeAmt { get; set; }
    }
}