using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models
{
    public class RefundReportQueryResult : BaseListModel
    {
        /// <summary>
        /// 列序號
        /// </summary>
        public int RowNum { get; set; }

        /// <summary>
        /// 訂單日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 付款日期
        /// </summary>
        public DateTime PaymentDate { get; set; }

        /// <summary>
        /// icashpay 訂單編號
        /// </summary>
        public string TradeNo { get; set; }

        /// <summary>
        /// 付款方電支帳號
        /// </summary>
        public string PaymentSideICPMID { get; set; }

        /// <summary>
        /// 付款方名稱
        /// </summary>
        public string PaymentSideName { get; set; }

        /// <summary>
        /// 收款方電支帳號
        /// </summary>
        public string ReceiptSideICPMID { get; set; }

        /// <summary>
        /// 收款方名稱
        /// </summary>
        public string ReceiptSideName { get; set; }

        /// <summary>
        /// 收款方統一編號
        /// </summary>
        public string ReceiptSideUnifiedBusinessNo { get; set; }

        /// <summary>
        /// 繳費方式
        /// </summary>
        public string PaymentTypeMeaning { get; set; }

        /// <summary>
        /// 撥款狀態
        /// </summary>
        public string AllocateStatusMeaning { get; set; }

        /// <summary>
        /// 原始訂單金額
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 實際收到金額
        /// </summary>
        public decimal RealAmount { get; set; }

        /// <summary>
        /// 金流手續費
        /// </summary>
        public decimal GoldFlowChargeFee { get; set; }

        /// <summary>
        /// 應撥款項
        /// </summary>
        public decimal ShouldAllocateAmount { get; set; }

        /// <summary>
        /// 退款日期
        /// </summary>
        public DateTime RefundDate { get; set; }

        /// <summary>
        /// 退款金額
        /// </summary>
        public decimal RefundAMT { get; set; }

        /// <summary>
        /// 返還手續費
        /// </summary>
        public decimal BackChargeFee { get; set; }

        /// <summary>
        /// 原始訂單金額總額
        /// </summary>
        public decimal AmountSum { get; set; }

        /// <summary>
        /// 實際收到金額總額
        /// </summary>
        public decimal RealAmountSum { get; set; }

        /// <summary>
        /// 金流手續費總額
        /// </summary>
        public decimal GoldFlowChargeFeeSum { get; set; }

        /// <summary>
        /// 應撥款項總額
        /// </summary>
        public decimal ShouldAllocateAmountSum { get; set; }

        /// <summary>
        /// 退款金額總額
        /// </summary>
        public decimal RefundAMTSum { get; set; }

        /// <summary>
        /// 返還手續費總額
        /// </summary>
        public decimal BackChargeFeeSum { get; set; }
    }
}
