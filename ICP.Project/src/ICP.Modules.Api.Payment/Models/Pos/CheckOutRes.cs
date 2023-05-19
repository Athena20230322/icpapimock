using ICP.Infrastructure.Core.ExtraAttribute;
using ICP.Library.Models.AuthorizationApi;
using Newtonsoft.Json;
using System;

namespace ICP.Modules.Api.Payment.Models.Pos
{
    public class CheckOutRes : BaseAuthorizationApiResult
    {
        /// <summary>
        /// 愛金卡交易序號
        /// </summary>
        public string TransactionID { get; set; }

        /// <summary>
        /// 愛金卡帳戶
        /// </summary>
        public string IcashAccount { get; set; }

        /// <summary>
        /// 交易總金額
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 付款時間
        /// </summary>
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? PaymentDate { get; set; }

        /// <summary>
        /// 扣款金流類型
        /// </summary>
        public string PaymentType { get; set; }

        /// <summary>
        /// 支付識別碼
        /// </summary>
        public string IcashAccountPayID { get; set; }

        /// <summary>
        /// 銀行代碼
        /// </summary>
        public string BankNo { get; set; }

        /// <summary>
        /// 銀行名稱
        /// </summary>
        public string BankName { get; set; }
    }
}