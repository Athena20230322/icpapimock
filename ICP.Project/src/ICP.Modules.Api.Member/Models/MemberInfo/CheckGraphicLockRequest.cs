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
    /// M0015 檢查圖形密碼
    /// </summary>
    public class CheckGraphicLockRequest : BaseAuthorizationApiRequest
    {
        /// <summary>
        /// 圖形密碼
        /// </summary>
        [Required]
        public string GraphicPwd { get; set; }
    }
}
