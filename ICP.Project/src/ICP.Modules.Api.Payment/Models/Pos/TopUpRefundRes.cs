using ICP.Infrastructure.Core.ExtraAttribute;
using ICP.Library.Models.AuthorizationApi;
using Newtonsoft.Json;
using System;

namespace ICP.Modules.Api.Payment.Models.Pos
{
    public class TopUpRefundRes : BaseAuthorizationApiResult
    {
        /// <summary>
        /// 愛金卡儲值退貨交易序號
        /// </summary>
        public string TransactionID { get; set; }

        /// <summary>
        /// 愛金卡帳戶
        /// </summary>
        public string IcashAccount { get; set; }

        /// <summary>
        /// 儲值退貨金額
        /// </summary>
        public decimal TopUpAmt { get; set; }

        /// <summary>
        /// 原始儲值後金額
        /// </summary>
        public decimal OriginalAmt { get; set; }

        /// <summary>
        /// 儲值退貨後金額
        /// </summary>
        public decimal CurrentAmt { get; set; }

        /// <summary>
        /// 儲值退貨時間
        /// </summary>
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? TopUpDiscardDate { get; set; }

        /// <summary>
        /// 可儲值額度
        /// </summary>
        public decimal TopUpAmtAvailable { get; set; }
    }
}