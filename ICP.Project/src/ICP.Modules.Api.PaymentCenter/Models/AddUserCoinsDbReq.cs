namespace ICP.Modules.Api.PaymentCenter.Models
{
    /// <summary>
    /// 新增電支帳戶金額
    /// </summary>
    public class AddUserCoinsDbReq
    {
        /// <summary>
        /// 交易編號
        /// </summary>
        public string TradeNo { get; set; }

        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 特店編號
        /// </summary>
        public long MerchantID { get; set; }

        /// <summary>
        /// 帳務類型
        /// </summary>
        public short TradeModeID { get; set; }

        /// <summary>
        /// 帳務子類型
        /// </summary>
        public short PaymentTypeID { get; set; }

        /// <summary>
        /// 款項來源
        /// </summary>
        public short PaymentSubTypeID { get; set; }

        /// <summary>
        /// 交易金額
        /// </summary>
        public decimal TradeRealCash { get; set; }

        /// <summary>
        /// 儲值金額
        /// </summary>
        public decimal TradeTopUpCash { get; set; }

        /// <summary>
        /// 幣別
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        public string Notes { get; set; }
    }
}
