namespace ICP.Modules.Api.AccountLink.Models.Cathay
{
    /// <summary>
    /// 交易查詢 - API傳入參數
    /// </summary>
    public class ACLinkPayQryModel : BaseACLinkModel
    {
        /// <summary>
        /// 銀行帳號
        /// </summary>
        public string BankAccount { get; set; }

        /// <summary>
        /// 要查詢的訊息序號
        /// </summary>
        public string SerMsgNo { get; set; }
    }
}
