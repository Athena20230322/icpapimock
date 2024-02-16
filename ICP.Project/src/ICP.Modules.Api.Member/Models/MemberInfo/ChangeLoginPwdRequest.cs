using ICP.Library.Models.AuthorizationApi;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    public class ChangeLoginPwdRequest : BaseAuthorizationApiRequest
    {
        /// <summary>
        /// 變更類別 1：修改登入密碼 2：登入密碼一年到期
        /// </summary>
        public int ChangeType { get; set; }

        /// <summary>
        /// 原登入密碼 當ChangeType=1時必填
        /// </summary>        
        public string OriginalLoginPwd { get; set; }

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
