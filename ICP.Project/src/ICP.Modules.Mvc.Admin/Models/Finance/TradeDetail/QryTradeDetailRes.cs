using ICP.Infrastructure.Core.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Mvc.Admin.Models.Finance.TradeDetail
{
    /// <summary>
    /// 實質交易明細查詢 查詢結果
    /// </summary>
    public class QryTradeDetailRes : BaseListModel
    {
        /// <summary>
        /// 交易類型
        /// </summary>
        [Display(Name = "交易類型")]
        public string TradeStatusName { get; set; }

        /// <summary>
        /// 訂單日期
        /// </summary>
        [Display(Name = "訂單日期")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 付款日期
        /// </summary>
        [Display(Name = "付款日期")]
        public DateTime? PaymentDate { get; set; }

        /// <summary>
        /// 傳輸日期
        /// </summary>
        [Display(Name = "傳輸日期")]
        public DateTime? TransmittalDate { get; set; }

        /// <summary>
        /// 撥款日期
        /// </summary>
        [Display(Name = "撥款日期")]
        public DateTime? AllocateDate { get; set; }

        /// <summary>
        /// 退款日期
        /// </summary>
        [Display(Name = "退款日期")]
        public DateTime? RefundDate { get; set; }

        /// <summary>
        /// 收款方電支帳號
        /// </summary>
        [Display(Name = "收款方電支帳號")]
        public string PayeeICPMID { get; set; }

        /// <summary>
        /// 付款方電支帳號
        /// </summary>
        [Display(Name = "付款方電支帳號")]
        public string PayerICPMID { get; set; }

        /// <summary>
        /// 平台商編號
        /// </summary>
        [Display(Name = "平台商編號")]
        public long PlatformID { get; set; }

        /// <summary>
        /// icashpay訂單編號
        /// </summary>
        [Display(Name = "icashpay訂單編號")]
        public string TradeNo { get; set; }

        /// <summary>
        /// 特店訂單編號
        /// </summary>
        [Display(Name = "特店訂單編號")]
        public string MerchantTradeNo { get; set; }

        /// <summary>
        /// 付款方式
        /// </summary>
        [Display(Name = "付款方式")]
        public string PaymentTypeName { get; set; }

        /// <summary>
        /// 款項來源(銀行)
        /// </summary>
        [Display(Name = "款項來源(銀行)")]
        public string PaymentSource { get; set; }

        /// <summary>
        /// 原始訂單金額
        /// </summary>
        [Display(Name = "原始訂單金額")]
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 信託金額
        /// </summary>
        [Display(Name = "信託金額")]
        public decimal TrustAmt { get; set; }

        /// <summary>
        /// 實際收到金額
        /// </summary>
        [Display(Name = "實際收到金額")]
        public decimal RealAmt { get; set; }

        /// <summary>
        /// 點數折抵金額
        /// </summary>
        [Display(Name = "點數折抵金額")]
        public int BonusAmt { get; set; }

        /// <summary>
        /// 交易手續費(%數/$筆)
        /// </summary>
        [Display(Name = "交易手續費(%數/$筆)")]
        public string ChargeFee { get; set; }

        /// <summary>
        /// 交易手續費金額
        /// </summary>
        [Display(Name = "交易手續費金額")]
        public decimal ChargeFeeAmt { get; set; }

        /// <summary>
        /// 應撥/退款項(淨額)
        /// </summary>
        [Display(Name = "應撥/退款項(淨額)")]
        public decimal AllocateAmt { get; set; }

        /// <summary>
        /// 付款狀態
        /// </summary>
        [Display(Name = "付款狀態")]
        public string PaymentStatusName { get; set; }

        /// <summary>
        /// 撥款狀態
        /// </summary>
        [Display(Name = "撥款狀態")]
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

    }
}
