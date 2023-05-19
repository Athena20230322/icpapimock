using ICP.Library.Models.AuthorizationApi;

namespace ICP.Modules.Api.Payment.Models.GetOnlineOrderInfo
{
    public class GetOnlineOrderInfoRes : BaseAuthorizationApiResult
    {
        /// <summary>
        /// 特店icashpay電支帳號
        /// </summary>
        public string MerchantID { get; set; }

        /// <summary>
        /// 特店名稱
        /// </summary>
        public string MerchantName { get; set; }

        /// <summary>
        /// 特店icon URL
        /// </summary>
        public string MerchantIconUrl { get; set; }

        /// <summary>
        /// 付款方icashpay電支帳號
        /// </summary>
        public string PayerIcpMID { get; set; }

        /// <summary>
        /// 付款方姓名
        /// </summary>
        public string PayerName { get; set; }

        /// <summary>
        /// 付款時間 格式：2019/01/01 00:00:00
        /// </summary>
        public string TradeDate { get; set; }

        /// <summary>
        /// 付款方式(1：icashpay電支帳戶 2：連結帳戶)
        /// </summary>
        public string PaymentType { get; set; }

        /// <summary>
        /// 連結帳戶-銀行名稱(簡寫四碼)
        /// </summary>
        public string BankShortName { get; set; }

        /// <summary>
        /// 連結帳戶-帳號末五碼
        /// </summary>
        public string AccountLast5No { get; set; }

        /// <summary>
        /// 訂單金額(交易金額包含2位小數點。e.g. 30000 (代表NT$300))
        /// </summary>
        public decimal TradeAmount { get; set; }

        /// <summary>
        /// 實際付款金額 (訂單金額-OPENPOINT折抵)
        /// </summary>
        public decimal RealAmount { get; set; }

        /// <summary>
        /// OPEN POINT折抵金額
        /// </summary>
        public decimal BonusAmt { get; set; }

        /// <summary>
        /// 交易資料
        /// </summary>
        public string AppData { get; set; }
    }
}
