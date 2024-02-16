namespace ICP.Modules.Api.AccountLink.Models
{
    /// <summary>
    /// 綁定資訊回應
    /// </summary>
    public class ACLinkInfoDbRes
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

        /// <summary>
        /// 訊息序號
        /// </summary>
        public string MsgNo { get; set; }

        /// <summary>
        /// 銀行帳號
        /// </summary>
        public string BankAccount { get; set; }

        /// <summary>
        /// 綁定狀態 (0:待驗證 1:連結綁定 2:取消連結綁定 3:驗證失敗)
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 預設使用之銀行帳號
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// 顯示名稱
        /// </summary>
        public string BankName { get; set; }
    }
}
