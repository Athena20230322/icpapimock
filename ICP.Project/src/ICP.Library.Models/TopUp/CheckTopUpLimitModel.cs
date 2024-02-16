using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.TopUp
{
    public class CheckTopUpLimitModel
    {
        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 交易記錄流水號
        /// </summary>
        public long TradeID { get; set; }

        /// <summary>
        /// 付款方式代碼
        /// </summary>
        public int PaymentTypeID { get; set; }

        /// <summary>
        /// 付款方式子類別代碼
        /// </summary>
        public int PaymentSubTypeID { get; set; }

        /// <summary>
        /// 交易金額
        /// </summary>
        public int Amount { get; set; }
    }
}
