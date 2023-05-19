using ICP.Library.Models.AccountLinkApi.Enums;

namespace ICP.Modules.Api.PaymentCenter.Models.PaymentMethod.AccountLink.Auth
{
    /// <summary>
    /// AccountLink API 傳入參數(基本參數)
    /// </summary>
    public class BaseACLinkModel
    {
        /// <summary>
        /// 會員代碼
        /// </summary>        
        public long MID { get; set; }

        /// <summary>
        /// 身分證字號
        /// </summary>
        public string IDNO { get; set; }

        /// <summary>
        /// 帳號識別碼
        /// </summary>
        public string INDTAccount { get; set; }

        /// <summary>
        /// 銀行類別
        /// </summary>
        public BankType BankType { get; set; }

        /// <summary>
        /// TimeStamp
        /// </summary>
        public string TimeStamp { get; set; }
    }
}
