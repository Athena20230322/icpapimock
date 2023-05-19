using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    public class AddWithdrawBalanceRequest
    {
        /// <summary>
        /// 流水號
        /// </summary>
        public long AccountID { get; set; }

        /// <summary>
        /// 銀行代碼
        /// </summary>
        public string BankCode { get; set; }

        /// <summary>
        /// 銀行帳號
        /// </summary>
        public string BankAccount { get; set; }

        /// <summary>
        /// 提領金額
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// 是否同意升級為二類會員
        /// </summary>
        public bool AgreeLevelUp { get; set; }
    }
}
