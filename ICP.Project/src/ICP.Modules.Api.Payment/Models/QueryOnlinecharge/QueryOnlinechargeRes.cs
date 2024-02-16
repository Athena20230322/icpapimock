using ICP.Infrastructure.Core.ExtraAttribute;
using ICP.Library.Models.AuthorizationApi;
using Newtonsoft.Json;
using System;

namespace ICP.Modules.Api.Payment.Models.QueryOnlinecharge
{
    public class QueryOnlinechargeRes : BaseAuthorizationApiResult
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
        /// 交易金額
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// 支付種類(銀行代碼)
        /// </summary>
        public string BankNo { get; set; }

        /// <summary>
        /// 支付名稱(銀行名稱)
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// 交易狀態 0001:交易成功，其餘為失敗，錯誤代碼請參考狀態代碼表
        /// </summary>
        public string RtnCode { get; set; }

        /// <summary>
        /// 交易訊息
        /// </summary>
        public string RtnMsg { get; set; }

    }
}
