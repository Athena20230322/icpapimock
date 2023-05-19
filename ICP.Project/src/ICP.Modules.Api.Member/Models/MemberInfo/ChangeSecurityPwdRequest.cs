using ICP.Library.Models.AuthorizationApi;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    /// <summary>
    /// M0012 變更安全密碼
    /// </summary>
    public class ChangeSecurityPwdRequest : BaseAuthorizationApiRequest
    {
        /// <summary>
        /// 新的安全密碼
        /// </summary>
        [Required]
        public string NewSecPwd { get; set; }

        /// <summary>
        /// 再次確認安全密碼
        /// </summary>
        [Required]
        public string ConfirmSecPwd { get; set; }
    }
}
