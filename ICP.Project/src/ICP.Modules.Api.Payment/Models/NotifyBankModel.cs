using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Payment.Models
{
    public class NotifyBankModel
    {
        /// <summary>
        /// 虛擬帳號
        /// </summary>
        public string VirtualAccount { get; set; }

        /// <summary>
        /// 通知銀行狀態 → 1:通知申請成功 2:通知申請失敗 3:通知取消成功 4:通知取消失敗
        /// </summary>
        public int NotifyBankStatus { get; set; }

        /// <summary>
        /// 通知銀行時間
        /// </summary>
        public DateTime NotifyBankDateTime { get; set; }
    }
}
