using ICP.Infrastructure.Core.ExtraAttribute;
using ICP.Infrastructure.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Payment.Models.ChargeBack
{
    public class ChargeBackSendToPaymentCenterRes : BaseResult
    {
       public PaymentCenterChargeBackEncData RtnData { get; set; }
    }

    public class PaymentCenterChargeBackEncData
    {
        /// <summary>
        /// 退款單號
        /// </summary>
        public long PaymentCenterTradeID { get; set; }

        /// <summary>
        /// 退款金額
        /// </summary>
        public decimal RefundAmount { get; set; }

        /// <summary>
        /// 訂單剩餘金額
        /// </summary>
        public decimal LeftAmount { get; set; }

        /// <summary>
        /// 退款日期
        /// </summary>
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime TradeDate { get; set; }

        /// <summary>
        /// 退款訂單流水碼
        /// </summary>
        public long ChargeBackID { get; set; }

        /// <summary>
        /// 標記此交易為取消交易(沖正)，不需帶退貨金額
        /// </summary>
        public int ForCancel { get; set; }

        /// <summary>
        /// PaymentCenter退款訂單流水號
        /// </summary>
        public long RefundTradeID { get; set; }

        /// <summary>
        /// 退款日期
        /// </summary>
        public DateTime RefundDate { get; set; }
    }
}
