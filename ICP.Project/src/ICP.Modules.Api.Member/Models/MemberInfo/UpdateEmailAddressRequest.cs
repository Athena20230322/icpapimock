using ICP.Infrastructure.Core.Models.Consts;
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
    /// M0023 修改Email
    /// </summary>
    public class UpdateEmailAddressRequest : BaseAuthorizationApiRequest
    {
        /// <summary>
        /// 電子郵件帳號
        /// </summary>
        [Required]
        [RegularExpression(RegexConst.Email, ErrorMessage = "格式不符，請再次確認您輸入的{0}")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
