namespace ICP.Modules.Api.AccountLink.Models.First
{
    /// <summary>
    ///  第一銀行-連結帳戶交易退款(ACLinkRefund) 接收參數
    /// </summary>
    public class ACLinkRefundModel : BaseACLinkModel
    {
        /// <summary>
        /// 退款(原)訂單編號
        /// </summary>
        public string TradeNo { get; set; }

        /// <summary>
        /// 退款交易時間
        /// </summary>
        public string RefundTime { get; set; }

        /// <summary>
        /// 退款交易明細
        /// </summary>
        public string RefundNote { get; set; }

        /// <summary>
        /// 退款金額
        /// </summary>
        public int RefundAmt { get; set; }
    }
}
