using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Models.Consts;
using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Api.Member.Models.ACLink
{
    public class ACLinkApplyReq : ValidatableObject
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
        /// 銀行代碼
        /// </summary>
        [Required(ErrorMessage = "{0} 為必填")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "{0} 格式不正確")]
        public string BankCode { get; set; }

        /// <summary>
        /// 銀行帳號
        /// </summary>
        [Required(ErrorMessage = "{0} 為必填")]
        [StringLength(16, MinimumLength = 12, ErrorMessage = "{0} 格式不正確")]
        public string BankAccount { get; set; }

        /// <summary>
        /// 約定條款同意時間
        /// </summary>
        [Required(ErrorMessage = "{0} 為必填")]
        [StringLength(14, ErrorMessage = "{0} 格式不正確")]
        public string AgreeTime { get; set; }
    }
}
