using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.Logging
{
    /// <summary>
    /// 特店多重修改歷程
    /// </summary>
    public class CustomerMutipleLogModel
    {
        /// <summary>
        /// 撥款天數 修改歷程
        /// </summary>
        public List<CustomerAllocateDayLog> CustomerAllocateDayLogs { get; set; }

        /// <summary>
        /// 年費設定 修改歷程
        /// </summary>
        public List<CustomerAnnualFeeLog> CustomerAnnualFeeLogs { get; set; }

        /// <summary>
        /// 帳戶餘額最低保留款 修改歷程
        /// </summary>
        public List<CustomerMinimunRetentionLog> CustomerMinimunRetentionLogs { get; set; }

        /// <summary>
        /// 30 天提領額度限制 修改歷程
        /// </summary>
        public List<CustomerTransferUsedLimitLog> CustomerTransferUsedLimitLogs { get; set; }

        /// <summary>
        /// 負責業務 異動歷程
        /// </summary>
        public List<CustomerSalesLog> CustomerSalesLogs { get; set; }

        /// <summary>
        /// 審核 歷程
        /// </summary>
        public List<CustomerAuditLog> CustomerAuditLogs { get; set; }

        /// <summary>
        /// 記錄 歷程
        /// </summary>
        public List<CustomerMemoLog> CustomerMemoLogs { get; set; }

        /// <summary>
        /// 歸檔編號 異動歷程
        /// </summary>
        public List<CustomerArchivingNoLog> CustomerArchivingNoLogs { get; set; }
    }
}
