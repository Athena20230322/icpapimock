using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.ACLink
{
    public class ACLinkBankSetting
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
        /// App 綁定開啟模式
        /// 0 = 內開
        /// 1 = 外開流覽器
        /// </summary>
        public byte AppBindMode { get; set; }

        /// <summary>
        /// 是否需導填寫分行代碼、帳號頁面
        /// 0 = 否
        /// 1 = 是
        /// </summary>
        public bool AppFillBranchInfo { get; set; }
    }
}
