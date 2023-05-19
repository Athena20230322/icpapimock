using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.PaymentCenter
{
    /// <summary>
    /// 金流中心統計資訊明細查詢條件
    /// </summary>
    public class TradeStatisticsDetailQueryModel : PageModel
    {
        /// <summary>
        /// 銀行代碼
        /// </summary>
        public string BankCode { get; set; }

        /// <summary>
        /// 付款時間
        /// </summary>
        public DateTime PaymentDate { get; set; }

        /// <summary>
        /// 交易種類
        /// </summary>
        public int QueryType { get; set; }
    }
}
