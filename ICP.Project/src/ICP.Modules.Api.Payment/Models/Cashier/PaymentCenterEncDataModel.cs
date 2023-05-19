using ICP.Infrastructure.Core.ExtraAttribute;
using Newtonsoft.Json;
using System;

namespace ICP.Modules.Api.Payment.Models.Cashier
{
    public class PaymentCenterEncDataModel
    {
        /// <summary>
        /// AccountLink驗證回傳Code
        /// </summary>
        public long? VerifyRtnCode { get; set; }

        /// <summary>
        /// AccountLink驗證回傳Msg
        /// </summary>
        public string VerifyRtnMsg { get; set; }

        /// <summary>
        /// AccountLink回傳虛擬帳號
        /// </summary>
        public string VirtualAccount { get; set; }

        /// <summary>
        /// 銀行交易序號
        /// </summary>
        public string BankTradeNo { get; set; }

        /// <summary>
        /// ATM回傳日期
        /// </summary>
        public string ATMTradeDate { get; set; }

        /// <summary>
        /// 銀行代碼
        /// </summary>
        public string BankCode { get; set; }

        /// <summary>
        /// 收單行虛擬帳號
        /// </summary>
        public string vAccount { get; set; }

        /// <summary>
        /// ATM有效日期
        /// </summary>
        public string ATMExpireDate { get; set; }

        /// <summary>
        /// 會員銀行帳號
        /// </summary>
        public string AccBank { get; set; }

        /// <summary>
        /// 會員銀行代碼
        /// </summary>
        public string AccNo { get; set; }

        /// <summary>
        /// 付款日期
        /// </summary>
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? PayDate { get; set; }

        /// <summary>
        /// 原始金額
        /// </summary>
        public decimal OriginalAmt { get; set; }

        /// <summary>
        /// 儲值後金額
        /// </summary>
        public decimal CurrentAmt { get; set; }

        /// <summary>
        /// 儲值日期
        /// </summary>
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? TopUpDate { get; set; }

        /// <summary>
        /// 可儲值額度
        /// </summary>
        public decimal TopUpAmtAvailable { get; set; }

        /// <summary>
        /// PaymentCenter的交易序號
        /// </summary>
        public long PaymentCenterTradeID { get; set; }
    }
}
