using ICP.Infrastructure.Core.ExtraAttribute;
using ICP.Infrastructure.Core.Models;
using Newtonsoft.Json;
using System;

namespace ICP.Modules.Api.Payment.Models.ChargeBack
{
    public class UpdateChargeBackDbRes : BaseResult
    {
        /// <summary>
        /// 退款時間
        /// </summary>
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? PaymentDate { get; set; }

        /// <summary>
        /// 愛金卡帳戶
        /// </summary>
        public string IcashAccount { get; set; }

        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }
    }
}
