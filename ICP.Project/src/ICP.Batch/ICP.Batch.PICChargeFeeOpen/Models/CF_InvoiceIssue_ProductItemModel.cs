using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.PICChargeFeeOpen.Models
{
    /// <summary>
    /// 手續費電子發票明細檔			
    /// ChargeFee_InvoiceIssue_ProductItemModel
    /// </summary>
    public class CF_InvoiceIssue_ProductItemModel
    {
        /// <summary>
        /// 發票ID
        /// </summary>
        public long InvoiceID { get; set; }

        /// <summary>
        /// 明細排列序號
        /// </summary>
        public int SequenceNumber { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 數量
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// 單位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 單價
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 金額
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 營業稅額
        /// </summary>
        public int Tax { get; set; }

        /// <summary>
        /// 單一欄位備註
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 相關號碼
        /// </summary>
        public string RelateNumber { get; set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}
