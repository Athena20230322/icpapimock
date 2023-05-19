namespace ICP.Modules.Api.Payment.Models
{
    public class AccountLinkDbRes
    {       
        /// <summary>
        /// 帳號識別碼
        /// </summary>
        public string INDTAccount { get; set; }

        /// <summary>
        /// 銀行代碼
        /// </summary>
        public string BankCode { get; set; }

        /// <summary>
        /// 銀行名稱(全名)
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// 銀行名稱(簡寫四碼)
        /// </summary>
        public string BankShortName { get; set; }

        /// <summary>
        /// 銀行帳號
        /// </summary>
        public string BankAccount { get; set; }

        /// <summary>
        /// AccountLink訊息序號
        /// </summary>
        public string MsgNo { get; set; }

        /// <summary>
        /// 是否為預設使用之銀行帳戶(1:預設 2:非預設)
        /// </summary>
        public int IsDefault { get; set; }

        /// <summary>
        /// 綁定狀態 （0:待驗證 1:連結綁定 2:取消連結綁定 3:驗證失敗）
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// BankAppName
        /// </summary>
        public string BankAppName { get; set; }

        /// <summary>
        /// 連結帳戶綁定流水號
        /// </summary>
        public long AccountID { get; set; }
    }
}
