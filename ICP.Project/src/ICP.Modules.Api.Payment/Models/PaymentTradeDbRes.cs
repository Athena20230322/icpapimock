using ICP.Infrastructure.Core.ExtraAttribute;
using ICP.Infrastructure.Core.Models;
using Newtonsoft.Json;
using System;

namespace ICP.Modules.Api.Payment.Models
{
    public class PaymentTradeDbRes : BaseResult
    {
        /// <summary>
        /// 交易流水號
        /// </summary>
        public long TradeID { get; set; }

        /// <summary>
        /// 訂單編號
        /// </summary>
        public string TradeNo { get; set; }

        /// <summary>
        /// 付款方式代碼
        /// </summary>
        public int PaymentTypeID { get; set; }

        /// <summary>
        /// 收單行名稱代碼
        /// </summary>
        public int PaymentSubTypeID { get; set; }

        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 廠商編號
        /// </summary>
        public long MerchantID { get; set; }

        /// <summary>
        /// 平台商編號
        /// </summary>
        public long PlatformID { get; set; }

        /// <summary>
        /// 交易編號
        /// </summary>
        public string MerchantTradeNo { get; set; }

        /// <summary>
        /// 交易日期
        /// </summary>
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? MerchantTradeDate { get; set; }

        /// <summary>
        /// 交易狀態 0 : 訂單成立 1 : 交易結束(含履約完成) 2 : 全額退款 3 : 部分退款 4 : 保留不使用 5 : 通知廠商出貨 6 : 廠商已出貨 7 : 履約交易保證中 8 : 交易履約完成  9 : 已通知請款 10 : 換貨  11 : 退貨
        /// </summary>
        public int TradeStatus { get; set; }

        /// <summary>
        /// 付款狀態 1: 已付款
        /// </summary>
        public int PaymentStatus { get; set; }

        /// <summary>
        /// 付款日期
        /// </summary>
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? PaymentDate { get; set; }

        /// <summary>
        /// 撥款狀態
        /// </summary>
        public int AllocateStatus { get; set; }

        /// <summary>
        /// 撥款日期
        /// </summary>
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? AllocateDate { get; set; }

        /// <summary>
        /// 交易總金額
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 紅利折抵金額
        /// </summary>
        public decimal BonusAmt { get; set; }

        /// <summary>
        /// 退款金額
        /// </summary>
        public decimal RefundAMT { get; set; }

        /// <summary>
        /// 退款時間 (yyyyMMddHHmmss)
        /// </summary>
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime RefundDate { get; set; }

        /// <summary>
        /// 傳輸日期(for超商行動支付對帳用)
        /// </summary>
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? TransmittalDate { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 付款方式ID
        /// </summary>
        public string PayID { get; set; }

        /// <summary>
        /// 愛金卡帳戶
        /// </summary>
        public string IcashAccount { get; set; }

        /// <summary>
        /// 交易模式(交易:1 儲值:2 轉帳:3 提領:4)
        /// </summary>
        public int TradeModeID { get; set; }

        /// <summary>
        /// 支付種類(銀行代碼)
        /// </summary>
        public string BankNo { get; set; }

        /// <summary>
        /// 支付名稱(銀行名稱)
        /// </summary>
        public string BankName { get; set; }

    }
}
