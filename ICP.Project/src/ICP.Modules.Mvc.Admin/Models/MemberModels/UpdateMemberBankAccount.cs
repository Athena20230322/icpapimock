using ICP.Infrastructure.Core.Models.Consts;
using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Mvc.Admin.Models.MemberModels
{
    public class UpdateMemberBankAccount
    {
        /// <summary>
        /// 銀行代號
        /// </summary>
        [Required]
        [RegularExpression(RegexConst.Only_Number, ErrorMessage = "{0} 格式錯誤")]
        [Display(Name = "銀行代號")]
        public string BankCode { get; set; }

        /// <summary>
        /// 分行代號
        /// </summary>
        [Required]
        [RegularExpression(RegexConst.Only_Number, ErrorMessage = "{0} 格式錯誤")]
        [Display(Name = "分行代號")]
        public string BankBranchCode { get; set; }

        /// <summary>
        /// 銀行帳號
        /// </summary>
        [Required]
        [StringLength(14)]
        [Display(Name = "銀行帳號")]
        public string BankAccount { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        [Display(Name = "備註")]
        public string AuthMsg { get; set; }

        /// <summary>
        /// 存摺封面
        /// </summary>
        [Display(Name = "存摺封面")]
        public string FilePath1 { get; set; }

        public string FilePath2 { get; set; }
    }
}
