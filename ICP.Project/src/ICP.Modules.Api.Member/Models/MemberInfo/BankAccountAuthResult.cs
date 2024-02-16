using ICP.Library.Models.AuthorizationApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    public class BankAccountAuthResult : BaseAuthorizationApiResult
    {
        /// <summary>
        /// 銀行網址
        /// (BankType為合作銀行時才會有值)
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// 銀行網頁開啟方式
        /// 0 = 內嵌WebView
        /// 1 = 外開瀏覽器
        /// (BankType為合作銀行時才會有值)
        /// </summary>
        public int? ViewType { get; set; }
    }
}
