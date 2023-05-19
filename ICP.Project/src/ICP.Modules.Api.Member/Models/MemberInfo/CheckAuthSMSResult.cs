using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    using Library.Models.AuthorizationApi;

    public class CheckAuthSMSResult: BaseAuthorizationApiResult
    {
        /// <summary>
        /// 驗證錯誤次數
        /// </summary>
        public int AuthErrorCount { get; set; }

        /// <summary>
        /// 登入帳號 SMSAuthType=2，且驗證簡訊碼正確時才會回傳
        /// </summary>
        public string UserCode { get; set; }
    }
}
