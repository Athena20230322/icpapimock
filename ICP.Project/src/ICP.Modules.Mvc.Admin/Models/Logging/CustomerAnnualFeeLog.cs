using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.Logging
{
    /// <summary>
    /// 特店 年費設定 修改歷程
    /// </summary>
    public class CustomerAnnualFeeLog
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
        /// 年費
        /// </summary>
        public decimal AnnualFee { get; set; }

        /// <summary>
        /// 年費合約啟始日
        /// </summary>
        public DateTime AnnualFeeStartDate { get; set; }

        /// <summary>
        /// 年費合約結束日
        /// </summary>
        public DateTime AnnualFeeEndDate { get; set; }

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