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
    /// M0017 驗證指紋辨識及FaceID
    /// </summary>
    public class CheckFingerPrintPwdRequest : BaseAuthorizationApiRequest
    {
        /// <summary>
        /// 指紋/FaceID密碼
        /// </summary>
        [Required]
        public string FingerPrintPwd { get; set; }

    }
}
