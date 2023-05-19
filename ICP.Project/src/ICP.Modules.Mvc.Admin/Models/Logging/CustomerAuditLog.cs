using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.Logging
{
    /// <summary>
    /// 特店 審核歷程
    /// </summary>
    public class CustomerAuditLog
    {
        /// <summary>
        /// 流水號
        /// </summary>
        public long LogID { get; set; }

        /// <summary>
        /// 特店代號
        /// </summary>
        public long CustomerID { get; set; }

        /// <summary>
        /// 特店過件狀態
        /// 0: 未過件 1: 已過件
        /// </summary>
        public byte CustomerStatus { get; set; }

        /// <summary>
        /// 審核階段
        /// </summary>
        public byte AuditStatus { get; set; }

        /// <summary>
        /// 審核備註
        /// </summary>
        public string AuditNote { get; set; }

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
