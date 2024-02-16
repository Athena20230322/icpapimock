using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    using Infrastructure.Core.ValidationAttributes;
    using Infrastructure.Core.Models.Consts;
    using Library.Models.AuthorizationApi;

    /// <summary>
    /// 設定註冊資料
    /// </summary>
    public class SetRegisterInfoRequest: BaseOPAuthorizationApiRequest
    {
        /// <summary>
        /// OPEN WALLET 提供的AuthV
        /// </summary>
        [Required]
        public string AuthV { get; set; }

        /// <summary>
        /// 手機號碼，格式：09開頭，共10碼
        /// </summary>
        [Required(ErrorMessage = "手機號碼不可為空值。")]
        [RegularExpression(RegexConst.CellPhone, ErrorMessage = "{0} 格式錯誤")]
        [Display(Name = "手機號碼")]
        public string CellPhone { get; set; }

        /// <summary>
        /// 登入帳號，格式：6-12碼半形英數混合
        /// </summary>
        [Required(ErrorMessage = "登入帳號不可為空值。")]
        [NotSimilar("UserPwd,CellPhone", ErrorMessage = "{0} 不得與登入密碼或手機號碼一致或者身分證字號規則相同。")]
        [RegularExpression(RegexConst.UserCode, ErrorMessage = "{0} 格式錯誤")]
        [Display(Name = "登入帳號")]
        public string UserCode { get; set; }

        /// <summary>
        ///  登入密碼，格式：6-12碼半形英數混合
        /// </summary>
        [Required]
        [NotSimilar("UserCode,CellPhone")]
        [RegularExpression(RegexConst.UserPwd, ErrorMessage = "{0} 格式錯誤")]
        [Display(Name = "登入密碼")]
        public string UserPwd { get; set; }

        /// <summary>
        /// 推薦碼
        /// </summary>
        [StringLength(15)]
        [Display(Name = "推薦碼")]
        public string RCCode { get; set; }
    }
}