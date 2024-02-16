using ICP.Library.Models.AuthorizationApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    public class GetBindAccountListResult : BaseAuthorizationApiResult
    {
        public List<AccountList> AccountList { get; set; }
    }

    public class AccountList
    {
        /// <summary>
        /// 銀行代碼
        /// </summary>
        public string BankCode { get; set; }

        /// <summary>
        /// 銀行帳戶編號(系統定義流水號)
        /// </summary>
        public long AccountID { get; set; }

        /// <summary>
        /// 銀行帳號末五碼
        /// </summary>
        public string AccountLastNo { get; set; }

        /// <summary>
        /// 銀行名稱(全名)
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// 銀行名稱(簡寫四碼)
        /// </summary>
        public string BankShortName { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}
