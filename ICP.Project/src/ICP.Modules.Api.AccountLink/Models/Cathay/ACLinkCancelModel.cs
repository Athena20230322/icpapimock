namespace ICP.Modules.Api.AccountLink.Models.Cathay
{
    /// <summary>
    /// 解綁 - API傳入參數
    /// </summary>
    public class ACLinkCancelModel : BaseACLinkModel
    {
        /// <summary>
        /// 銀行帳號
        /// </summary>
        public string BankAccount { get; set; }
    }
}
