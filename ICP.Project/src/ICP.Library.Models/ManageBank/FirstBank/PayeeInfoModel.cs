using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.ManageBank.FirstBank
{
    /// <summary>
    /// 收款人資訊
    /// </summary>
    public class PayeeInfoModel
    {
        /// <summary>
        /// 收款人帳號統一編號
        /// </summary>
        public string PayeeAccountId { get; set; }

        /// <summary>
        /// 收款人帳號戶名
        /// </summary>
        public string PayeeName { get; set; }

        /// <summary>
        /// 收款帳號銀行分行代號
        /// </summary>
        public string PayeeBankId { get; set; }

        /// <summary>
        /// 收款人帳號
        /// </summary>
        public string PayeeAccount { get; set; }
    }
}
