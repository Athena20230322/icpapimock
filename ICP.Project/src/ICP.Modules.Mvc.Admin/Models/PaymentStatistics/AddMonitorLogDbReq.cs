namespace ICP.Modules.Mvc.Admin.Models.PaymentStatistics
{
    /// <summary>
    /// 新增歷程清單Req
    /// </summary>
    public class AddMonitorLogDbReq
    {
        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

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
        /// 監控類型
        /// </summary>
        public int MonitorType { get; set; }
    }
}
