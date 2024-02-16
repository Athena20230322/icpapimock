using ICP.Infrastructure.Core.Models;
using System;

namespace ICP.Modules.Mvc.Admin.Models.PaymentStatistics.PaymentMonitor
{
    /// <summary>
    /// 每日付款交易金額監控 DB請求
    /// </summary>
    public class QryPaymentMonitorDbReq
    {
        /// <summary>
        /// 查詢日期
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 電支帳號
        /// </summary>
        public string ICPMID { get; set; }

        /// <summary>
        /// 商戶名稱
        /// </summary>
        public string MerchantName { get; set; }

        /// <summary>
        /// 收款觀察名單(定時監控)
        /// </summary>
        /// <remarks>0:否 1:是</remarks>
        public bool IncomeStaus { get; set; }

        /// <summary>
        /// 付款觀察名單
        /// </summary>
        /// <remarks>0:否 1:是</remarks>
        public bool PaymentStatus { get; set; }

        /// <summary>
        /// 金流類型
        /// </summary>
        /// <remarks>1:帳戶餘額 2:連結銀行帳號 3:1天總付款額 4:10天總付款額 5:30天總付款額 6:10天帳戶餘額付款 7:30天帳戶餘額付款 8:10天儲值總額</remarks>
        public int TradeType { get; set; }

        /// <summary>
        /// 商戶類型
        /// </summary>
        /// <remarks>0:全選 1:個人 2:法人</remarks>
        public int MerchantType { get; set; }

        /// <summary>
        /// 金額區間-金額
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// 金額區間-筆數
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 排序方式
        /// </summary>
        /// <remarks>1:帳戶餘額 2:連結銀行帳號 3:1天總付款額 4:10天總付款額 5:30天總付款額 6:10天帳戶餘額付款 7:30天帳戶餘額付款 8:10天儲值總額</remarks>
        public int SortType { get; set; }

        /// <summary>
        /// 排序方式
        /// </summary>
        /// <remarks>1:金額 2:筆數</remarks>
        public int SortKind { get; set; }

        /// <summary>
        /// 目前頁數
        /// </summary>
        public int PageNo { get; set; }

        /// <summary>
        /// 每頁顯示筆數
        /// </summary>
        /// <remarks>NULL:全部</remarks>
        public int PageSize { get; set; }

    }
}
