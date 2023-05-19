using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Models.Consts;
using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Api.AccountLink.Models.ChinaTrust
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
        /// 會員生日
        /// </summary>
        [Required(ErrorMessage = "{0} 為必填")]
        [StringLength(10, ErrorMessage = "{0} 格式不正確")]
        public string Birth { get; set; }

        /// <summary>
        /// 銀行帳號
        /// </summary>
        [Required(ErrorMessage = "{0} 為必填")]
        [StringLength(16, MinimumLength = 12, ErrorMessage = "{0} 格式不正確")]
        public string BankAccount { get; set; }

        /// <summary>
        /// 網頁識別碼
        /// </summary>
        [Required(ErrorMessage = "{0} 為必填")]
        [StringLength(4, ErrorMessage = "{0} 格式不正確")]
        public string AuthId { get; set; }

        /// <summary>
        /// OTP驗證碼
        /// </summary>
        [Required(ErrorMessage = "{0} 為必填")]
        [StringLength(7, ErrorMessage = "{0} 格式不正確")]
        public string Otp { get; set; }
    }
}
