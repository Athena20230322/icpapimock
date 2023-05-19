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
    /// 首次登入設定帳密
    /// </summary>
    public class FirstLoginSetAccountRequest : BaseAuthorizationApiRequest
    {  
        /// <summary>
        /// 登入帳號，格式：6-12碼半形英數混合
        /// </summary>
        [Required]
        [NotSimilar("LoginPwd")]
        [RegularExpression(RegexConst.UserCode, ErrorMessage = "{0} 格式錯誤")]
        [Display(Name = "登入帳號")]
        public string UserCode { get; set; }

        /// <summary>
        ///  登入密碼，格式：6-12碼半形英數混合
        /// </summary>
        [Required]
        [NotSimilar("UserCode")]
        [RegularExpression(RegexConst.UserPwd, ErrorMessage = "{0} 格式錯誤")]
        [Display(Name = "登入密碼")]
        public string LoginPwd { get; set; }

        /// <summary>
        /// 再次確認登入密碼
        /// </summary>
        [Required]
        [NotSimilar("UserCode")]
        [RegularExpression(RegexConst.UserPwd, ErrorMessage = "{0} 格式錯誤")]
        [Display(Name = "再次確認登入密碼")]
        public string ConfirmLoginPwd { get; set; }
    }
}