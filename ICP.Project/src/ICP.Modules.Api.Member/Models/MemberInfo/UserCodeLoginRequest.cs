using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    using Library.Models.AuthorizationApi;
    
    public class UserCodeLoginRequest: BaseOPAuthorizationApiRequest
    {
        /// <summary>
        /// 登入帳號
        /// </summary>
        [Required]
        [Display(Name = "登入帳號")]
        public string UserCode { get; set; }

        /// <summary>
        /// 登入密碼
        /// </summary>
        [Required]
        [Display(Name = "登入密碼")]
        public string UserPwd { get; set; }

        /// <summary>
        /// 登入方式
        /// </summary>
        [Range(1,2)]
        [Display(Name = "登入方式")]
        public byte? LoginType { get; set; }

        /// <summary>
        /// 簡訊驗證碼(提供換機驗證)
        /// </summary>
        [Display(Name = "簡訊驗證碼")]
        public string SMSAuthCode { get; set; }
    }
}
