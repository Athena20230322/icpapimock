using ICP.Infrastructure.Core.ExtraAttribute;
using ICP.Library.Models.AuthorizationApi;
using Newtonsoft.Json;
using System;

namespace ICP.Modules.Api.Payment.Models.Pos
{
    public class RefundRes : BaseAuthorizationApiResult
    {
        /// <summary>
        /// 愛金卡退款交易序號
        /// </summary>
        public string TransactionID { get; set; }

        /// <summary>
        /// 退款時間
        /// </summary>
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? PaymentDate { get; set; }
    }
}