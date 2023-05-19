namespace ICP.Modules.Api.Payment.Models
{
    public class AccountLinkRes
    {
        /// <summary>
        /// 付款方式識別碼
        /// </summary>
        public string PayID { get; set; }

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
        /// 實際銀行帳號(遮罩後)
        /// </summary>
        public string LinkAccount { get; set; }

        /// <summary>
        /// 銀行帳號末五碼
        /// </summary>
        public string AccountLastNo { get; set; }

        /// <summary>
        /// 是否為預設使用之銀行帳戶(1:預設 2:非預設)
        /// </summary>
        public string IsDefaultBank { get; set; }
    }
}
