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
    /// M0013 檢查安全密碼
    /// </summary>
    public class CheckPayPwdRequest : BaseAuthorizationApiRequest
    {
        /// <summary>
        /// 安全密碼
        /// </summary>
        [Required]
        public string SecPwd { get; set; }
    }
}
