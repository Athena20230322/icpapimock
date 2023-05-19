using ICP.Infrastructure.Core.Models;
using System;

namespace ICP.Modules.Mvc.Admin.Models.PaymentStatistics.PaymentMonitor
{
    /// <summary>
    /// 每日付款交易金額監控 DB回應
    /// </summary>
    public class QryPaymentMonitorDbRes
    {
        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 電支帳號
        /// </summary>
        public string ICPMID { get; set; }

        /// <summary>
        /// 收款觀察名單
        /// </summary>
        /// <remarks>0:否 1:是</remarks>
        public bool IncomeStaus { get; set; }

        /// <summary>
        /// 付款觀察名單
        /// </summary>
        /// <remarks>0:否 1:是</remarks>
        public bool PaymentStatus { get; set; }

        /// <summary>
        /// 商戶名稱
        /// </summary>
        public string MerchantName { get; set; }

        /// <summary>
        /// 商店名稱
        /// </summary>
        public string WebSiteName { get; set; }

        /// <summary>
        /// 商店網址
        /// </summary>
        public string WebSiteURL { get; set; }

        /// <summary>
        /// 註冊時間
        /// </summary>
        public DateTime RegDate { get; set; }

        /// <summary>
        /// MCC Code
        /// </summary>
        public int MCCCode { get; set; }

        /// <summary>
        /// 商品類別
        /// </summary>
        public string CommoditiyTypeName { get; set; }

        /// <summary>
        /// 商戶類型
        /// </summary>
        /// <remarks>個人/法人</remarks>
        public string MerchantTypeName { get; set; }

        /// <summary>
        /// 帳戶餘額
        /// </summary>
        public int ICashAmt { get; set; }

        /// <summary>
        /// 帳戶餘額筆數
        /// </summary>
        public int ICashCount { get; set; }

        /// <summary>
        /// 連結銀行帳戶
        /// </summary>
        public int ACLinkAmt { get; set; }

        /// <summary>
        /// 連結銀行帳戶筆數
        /// </summary>
        public int ACLinkCount { get; set; }

        /// <summary>
        /// 1天總付款額
        /// </summary>
        public int Total1DayAmt { get; set; }

        /// <summary>
        /// 1天總付款額筆數
        /// </summary>
        public int Total1DayCount { get; set; }

        /// <summary>
        /// 10天總付款額
        /// </summary>
        public int Total10DaysAmt { get; set; }

        /// <summary>
        /// 10天總付款額筆數
        /// </summary>
        public int Total10DaysCount { get; set; }

        /// <summary>
        /// 30天總付款額
        /// </summary>
        public int Total30DaysAmt { get; set; }

        /// <summary>
        /// 30天總付款額筆數
        /// </summary>
        public int Total30DaysCount { get; set; }

        /// <summary>
        /// 10天帳戶餘額付款
        /// </summary>
        public int ICash10DaysAmt { get; set; }

        /// <summary>
        /// 10天帳戶餘額付款筆數
        /// </summary>
        public int ICash10DaysCount { get; set; }

        /// <summary>
        /// 30天帳戶餘額付款
        /// </summary>
        public int ICash30DaysAmt { get; set; }

        /// <summary>
        /// 30天帳戶餘額付款筆數
        /// </summary>
        public int ICash30DaysCount { get; set; }

        /// <summary>
        /// 10天儲值總額
        /// </summary>
        public int ACLink10DaysAmt { get; set; }

        /// <summary>
        /// 10天儲值總額筆數
        /// </summary>
        public int ACLink10DaysCount { get; set; }

        /// <summary>
        /// 是否有備註記錄
        /// </summary>
        /// <remarks>0:沒有 1:有</remarks>
        public bool InspectStatus { get; set; }

        /// <summary>
        /// 檢視記錄日期
        /// </summary>
        /// <remarks>無值:未檢視</remarks>
        public DateTime? InspectDate { get; set; }

        /// <summary>
        /// 會員等級
        /// </summary>
        /// <remarks>(31:特店)前端判斷電支帳號連結使用</remarks>
        public int LevelID { get; set; }

        /// <summary>
        /// 特店編號 
        /// </summary>
        /// <remarks>前端判斷電支帳號連結使用</remarks>
        public int CustomerID { get; set; }

        /// <summary>
        /// 總筆數
        /// </summary>
        public int TotalCount { get; set; }

    }
}
