using System;
using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Mvc.Payment.Models.QueryTrade
{
    public class TradeInfo
    {
        /// <summary>
        /// 訂單流水號
        /// </summary>
        public long TradeID { get; set; }

        /// <summary>
        /// 訂單建立時間
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 訂單狀態
        /// </summary>
        public int TradeStatus { get; set; }

        /// <summary>
        /// 付款/儲值總金額
        /// </summary>
        [DisplayFormat(DataFormatString = "NT${0:N0}")]
        public int TotalAmount { get; set; }

        /// <summary>
        /// 付款/儲值金額
        /// </summary>
        [DisplayFormat(DataFormatString = "NT${0:N0}")]
        public int Amount { get; set; }

        /// <summary>
        /// 訂單編號
        /// </summary>
        public string TradeNo { get; set; }

        /// <summary>
        /// 付款狀態
        /// </summary>
        public int PaymentStatus { get; set; }

        /// <summary>
        /// 付款日期
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}")]
        public DateTime PaymentDate { get; set; }

        /// <summary>
        /// 銀行代碼
        /// </summary>
        public string BankCode { get; set; }

        /// <summary>
        /// 銀行名稱
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// AccountLink銀行帳號
        /// </summary>
        public string BankAccount { get; set; }

        /// <summary>
        /// AccountLink銀行帳號(顯示用)
        /// </summary>
        public string BankAccountShowName { get; set; }

        /// <summary>
        /// 交易識別碼(條碼)
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 商店編號
        /// </summary>
        public string StoreID { get; set; }

        /// <summary>
        /// 商店名稱
        /// </summary>
        public string StoreName { get; set; }

        /// <summary>
        /// 通路名稱
        /// </summary>
        public string MerchantName { get; set; }

        /// <summary>
        /// 付款方式代碼
        /// </summary>
        public int PaymentTypeID { get; set; }

        /// <summary>
        /// 付款方式子代碼
        /// </summary>
        public int PaymentSubTypeID { get; set; }

        /// <summary>
        /// IcashPay帳戶編號
        /// </summary>
        public string IcashAccount { get; set; }
    }
}