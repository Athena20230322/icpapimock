using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Payment.Models.QueryTrade
{
    public class TopUpATMTradeDbRes
    {
        /// <summary>
        /// 訂單流水號
        /// </summary>
        public long TradeID { get; set; }

        /// <summary>
        /// 訂單建立時間
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 訂單狀態
        /// </summary>
        public int TradeStatus { get; set; }

        /// <summary>
        /// 儲值金額
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// 訂單編號
        /// </summary>
        public string TradeNo { get; set; }

        /// <summary>
        /// 銀行代號
        /// </summary>
        public string BankCode { get; set; }

        /// <summary>
        /// 銀行名稱
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// 虛擬帳號
        /// </summary>
        public string VirtualAccount { get; set; }

        /// <summary>
        /// 虛擬帳號有效時間
        /// </summary>
        public string ExpireDate { get; set; }

        /// <summary>
        /// 付款狀態
        /// </summary>
        public int PaymentStatus { get; set; }

        /// <summary>
        /// 付款日期
        /// </summary>
        public DateTime PaymentDate { get; set; }
    }
}
