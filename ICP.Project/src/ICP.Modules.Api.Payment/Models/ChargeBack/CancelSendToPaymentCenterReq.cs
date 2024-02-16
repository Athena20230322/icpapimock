namespace ICP.Modules.Api.Payment.Models.ChargeBack
{
    public class CancelSendToPaymentCenterReq
    {
        /// <summary>
        /// 廠商編號
        /// </summary>
        public long MerchantID { get; set; }

        /// <summary>
        /// 平台商編號
        /// </summary>
        public long PlatformID { get; set; }

        /// <summary>
        /// 愛金卡交易序號
        /// </summary>
        public string TradeNo { get; set; }

        /// <summary>
        /// 訂單編號
        /// </summary>
        public string MerchantTradeNo { get; set; }

        /// <summary>
        /// 檢核碼
        /// </summary>
        public string CheckMacValue { get; set; }

        /// <summary>
        /// PaymentCenter的訂單流水號
        /// </summary>
        public long PaymentCenterTradeID { get; set; }
    }
}