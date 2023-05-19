using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Payment.Models.ACLink
{
    public class ACLinkModel
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
        /// 經遮蔽處理的綁定實體帳號
        /// </summary>
        public string LINKAccount { get; set; }

        /// <summary>
        /// 銀行中文名稱 (全名)
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// 銀行中文簡稱 (四碼)
        /// </summary>
        public string BankShortName { get; set; }
    }
}
