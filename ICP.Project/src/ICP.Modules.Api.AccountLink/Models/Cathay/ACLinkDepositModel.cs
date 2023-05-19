namespace ICP.Modules.Api.AccountLink.Models.Cathay
{
    /// <summary>
    /// 儲值 - API傳入參數
    /// </summary>
    public class ACLinkDepositModel : BaseACLinkModel
    {
        /// <summary>
        /// 銀行帳號
        /// </summary>
        public string BankAccount { get; set; }

        /// <summary>
        /// 儲值金額
        /// </summary>        
        public int TradeAmt { get; set; }

        /// <summary>
        /// 訂單號碼
        /// </summary>
        public string TradeNo { get; set; }
    }
}
