using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.PaymentCenter
{
    /// <summary>
    /// 金流手續費統計資訊明細
    /// </summary>
    public class FeeStatisticsDetailQueryModel : PageModel
    {
        /// <summary>
        /// 統計方式
        /// </summary>
        public int StatisticsType { get; set; }

        /// <summary>
        /// 撥款時間
        /// </summary>
        public DateTime AllocateDate { get; set; }

        /// <summary>
        /// 起始時間
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 結束時間
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 電支帳號
        /// </summary>
        public long MerchantID { get; set; }

        /// <summary>
        /// 金流方式
        /// </summary>
        public int TradeModeID { get; set; }
    }
}
