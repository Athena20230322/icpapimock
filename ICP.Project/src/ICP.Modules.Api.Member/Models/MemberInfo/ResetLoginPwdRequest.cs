using ICP.Library.Models.AuthorizationApi;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    public class ResetLoginPwdRequest : BaseAuthorizationApiRequest
    {
        /// <summary>
        /// 登入/註冊TokenID (帶入M0001回傳LoginTokenID)
        /// </summary>
        [Required]
        public string LoginTokenID { get; set; }

        /// <summary>
        /// 新的登入密碼
        /// </summary>
        [Required]
        public string NewLoginPwd { get; set; }

        /// <summary>
        /// 再次確認登入密碼
        /// </summary>
        [Required]
        public string ConfirmLoginPwd { get; set; }
    }
}
