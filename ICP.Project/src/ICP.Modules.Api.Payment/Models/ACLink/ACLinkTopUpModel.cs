using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Payment.Models.ACLink
{
    public class ACLinkTopUpModel
    {
        /// <summary>
        /// 會員編號
        /// </summary>        
        public long MID { get; set; }

        /// <summary>
        /// 銀行代碼
        /// </summary>
        public string BankCode { get; set; }

        /// <summary>
        /// AccountLink帳號識別碼
        /// </summary>
        public long AccountID { get; set; }

        /// <summary>
        /// 儲值金額
        /// </summary>
        public int Amount { get; set; }
    }
}
