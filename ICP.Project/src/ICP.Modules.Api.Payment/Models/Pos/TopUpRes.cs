using ICP.Infrastructure.Core.ExtraAttribute;
using ICP.Library.Models.AuthorizationApi;
using Newtonsoft.Json;
using System;

namespace ICP.Modules.Api.Payment.Models.Pos
{
    public class TopUpRes : BaseAuthorizationApiResult
    {
        /// <summary>
        /// 愛金卡儲值序號
        /// </summary>
        public string TransactionID { get; set; }

        /// <summary>
        /// 愛金卡帳戶
        /// </summary>
        public string IcashAccount { get; set; }

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
    }
}