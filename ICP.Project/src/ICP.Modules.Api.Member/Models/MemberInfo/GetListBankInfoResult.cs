using ICP.Library.Models.AuthorizationApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    public class GetListBankInfoResult : BaseAuthorizationApiResult
    {
        public List<CoopBank> CoopBank { get; set; }

        public List<NonCoopBank> NonCoopBank { get; set; }
    }

    /// <summary>
    /// 合作銀行
    /// </summary>
    public class CoopBank
    {
        /// <summary>
        /// 銀行代碼
        /// </summary>
        public string BankCode { get; set; }

        /// <summary>
        /// 銀行名稱
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// 銀行簡稱
        /// </summary>
        public string BankShortName { get; set; }

        /// <summary>
        /// 是否需導填寫分行代碼、帳號頁面
        /// 0 = 否
        /// 1 = 是
        /// </summary>
        public int FillBranchInfo { get; set; }
    }

    /// <summary>
    /// 非合作銀行
    /// </summary>
    public class NonCoopBank
    {
        /// <summary>
        /// 銀行代碼
        /// </summary>
        public string BankCode { get; set; }

        /// <summary>
        /// 銀行名稱
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// 銀行簡稱
        /// </summary>
        public string BankShortName { get; set; }
    }
}
