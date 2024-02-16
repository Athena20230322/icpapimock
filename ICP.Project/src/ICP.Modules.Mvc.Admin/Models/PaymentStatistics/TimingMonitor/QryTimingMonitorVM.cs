using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Models.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.PaymentStatistics.TimingMonitor
{
    public class QryTimingMonitorVM : PageModel
    {
        /// <summary>
        /// 查詢日期
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 愛金卡帳戶
        /// </summary>
        public string ICPMID { get; set; }

        /// <summary>
        /// 是否顯是全部廠商
        /// </summary>
        public bool SelectMode { get; set; }

        /// <summary>
        /// 顯示觀察廠商
        /// </summary>
        public bool MonitorStatus { get; set; }

        /// <summary>
        /// 排序依據
        /// </summary>
        public int SortSet { get; set; }

        /// <summary>
        /// 排序方式
        /// </summary>
        public int SortType { get; set; }

        /// <summary>
        /// 特店名稱
        /// </summary>
        public string MerchantName { get; set; }

        /// <summary>
        /// 1天交易監控比率
        /// </summary>
        public string Day1Waring { get; set; }

        /// <summary>
        /// 10天交易監控比率
        /// </summary>
        public string Day10Waring { get; set; }

        /// <summary>
        /// 30天交易監控比率
        /// </summary>
        public string Day30Waring { get; set; }

        /// <summary>
        /// 1天最低交易總額
        /// </summary>
        public string Day1Amount { get; set; }

        /// <summary>
        /// 10天最低交易總額
        /// </summary>
        public string Day10Amount { get; set; }

        /// <summary>
        /// 30天最低交易總額
        /// </summary>
        public string Day30Amount { get; set; }

        /// <summary>
        /// 提領限制狀態
        /// </summary>
        public bool WithdrawStatus { get; set; }

        /// <summary>
        /// 查詢規則(1:規則一 2:規則二)
        /// </summary>
        public int RuleMode { get; set; }
        
        /// <summary>
        /// 篩選欄位(0:1日, 1:10日, 2:30日, 3:交易退款) (規則二)
        /// </summary>
        public int TradeType { get; set; }
        
        /// <summary>
        /// 查詢模式(1:金額 2:筆數 3:比例) (規則二)
        /// </summary>
        public int TradeMode { get; set; }

        /// <summary>
        /// 金額/筆數/比例(以上) (規則二)
        /// </summary>
        public string TradeContent { get; set; }

        public int TradeSet { get; set; }
    }
}
