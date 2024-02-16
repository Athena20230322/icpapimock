using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Api.AccountLink.Models.First
{
    /// <summary>
    /// 第一銀行-連結帳戶交易退款(ACLinkRefund) 送出值
    /// </summary>
    public class ACLinkRefundReq
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
        /// 退款交易時間
        /// </summary>
        [StringLength(14, ErrorMessage = "The max length of {0} is 14 char.")]
        [Required(ErrorMessage = "{0} is Required.")]
        public string REFUND_TIME { get; set; }

        /// <summary>
        /// 退款(原)訂單編號
        /// </summary>
        [StringLength(24, ErrorMessage = "The max length of {0} is 24 char.")]
        [Required(ErrorMessage = "{0} is Required.")]
        public string TRNS_NO { get; set; }

        /// <summary>
        /// 退款交易明細
        /// </summary>
        /// <remarks>使用URLEncoder.encode編碼</remarks>
        [StringLength(32, ErrorMessage = "The max length of {0} is 32 char.")]
        [Required(ErrorMessage = "{0} is Required.")]
        public string REFUND_NOTE { get; set; }

        /// <summary>
        /// 退款金額
        /// </summary>
        [Required(ErrorMessage = "{0} is Required.")]
        public int REFUND_AMT { get; set; }

        /// <summary>
        /// 簽章日期時間
        /// </summary>
        [StringLength(14, ErrorMessage = "The max length of {0} is 14 char.")]
        [Required(ErrorMessage = "{0} is Required.")]
        public string SIGN_TIME { get; set; }

        /// <summary>
        /// 簽章值
        /// </summary>
        [Required(ErrorMessage = "{0} is Required.")]
        public string SIGN_VALUE { get; set; }

        /// <summary>
        /// 憑證序號
        /// </summary>
        [Required(ErrorMessage = "{0} is Required.")]
        public string CERT_SN { get; set; }
    }
}
