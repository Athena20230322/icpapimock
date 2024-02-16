namespace ICP.Modules.Api.AccountLink.Models.First
{
    /// <summary>
    /// 第一銀行-連結帳戶交易扣款(ACLinkPay) 接收參數
    /// </summary>
    public class ACLinkPayModel : BaseACLinkModel
    {
        /// <summary>
        /// 交易訂單編號
        /// </summary>
        public string TradeNo { get; set; }

        /// <summary>
        /// 交易時間
        /// </summary>
        public string TradeTime { get; set; }

        /// <summary>
        /// 交易明細
        /// </summary>
        public string TradeNote { get; set; }

        /// <summary>
        /// 交易金額
        /// </summary>
        public int TradeAmt { get; set; }

        /// <summary>
        /// 交易流水號(Payment_TradeNo)
        /// </summary>
        public string TradeID { get; set; }

        ///// <summary>
        ///// 轉入虛擬帳戶
        ///// </summary>
        //public string PayeeAccount { get; set; }

        ///// <summary>
        ///// 來源(1:交易 3:儲值)
        ///// </summary>
        //public int TradeSource { get; set; }
    }
}
