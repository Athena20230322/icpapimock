using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Admin.Models.PaymentStatistics.PaymentMonitor
{
    /// <summary>
    /// 每日付款交易金額監控(備註歷程) 查詢條件
    /// </summary>
    public class QryPaymentMonitorLogReq
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
        /// 觀察名單
        /// </summary>
        /// <remarks>0:關閉 1:開啟</remarks>
        public int Status { get; set; } = 1;

        /// <summary>
        /// 監控類型選單
        /// </summary>
        public IEnumerable<SelectListItem> MonitorTypeList { get; set; }

        /// <summary>
        /// 監控類型
        /// </summary>
        /// <remarks>6:觀察名單狀態 7:已檢視</remarks>
        public int MonitorType { get; set; } = 6;

        /// <summary>
        /// 歷程清單
        /// </summary>
        public List<ListMonitorLogDbRes> LogList { get; set; }
    }
}
