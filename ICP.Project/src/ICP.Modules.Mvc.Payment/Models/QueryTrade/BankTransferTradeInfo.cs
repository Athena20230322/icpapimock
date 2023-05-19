using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Mvc.Payment.Models.QueryTrade
{
    public class BankTransferTradeInfo : TradeInfo
    {
        /// <summary>
        /// 特店手續費(付款完畢後結算)
        /// </summary>
        [DisplayFormat(DataFormatString = "NT${0:N0}")]
        public int HandlingCharge { get; set; }

        /// <summary>
        /// 帳戶扣除金額
        /// </summary>
        [DisplayFormat(DataFormatString = "NT${0:N0}")]
        public int ActualAmount { get; set; }

        /// <summary>
        /// 付款狀態 0:預設 1:已完成轉帳 2: 轉帳失敗
        /// </summary>
        public int PayStatus { get; set; }
    }
}