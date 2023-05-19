using ICP.Infrastructure.Core.ExtraAttribute;
using ICP.Infrastructure.Core.Models;
using Newtonsoft.Json;
using System;

namespace ICP.Modules.Api.Payment.Models.Cashier
{
    public class PaymentCenterRes : BaseResult
    {
        public PaymentCenterEncDataModel RtnData { get; set; }

        /// <summary>
        /// 訂單流水碼
        /// </summary>
        public long TradeID { get; set; }

        /// <summary>
        /// 交易編號
        /// </summary>
        public string TradeNo { get; set; }

        /// <summary>
        /// 銀行帳號識別碼
        /// </summary>
        public long AccountID { get; set; }

        #region 儲值專用

        /// <summary>
        /// 儲值總金額
        /// </summary>
        public decimal TopUpAmt { get; set; }

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

        #endregion
    }
}
