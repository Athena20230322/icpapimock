using ICP.Library.Models.AuthorizationApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    public class SendAuthSMSResult : BaseAuthorizationApiResult
    {
        /// <summary>
        /// 簡訊驗證時限
        /// </summary>
        public long ExpireRange { get; set; }

        /// <summary>
        /// 回傳驗證碼（僅於測試環境回傳此參數）
        /// </summary>
        public string AuthCode { get; set; }
    }
}
