using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.MemberModels
{
    public class MemberBankDetail
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
        /// 是否為合作銀行
        /// </summary>
        public bool isCooperate { get; set; }

        /// <summary>
        /// 是否需導填寫分行代碼、帳號頁面
        /// 0 = 否
        /// 1 = 是
        /// </summary>
        public bool AppFillBranchInfo { get; set; }
    }
}
