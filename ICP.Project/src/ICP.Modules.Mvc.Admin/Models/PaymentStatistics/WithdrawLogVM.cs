using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Admin.Models.PaymentStatistics
{
    /// <summary>
    /// 提領監控歷程 ViewModel
    /// </summary>
    public class WithdrawLogVM
    {
        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 電支帳號
        /// </summary>
        [Display(Name = "電支帳號")]
        public string ICPMID { get; set; }

        /// <summary>
        /// 商戶名稱/個人名稱
        /// </summary>
        [Display(Name = "商戶名稱/個人名稱")]
        public string MerchantName { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        [Display(Name = "備註")]
        [StringLength(300, ErrorMessage = "請輸入 300字內備註")]
        public string Remark { get; set; }

        /// <summary>
        /// 檢視狀態
        /// </summary>
        /// <remarks>1:觀察中,0:解除觀察</remarks>
        public int Status { get; set; }

        /// <summary>
        /// 歷程清單
        /// </summary>
        public List<ListMonitorLogDbRes> LogList { get; set; }

        /// <summary>
        /// 監控類型
        /// </summary>
        /// <remarks>20:全部, 2:備註, 3:已檢視</remarks>
        public int MonitorType { get; set; } = 2;

        /// <summary>
        /// 監控類型
        /// </summary>
        /// <remarks>20:全部, 2:備註, 3:已檢視</remarks>
        public int SelectType { get; set; } = 20;

        /// <summary>
        /// 監控類型選單
        /// </summary>
        /// <remarks>20:全部, 2:備註, 3:已檢視</remarks>
        public IEnumerable<SelectListItem> SelectTypeList { get; set; }
    }
}
