using ICP.Infrastructure.Core.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Mvc.Admin.Models.Finance.TradeDetail
{
    /// <summary>
    /// 實質交易明細查詢 查詢結果
    /// </summary>
    public class QryTradeDetailDbRes
    {
        /// <summary>
        /// 交易類型
        /// </summary>
        public string TradeStatusName { get; set; }

        /// <summary>
        /// 訂單日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 付款日期
        /// </summary>
        public DateTime? PaymentDate { get; set; }

        /// <summary>
        /// 傳輸日期
        /// </summary>
        public DateTime? TransmittalDate { get; set; }

        /// <summary>
        /// 撥款日期
        /// </summary>
        public DateTime? AllocateDate { get; set; }

        /// <summary>
        /// 退款日期
        /// </summary>
        public DateTime? RefundDate { get; set; }

        /// <summary>
        /// 收款方電支帳號
        /// </summary>
        public string PayeeICPMID { get; set; }

        /// <summary>
        /// 付款方電支帳號
        /// </summary>
        public string PayerICPMID { get; set; }

        /// <summary>
        /// 平台商編號
        /// </summary>
        public long PlatformID { get; set; }

        /// <summary>
        /// icashpay訂單編號
        /// </summary>
        public string TradeNo { get; set; }

        /// <summary>
        /// 特店訂單編號
        /// </summary>
        public string MerchantTradeNo { get; set; }

        /// <summary>
        /// 付款方式
        /// </summary>
        public string PaymentTypeName { get; set; }

        /// <summary>
        /// 款項來源(銀行)
        /// </summary>
        public string PaymentSource { get; set; }

        /// <summary>
        /// 原始訂單金額
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 信託金額
        /// </summary>
        public decimal TrustAmt { get; set; }

        /// <summary>
        /// 實際收到金額
        /// </summary>
        public decimal RealAmt { get; set; }

        /// <summary>
        /// 點數折抵金額
        /// </summary>
        public int BonusAmt { get; set; }

        /// <summary>
        /// 交易手續費(%數/$筆)
        /// </summary>
        public string ChargeFee { get; set; }

        /// <summary>
        /// 交易手續費金額
        /// </summary>
        public decimal ChargeFeeAmt { get; set; }

        /// <summary>
        /// 應撥/退款項(淨額)
        /// </summary>
        public decimal AllocateAmt { get; set; }

        /// <summary>
        /// 付款狀態
        /// </summary>
        public string PaymentStatusName { get; set; }

        /// <summary>
        /// 撥款狀態
        /// </summary>
        public string AllocateStatusName { get; set; }

        /// <summary>
        /// 總原始訂單金額
        /// </summary>
        public decimal SumTotalAmt { get; set; }

        /// <summary>
        /// 總信託金額
        /// </summary>
        public decimal SumTrustAmt { get; set; }

        /// <summary>
        /// 總實際收到金額
        /// </summary>
        public decimal SumRealAmt { get; set; }

        /// <summary>
        /// 總點數折抵金額
        /// </summary>
        public int SumBonusAmt { get; set; }

        /// <summary>
        /// 總交易手續費
        /// </summary>
        public decimal SumChargeFeeAmt { get; set; }

        /// <summary>
        /// 總應撥/退款項
        /// </summary>
        public decimal SumAllocateAmt { get; set; }

        /// <summary>
        /// 總筆數
        /// </summary>
        public int TotalCount { get; set; }

    }
}
