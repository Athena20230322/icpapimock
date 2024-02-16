using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.PaymentCenter.Models
{
    public class AtmNotifyModel
    {
        /// <summary>
        /// 訂單編號，須是唯一值
        /// </summary>
        public string TradeNo { get; set; }

        /// <summary>
        /// 交易金額
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 虛擬帳號
        /// </summary>
        public string VirtualAccount { get; set; }

        /// <summary>
        /// 繳款期限
        /// </summary>
        public DateTime ExpireDate { get; set; }

        /// <summary>
        /// 申請類型
        /// </summary>
        public string ApplyType { get; set; }
    }
}
