namespace ICP.Modules.Api.AccountLink.Models
{
    /// <summary>
    /// 解綁請求
    /// </summary>
    public class ACLinkCancelDbReq
    {
        /// <summary>
        /// 會員編號
        /// </summary>        
        public long MID { get; set; }

        /// <summary>
        /// 銀行回傳成功綁定帳號識別碼
        /// </summary>
        public string INDTAccount { get; set; }

        /// <summary>
        /// 銀行代碼
        /// </summary>
        public string BankCode { get; set; }

    }
}
