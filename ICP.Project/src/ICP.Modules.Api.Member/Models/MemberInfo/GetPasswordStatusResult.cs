using ICP.Library.Models.AuthorizationApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    public class GetPasswordStatusResult : BaseAuthorizationApiResult
    {

        /// <summary>
        /// 圖形密碼設定狀態 False：停用, True：啟用
        /// </summary>
        public bool GraphicLockStatus { get; set; }

        /// <summary>
        /// 是否曾經設定過圖形密碼 False：否, True：是
        /// </summary>
        public bool IsExistsSetGraphicLock { get; set; }

        /// <summary>
        /// 指紋辨識/Face ID設定狀態 False：停用, True：啟用
        /// </summary> 
        public bool FingerPrintPwdStatus { get; set; }
    }
}
