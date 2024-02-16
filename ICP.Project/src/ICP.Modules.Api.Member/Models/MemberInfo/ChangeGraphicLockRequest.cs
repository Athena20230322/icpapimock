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
    /// M0014 變更圖形密碼
    /// </summary>
    public class ChangeGraphicLockRequest : BaseAuthorizationApiRequest
    {
        /// <summary>
        /// 新的圖形密碼
        /// </summary>
        [Required]
        public string NewGraphicPwd { get; set; }

        /// <summary>
        /// 再次確認圖形密碼
        /// </summary>
        [Required]
        public string ConfirmGraphicPwd { get; set; }
    }
}
