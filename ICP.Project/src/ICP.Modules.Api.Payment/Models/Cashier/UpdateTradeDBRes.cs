using ICP.Infrastructure.Core.ExtraAttribute;
using ICP.Infrastructure.Core.Models;
using Newtonsoft.Json;
using System;

namespace ICP.Modules.Api.Payment.Models.Cashier
{
    public class UpdateTradeDBRes : BaseResult
    {
        /// <summary>
        /// 付款人會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 廠商編號
        /// </summary>
        public long MerchantID { get; set; }

        /// <summary>
        /// 交易金額
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 平臺商編號
        /// </summary>
        public long PlatformID { get; set; }

        /// <summary>
        /// 交易類型(EC:1 POS:2)
        /// </summary>
        public int TradeType { get; set; }

        /// <summary>
        /// 交易模式(交易:1 儲值:2 轉帳:3 提領:4)
        /// </summary>
        public int TradeModeID { get; set; }

        // <summary>
        /// 付款方式代碼
        /// </summary>
        public int PaymentTypeID { get; set; }

        /// <summary>
        /// 收單行名稱代碼
        /// </summary>
        public int PaymentSubTypeID { get; set; }

        /// <summary>
        /// 愛金卡交易序號
        /// </summary>
        public string TransactionID { get; set; }

        /// <summary>
        /// 訂單流水號
        /// </summary>
        public long TradeID { get; set; }

        /// <summary>
        /// 交易編號
        /// </summary>
        public string MerchantTradeNo { get; set; }

        /// <summary>
        /// 付款時間
        /// </summary>
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? PaymentDate { get; set; }

        /// <summary>
        /// 付款方式ID
        /// </summary>
        public string PayID { get; set; }

        /// <summary>
        /// 銀行代碼
        /// </summary>
        public string BankCode { get; set; }

        /// <summary>
        /// 銀行名稱
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// 銀行簡稱
        /// </summary>
        public string BankShortName { get; set; }

        /// <summary>
        /// app 顯示名稱
        /// </summary>
        public string BankAppName { get; set; }

        /// <summary>
        /// 銀行的中文簡稱
        /// </summary>
        public string DisplayShortNameTW { get; set; }

        /// <summary>
        /// 銀行的中文簡稱
        /// </summary>
        public string DisplayShortNameEN { get; set; }

        /// <summary>
        /// 愛金卡帳戶
        /// </summary>
        public string IcashAccount { get; set; }

        /// <summary>
        /// ATM轉帳截止日期
        /// </summary>
        public string ATMExpireDate { get; set; }

        /// <summary>
        /// ATM虛擬帳號
        /// </summary>
        public string VirtualAccount { get; set; }
    }
}
