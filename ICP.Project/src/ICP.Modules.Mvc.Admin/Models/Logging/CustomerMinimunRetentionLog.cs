using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.Logging
{
    /// <summary>
    /// 特店 帳戶餘額最低保留款 異動紀錄
    /// </summary>
    public class CustomerMinimunRetentionLog
    {
        /// <summary>
        /// 記錄編號
        /// </summary>
        public long LogID { get; set; }

        /// <summary>
        /// 特店編號
        /// </summary>
        public long CustomerID { get; set; }

        /// <summary>
        /// 原帳戶餘額最低保留款
        /// </summary>
        public decimal OAmount { get; set; }

        /// <summary>
        /// 帳戶餘額最低保留款
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 建立者
        /// </summary>
        public string CreateUser { get; set; }
    }
}
