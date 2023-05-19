using ICP.Infrastructure.Core.Models;
using System;

namespace ICP.Modules.Mvc.Admin.Models.PaymentCenter
{
    /// <summary>
    /// 金流中心統計資訊查詢條件
    /// </summary>
    public class TradeStatisticsQueryModel : PageModel
    {
        /// <summary>
        /// 起始時間
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 結束時間
        /// </summary>
        public DateTime EndDate { get; set; }
    }
}
