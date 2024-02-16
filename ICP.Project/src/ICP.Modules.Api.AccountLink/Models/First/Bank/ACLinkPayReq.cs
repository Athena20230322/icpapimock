using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Api.AccountLink.Models.First
{
    /// <summary>
    /// 第一銀行-連結帳戶交易扣款(ACLinkPay) 送出值
    /// </summary>
    public class ACLinkPayReq
    {
        /// <summary>
        /// 訊息序號
        /// </summary>
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(14, ErrorMessage = "The max length of {0} is 14 char.")]
        public string MSG_NO { get; set; }

        /// <summary>
        /// 平台代碼
        /// </summary>
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(4, ErrorMessage = "The max length of {0} is 4 char.")]
        public string EC_ID { get; set; }

        /// <summary>
        /// 平台會員代號
        /// </summary>
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(12, ErrorMessage = "The max length of {0} is 12 char.")]
        public string EC_USER { get; set; }

        /// <summary>
        /// 使用者身分證字號
        /// </summary>
        [Required(ErrorMessage = "{0} is Required.")]
        [StringLength(11, ErrorMessage = "The max length of {0} is 11 char.")]
        public string CUST_ID { get; set; }

        /// <summary>
        /// 交易時間
        /// </summary>
        [Required(ErrorMessage = "{0} is Required.")]
        [StringLength(14, ErrorMessage = "The max length of {0} is 14 char.")]
        public string TRNS_TIME { get; set; }

        /// <summary>
        /// 訂單編號
        /// </summary>
        [Required(ErrorMessage = "{0} is Required.")]
        [StringLength(24, ErrorMessage = "The max length of {0} is 24 char.")]
        public string TRNS_NO { get; set; }

        /// <summary>
        /// 交易明細
        /// </summary>
        /// <remarks>使用URLEncoder.encode編碼</remarks>
        [Required(ErrorMessage = "{0} is Required.")]
        [StringLength(32, ErrorMessage = "The max length of {0} is 32 char.")]
        public string TRNS_NOTE { get; set; }

        /// <summary>
        /// 交易金額
        /// </summary>
        [Required(ErrorMessage = "{0} is Required.")]
        public int TRNS_AMT { get; set; }

        /// <summary>
        /// 帳號識別碼
        /// </summary>
        [Required(ErrorMessage = "{0} is Required.")]
        [StringLength(14, ErrorMessage = "The max length of {0} is 14 char.")]
        public string INDT_ACNT { get; set; }

        /// <summary>
        /// 轉入虛擬帳戶
        /// </summary>
        /// <remarks>由電商平台依據第一銀行的帳號規則進行虛擬帳號編列</remarks>
        [Required(ErrorMessage = "{0} is Required.")]
        [StringLength(16, ErrorMessage = "The max length of {0} is 16 char.")]
        public string PAYEE_ACNT { get; set; }

        /// <summary>
        /// 交易類別(P : 扣 款 D : 儲 值)
        /// </summary>
        [Required(ErrorMessage = "{0} is Required.")]
        public string PAY_TYPE { get; set; }

        /// <summary>
        /// 憑證序號
        /// </summary>
        public string CERT_SN { get; set; }

        /// <summary>
        /// 簽章日期時間
        /// </summary>
        [Required(ErrorMessage = "{0} is Required.")]
        [StringLength(14, ErrorMessage = "The max length of {0} is 14 char.")]
        public string SIGN_TIME { get; set; }

        /// <summary>
        /// 簽章值
        /// </summary>
        public string SIGN_VALUE { get; set; }
    }
}
