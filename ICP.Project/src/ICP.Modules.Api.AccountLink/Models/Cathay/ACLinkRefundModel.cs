namespace ICP.Modules.Api.AccountLink.Models.Cathay
{
    /// <summary>
    /// 退款 - API傳入參數
    /// </summary>
    public class ACLinkRefundModel : BaseACLinkModel
    {
        /// <summary>
        /// 銀行帳號
        /// </summary>
        public string BankAccount { get; set; }

        /// <summary>
        /// 訊息序號
        /// </summary>
        public string MsgNo { get; set; }

        /// <summary>
        /// 退款金額
        /// </summary>        
        public int RefundAmt { get; set; }

        /// <summary>
        /// 退款時間
        /// </summary>
        public string RefundTime { get; set; }

        /// <summary>
        /// 訂單編號
        /// </summary>
        public string TradeNo { get; set; }
    }
}