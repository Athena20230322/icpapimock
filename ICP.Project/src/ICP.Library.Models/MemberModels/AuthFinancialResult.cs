using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.MemberModels
{
    public class AuthFinancialResult : BaseResult
    {
        /// <summary>
        /// 驗證日期 
        /// </summary>
        public string AuthDate { get; set; }

        /// <summary>
        /// 銀行帳號驗證: BankID
        /// </summary>
        public long AccountID { get; set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 銀行帳號/經遮蔽處理的綁定實體帳號
        /// </summary>
        public string BankAccount { get; set; }

        /// <summary>
        /// 銀行名稱
        /// </summary>
        public string BankName { get; set; }

    }
}
