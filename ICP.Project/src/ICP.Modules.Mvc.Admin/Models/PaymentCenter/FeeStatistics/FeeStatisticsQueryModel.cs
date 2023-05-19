using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Admin.Models.PaymentCenter
{
    /// <summary>
    /// 金流手續費統計(月結)查詢條件輸入
    /// </summary>
    public class FeeStatisticsQueryModel : PageModel
    {
        /// <summary>
        /// 統計方式
        /// </summary>
        [Display(Name = "統計方式")]
        public int StatisticsType { get; set; }

        /// <summary>
        /// 特店查詢方式(電支帳號,特店名稱)
        /// </summary>
        [Display(Name = "特店")]
        public int MerchantQueryType { get; set; }

        /// <summary>
        /// 特店查詢方式帶入值
        /// </summary>
        public string MerchantQueryValue { get; set; }

        /// <summary>
        /// 起始時間
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 結束時間
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 年度
        /// </summary>
        [Display(Name = "年")]
        public int Year { get; set; }

        /// <summary>
        /// 月份
        /// </summary>
        [Display(Name = "月")]
        public int Month { get; set; }

        /// <summary>
        /// 金流方式
        /// </summary>
        [Display(Name = "金流方式")]
        public int TradeModeID { get; set; }

        #region 選單
        public IEnumerable<SelectListItem> MerchantQueryList { get; set; }
        public IEnumerable<SelectListItem> YearList { get; set; }
        public IEnumerable<SelectListItem> MonthList { get; set; }
        public IEnumerable<SelectListItem> TradeModeList { get; set; }
        #endregion
    }
}
