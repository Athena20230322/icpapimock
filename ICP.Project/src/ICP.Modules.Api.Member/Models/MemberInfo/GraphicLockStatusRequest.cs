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
    /// M0018 設定圖形密碼開關
    /// </summary>
    public class GraphicLockStatusRequest : BaseAuthorizationApiRequest
    {
        /// <summary>
        /// 狀態 False：停用, True：啟用
        /// </summary>
        [Required]
        public bool Status { get; set; }
    }
}
