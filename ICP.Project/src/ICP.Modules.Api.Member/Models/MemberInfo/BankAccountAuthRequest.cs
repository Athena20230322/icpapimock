using ICP.Library.Models.AuthorizationApi;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    public class BankAccountAuthRequest : BaseAuthorizationApiRequest
    {
        /// <summary>
        /// 銀行代碼
        /// </summary>
        [Required]
        public string BankCode { get; set; }

        /// <summary>
        /// 銀行類別
        /// 1 = 合作銀行
        /// 2 = 非合作銀行
        /// </summary>
        [Required]
        [Range(1, 2)]
        public int BankType { get; set; }

        /// <summary>
        /// 銀行帳號
        /// </summary>
        public string BankAccount { get; set; }

        /// <summary>
        /// 分行代碼
        /// </summary>
        public string BranchCode { get; set; }
    }
}
