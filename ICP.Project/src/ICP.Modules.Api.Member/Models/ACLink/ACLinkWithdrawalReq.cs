using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Models.Consts;
using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Api.Member.Models.ACLink
{
    public class ACLinkWithdrawalReq : ValidatableObject
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
        /// 銀行帳號識別碼
        /// </summary>
        [Required(ErrorMessage = "{0} 為必填")]
        [StringLength(20, MinimumLength = 10, ErrorMessage = "{0} 格式不正確")]
        public string INDTAccount { get; set; }

        /// <summary>
        /// 提領金額
        /// </summary>
        [Required(ErrorMessage = "{0} 為必填")]
        public int Amount { get; set; }

        /// <summary>
        /// 訂單編號
        /// </summary>
        [Required(ErrorMessage = "{0} 為必填")]
        public string TradeNo { get; set; }
    }
}
