using ICP.Infrastructure.Core.Models;
using System;

namespace ICP.Modules.Mvc.Admin.Models.PaymentStatistics.TimingMonitor
{
    public class TimingMonitorRes : BaseListModel
    {
        public long MID { get; set; }

        /// <summary>
        /// 電支帳戶
        /// </summary>
        public string ICPMID { get; set; }

        /// <summary>
        /// 商戶名稱/個人名稱
        /// </summary>
        public string MerchantName { get; set; }

        /// <summary>
        /// 網站名稱
        /// </summary>
        public string WebSiteName { get; set; }

        /// <summary>
        /// 註冊時間
        /// </summary>
        public DateTime RegisterDate { get; set; }

        /// <summary>
        /// MCC Code
        /// </summary>
        public string MCCCode { get; set; }

        /// <summary>
        /// 觀察狀態
        /// </summary>
        public int MonitorStatus { get; set; }

        /// <summary>
        /// 提領狀態
        /// </summary>
        public int WithdrawStatus { get; set; }

        /// <summary>
        /// 選擇日期總金額
        /// </summary>
        public decimal Day1Amount { get; set; }

        /// <summary>
        /// 選擇日期筆數
        /// </summary>
        public int Day1Count { get; set; }

        /// <summary>
        /// 前10日總金額
        /// </summary>
        public decimal Day2To11Amount { get; set; }

        /// <summary>
        /// 選擇日與前10天平均額
        /// </summary>
        public double Day1Percent { get; set; }

        /// <summary>
        /// 連續10日交易金額
        /// </summary>
        public decimal Day1To10Amount { get; set; }

        /// <summary>
        /// 過去30天總金額
        /// </summary>
        public decimal Day11To40Amount { get; set; }

        /// <summary>
        /// 連續十天交易金額與過去30天總金額的平均額
        /// </summary>
        public double Day10Percent { get; set; }

        /// <summary>
        /// 連續30天交易金額
        /// </summary>
        public decimal Day1To30Amount { get; set; }

        /// <summary>
        /// 過去90天交易金額
        /// </summary>
        public decimal Day31To120Amount { get; set; }

        /// <summary>
        /// 連續30天交易金額雨過去90天交易金額的平均額
        /// </summary>
        public double Day30Percent { get; set; }

        /// <summary>
        /// 檢視狀態
        /// </summary>
        public int InspectStatus { get; set; }

        /// <summary>
        /// 檢視日期
        /// </summary>
        public DateTime? InspectDate { get; set; }

        /// <summary>
        /// 歷程修改日期
        /// </summary>
        public DateTime? ModifyDate { get; set; }
        
        /// <summary>
        /// 前七日退款金額
        /// </summary>
        public int RefundDay1To7Amount { get; set; }
        
        /// <summary>
        /// 前七日退刷筆數
        /// </summary>
        public int RefundDay7Count { get; set; }

        /// <summary>
        /// 商戶網址
        /// </summary>
        public string WebSiteURL { get; set; }

        /// <summary>
        /// 商戶/會員等級
        /// </summary>
        public int LevelID { get; set; }

    }
}