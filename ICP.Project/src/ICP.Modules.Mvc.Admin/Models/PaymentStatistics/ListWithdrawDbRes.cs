using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;

namespace ICP.Modules.Mvc.Admin.Models.PaymentStatistics
{
    /// <summary>
    /// 每日提領金額監控清單Res
    /// </summary>
    public class ListWithdrawDbRes : BaseListModel
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
        /// 交易觀察名單狀態
        /// </summary>
        /// <remarks>1:觀察中,0:解除觀察</remarks>
        public int MonitorStatus { get; set; }

        /// <summary>
        /// 商戶/個人名稱
        /// </summary>
        public string CName { get; set; }

        /// <summary>
        /// 商店名稱
        /// </summary>
        public string MerchantName { get; set; }

        /// <summary>
        /// 註冊時間
        /// </summary>
        public DateTime RegisterDate { get; set; }

        /// <summary>
        /// 負責業務
        /// </summary>
        public string SalesContactPerson { get; set; }

        /// <summary>
        /// MCC Code
        /// </summary>
        public int MCCCode { get; set; }

        /// <summary>
        /// 商品類別
        /// </summary>
        public int CommoditiyType { get; set; }

        /// <summary>
        /// 商品類別
        /// </summary>
        public string CommoditiyTypeName
        {
            get
            {
                switch (CommoditiyType)
                {
                    case 1:
                        return "實體";
                    case 2:
                        return "虛擬";
                    case 3:
                        return "實體|虛擬";
                    case 4:
                        return "遞延";
                    case 5:
                        return "實體|遞延";
                    case 6:
                        return "虛擬|遞延";
                    case 7:
                        return "實體|虛擬|遞延";
                    default:
                        return "";
                }
            }
        }

        /// <summary>
        /// 提領設定
        /// </summary>
        /// <remarks>0:全部 1:手動 2:自動</remarks>
        public int TransferType { get; set; }

        /// <summary>
        /// 提領日期
        /// </summary>
        public DateTime LastWithdrawDay { get; set; }

        /// <summary>
        /// 選擇日期金額
        /// </summary>
        public int SelectDayAmount { get; set; }

        /// <summary>
        /// 選擇日期筆數
        /// </summary>
        public int SelectDayCount { get; set; }

        /// <summary>
        /// 選擇日前1天
        /// </summary>
        public int Day1 { get; set; }

        /// <summary>
        /// 選擇日前2天
        /// </summary>
        public int Day2 { get; set; }

        /// <summary>
        /// 選擇日前3天
        /// </summary>
        public int Day3 { get; set; }

        /// <summary>
        /// 選擇日前4天
        /// </summary>
        public int Day4 { get; set; }

        /// <summary>
        /// 選擇日前5天
        /// </summary>
        public int Day5 { get; set; }

        /// <summary>
        /// 選擇日前6天
        /// </summary>
        public int Day6 { get; set; }

        /// <summary>
        /// 30天累計提領金額
        /// </summary>
        public int WithdrawDay30Amount { get; set; }

        /// <summary>
        /// 30天累計提領筆數
        /// </summary>
        public int WithdrawDay30Count { get; set; }

        /// <summary>
        /// 檢視狀態
        /// </summary>
        /// <remarks>1:觀察中,0:解除觀察</remarks>
        public int InspectStatus { get; set; }

        /// <summary>
        /// 檢視日期
        /// </summary>
        public DateTime InspectDate { get; set; }

        /// <summary>
        /// 提領排程清單
        /// </summary>
        public List<BankTransferScheduleDbRes> ListBankTransferSchedule = new List<BankTransferScheduleDbRes>();
    }

    /// <summary>
    /// 提領排程清單Res
    /// </summary>
    public class BankTransferScheduleDbRes
    {
        /// <summary>
        /// 排程種類
        /// </summary>
        /// <remarks>1:每日 2:每周 3:隔週 4:每月</remarks>
        public int ScheduleType { get; set; }

        /// <summary>
        /// 排程細項
        /// </summary>
        /// <remarks>日:0, 周: 1234567 月: 1,5,10,15,20,25,0 自訂: 1-30</remarks>
        public int ScheduleValue { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        /// <remarks>1: 啟用, 2: 停用</remarks>
        public int Status { get; set; }

        public string ScheduleTypeName
        {
            get
            {
                switch (ScheduleType)
                {
                    case 2:

                        if (ScheduleValue == 7)
                        {
                            return "每週日提領";
                        }

                        return "每週" + ScheduleValue + "提領";
                    case 3:

                        if (ScheduleValue == 7)
                        {
                            return "隔週日提領";
                        }

                        return "隔週" + ScheduleValue + "提領";
                    case 4:
                        return "每月" + ScheduleValue + "日提領";
                    default:
                        return "每日提領";
                }
            }
        }
    }
}
