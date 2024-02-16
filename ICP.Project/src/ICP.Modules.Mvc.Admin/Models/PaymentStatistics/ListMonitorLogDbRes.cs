using System;

namespace ICP.Modules.Mvc.Admin.Models.PaymentStatistics
{
    /// <summary>
    /// 歷程清單Res
    /// </summary>
    public class ListMonitorLogDbRes
    {
        /// <summary>
        /// 廠商/會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 監控類型
        /// </summary>
        public int MonitorType { get; set; }

        /// <summary>
        /// 檢視狀態
        /// </summary>
        /// <remarks>1:觀察中,0:解除觀察</remarks>
        public int Status { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 備註人員
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}
