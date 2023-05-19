using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Api.AccountLink.Models.First
{
    /// <summary>
    /// 第一銀行-取消連結帳戶綁定(ACLinkCancel) 送出值
    /// </summary>
    public class ACLinkCancelReq
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
        /// 帳號識別碼
        /// </summary>
        [Required(ErrorMessage = "{0} is Required.")]
        [StringLength(14, ErrorMessage = "The max length of {0} is 14 char.")]
        public string INDT_ACNT { get; set; }

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
