using ICP.Infrastructure.Core.ExtraAttribute;
using ICP.Modules.Api.Authorization.Models;
using Newtonsoft.Json;
using System;

namespace ICP.Modules.Api.Payment.Models.ChargeOnline
{
    public class EncDataModel : ICPRequestContext
    {
        /// <summary>
        /// 交易編號
        /// </summary>
        public string MerchantTradeNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ts { get; set; }

        /// <summary>
        /// OW端確認付款時間
        /// </summary>
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime TimeStamp { get; set; }
    }
}

