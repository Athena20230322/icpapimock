using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.PaymentCenter
{
    /// <summary>
    /// 金流中心統計資訊明細
    /// </summary>
    public class TradeStatisticsDetailModel : BaseListModel
    {
        /// <summary>
        /// 電支帳號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 筆數
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 金額
        /// </summary>
        public decimal Amount { get; set; }
    }
}
