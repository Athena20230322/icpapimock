using ICP.Library.Models.AuthorizationApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    /// <summary>
    /// M0016 設定指紋辨識及FaceID開關
    /// </summary>
    public class FingerPrintPwdStatusRequest : BaseAuthorizationApiRequest
    {
        /// <summary>
        /// 狀態 False：停用, True：啟用
        /// </summary>
        public bool Status { get; set; }
    }
}
