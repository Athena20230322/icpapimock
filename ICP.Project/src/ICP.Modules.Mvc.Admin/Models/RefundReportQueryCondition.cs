using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models
{
    public class RefundReportQueryCondition : PageModel
    {
        /// <summary>
        /// 日期類型 → 1:訂單日期 2:付款日期 3:退款日期
        /// </summary>
        public int DateType { get; set; }

        /// <summary>
        /// 起始日期
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 結束日期
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 繳費方式  0:全部 1:icash Pay 帳戶 2:連結扣款帳戶
        /// </summary>
        public int PaymentType { get; set; }

        /// <summary>
        /// 付款方資料類型 1:付款方電支帳號 2:付款方名稱
        /// </summary>
        public int PaymentSideDataType { get; set; }

        /// <summary>
        /// 付款方資料內容
        /// </summary>
        public string PaymentSideDataContent { get; set; }

        /// <summary>
        /// 撥款狀態 0:全部 1:已撥款 2:未撥款
        /// </summary>
        public int AllocateStatus { get; set; }

        /// <summary>
        /// 收款方資料類型 1:收款方電支帳號 2:收款方名稱
        /// </summary>
        public int ReceiptSideDataType { get; set; }

        /// <summary>
        /// 收款方資料內容
        /// </summary>
        public string ReceiptSideDataContent { get; set; }

        /// <summary>
        /// icash Pay 訂單編號
        /// </summary>
        public string TradeNo { get; set; }
    }
}
