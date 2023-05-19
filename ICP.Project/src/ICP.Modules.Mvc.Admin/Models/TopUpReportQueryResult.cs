using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models
{
    public class TopUpReportQueryResult : BaseListModel
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
        /// 收款日期(誤)
        /// </summary>
        public DateTime PaymentDate { get; set; }

        /// <summary>
        /// 傳輸日期
        /// </summary>
        public DateTime? TransmittalDate { get; set; }

        /// <summary>
        /// 繳費期限
        /// </summary>
        public DateTime? ExpireDate { get; set; }

        /// <summary>
        /// 電支帳號
        /// </summary>
        public string ICPMID { get; set; }

        /// <summary>
        /// iCash 訂單編號
        /// </summary>
        public string TradeNo { get; set; }

        /// <summary>
        /// 儲值金額
        /// </summary>
        public decimal TopUpAmount { get; set; }

        /// <summary>
        /// 實收金額：實際收到的金額
        /// </summary>
        public decimal RealReceiveAmount { get; set; }

        /// <summary>
        /// 儲值方式
        /// </summary>
        public string TopUpTypeMeaning { get; set; }

        /// <summary>
        /// 款項來源(銀行/超商)
        /// </summary>
        public string TopUpTypeSource { get; set; }

        /// <summary>
        /// 銀行連結帳號/虛擬帳號
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 超商店號/特店店號
        /// </summary>
        public string StoreID { get; set; }

        /// <summary>
        /// 銀行代碼
        /// </summary>
        public string BankCode { get; set; }

        /// <summary>
        /// 銀行轉帳轉出帳號
        /// </summary>
        public string BankAccNo { get; set; }

        /// <summary>
        /// 交易服務費
        /// </summary>
        public decimal TradeServiceRate { get; set; }

        /// <summary>
        /// 交易服務費金額
        /// </summary>
        public decimal TradeServiceAmount { get; set; }

        /// <summary>
        /// 應收款項(淨額)
        /// </summary>
        public decimal NetAmount { get; set; }

        /// <summary>
        /// 撥款(儲值)狀態
        /// </summary>
        public string TopUpStatusMeaning { get; set; }

        /// <summary>
        /// 儲值金額總額
        /// </summary>
        public decimal TopUpAmountSum { get; set; }

        /// <summary>
        /// 實收金額總額：實際收到的金額總額
        /// </summary>
        public decimal RealReceiveAmountSum { get; set; }

        /// <summary>
        /// 交易服務費金額總額
        /// </summary>
        public decimal TradeServiceAmountSum { get; set; }

        /// <summary>
        /// 應收款項(淨額)總額
        /// </summary>
        public decimal NetAmountSum { get; set; }
    }
}
