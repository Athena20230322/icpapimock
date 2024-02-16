namespace ICP.Modules.Api.AccountLink.Models.Cathay
{
    /// <summary>
    /// 付款 - API傳入參數
    /// </summary>
    public class ACLinkPayModel : BaseACLinkModel
    {
        /// <summary>
        /// 銀行帳號
        /// </summary>
        public string BankAccount { get; set; }

        /// <summary>
        /// 扣款金額
        /// </summary>        
        public int TradeAmt { get; set; }

        /// <summary>
        /// 訂單號碼
        /// </summary>
        public string TradeNo { get; set; }
    }
}
