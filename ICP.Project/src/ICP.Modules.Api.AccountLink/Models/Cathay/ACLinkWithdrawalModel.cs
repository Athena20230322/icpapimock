namespace ICP.Modules.Api.AccountLink.Models.Cathay
{
    /// <summary>
    /// 提領 - API傳入參數
    /// </summary>
    public class ACLinkWithdrawalModel : BaseACLinkModel
    {
        /// <summary>
        /// 銀行帳號
        /// </summary>
        public string BankAccount { get; set; }

        /// <summary>
        /// 提領金額
        /// </summary>        
        public int Amount { get; set; }

        /// <summary>
        /// 訂單號碼
        /// </summary>
        public string TradeNo { get; set; }
    }
}
