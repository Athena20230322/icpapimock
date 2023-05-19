using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Models.Consts;
using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Api.Member.Models.ACLink
{
    public class ACLinkBindReq : ValidatableObject
    {
        /// <summary>
        /// 會員編號
        /// </summary>
        [Required(ErrorMessage = "{0} 為必填")]
        [Range(1, long.MaxValue, ErrorMessage = "{0} 格式不正確")]
        public long MID { get; set; }

        /// <summary>
        /// 身分證字號
        /// </summary>
        [Required(ErrorMessage = "{0} 為必填")]
        [RegularExpression(RegexConst.IDNO, ErrorMessage = "{0} 格式錯誤")]
        public string IDNO { get; set; }

        /// <summary>
        /// 銀行代碼
        /// </summary>
        [Required(ErrorMessage = "{0} 為必填")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "{0} 格式不正確")]
        public string BankCode { get; set; }

        /// <summary>
        /// 銀行帳號
        /// </summary>
        public string BankAccount { get; set; }

        /// <summary>
        /// 會員生日
        /// </summary>
        public string Birth { get; set; }

        /// <summary>
        /// 約定條款同意時間
        /// </summary>
        public string AgreeTime { get; set; }

        /// <summary>
        /// 網頁識別碼
        /// </summary>
        public string AuthId { get; set; }

        /// <summary>
        /// OTP驗證碼
        /// </summary>
        public string Otp { get; set; }

        /// <summary>
        /// 綁定旗標 (Y:送AccountLink綁定)
        /// </summary>
        public string BindFlag { get; set; }
    }
}
