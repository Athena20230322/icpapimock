namespace ICP.Modules.Api.Payment.Models.Cashier
{
    public class PaymentCenterReq
    {
        /// <summary>
        /// 會員代碼
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 訂單流水號
        /// </summary>
        public long TradeID { get; set; }

        /// <summary>
        /// 訂單編號
        /// </summary>
        public string TradeNo { get; set; }

        /// <summary>
        /// 廠商編號
        /// </summary>
        public long MerchantID { get; set; }

        /// <summary>
        /// 平台編號
        /// </summary>
        public long PlatformID { get; set; }

        /// <summary>
        /// 交易金額
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 交易模式(交易:1 儲值:2 轉帳:3 提領:4)
        /// </summary>
        public int TradeModeID { get; set; }

        /// <summary>
        /// 付款方式代碼
        /// </summary>
        public int PaymentTypeID { get; set; }

        /// <summary>
        /// 收單行名稱代碼
        /// </summary>
        public int PaymentSubTypeID { get; set; }

        /// <summary>
        /// 交易編號
        /// </summary>
        public string MerchantTradeNo { get; set; }

        /// <summary>
        /// 銀行帳號識別碼
        /// </summary>
        public long AccountID { get; set; }

        /// <summary>
        /// 檢核碼
        /// </summary>
        public string CheckMacValue { get; set; }
    }
}
