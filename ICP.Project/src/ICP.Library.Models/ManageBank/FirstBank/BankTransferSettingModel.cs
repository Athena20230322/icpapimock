using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.ManageBank.FirstBank
{
    public class BankTransferSettingModel
    {
        /// <summary>
        /// 客戶統編含重複續號 :統一編號+空白+空白+重複續號 12345678  0
        /// </summary>
        public string CustId { get; set; }

        /// <summary>
        /// 付款(扣款)帳號所屬統一編號 ID(含重複序號) : 愛金卡申請付款帳號之統編
        /// </summary>
        public string PayAccountId { get; set; }

        /// <summary>
        /// 付款帳號戶名 : 愛金卡申請付款帳號之戶名
        /// </summary>
        public string PayAccountName { get; set; }

        /// <summary>
        /// 付款(扣款)帳號 : 愛金卡申請付款之帳號
        /// </summary>
        public string PayAccount { get; set; }

        /// <summary>
        /// 手續費分擔別 13:收款人負擔 ;15:付款人負擔
        /// </summary>
        public string ChargeRegulation { get; set; }
    }
}
