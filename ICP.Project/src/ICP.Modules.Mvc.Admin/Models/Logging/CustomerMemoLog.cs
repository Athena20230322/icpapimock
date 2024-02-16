using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.Logging
{
    /// <summary>
    /// 特店 記錄歷程
    /// </summary>
    public class CustomerMemoLog
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
        /// 記錄內容
        /// </summary>
        public string MemoNote { get; set; }

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
