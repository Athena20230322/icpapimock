namespace ICP.Modules.Api.Payment.Models
{
    public class iCashAccountInfo
    {
        /// <summary>
        /// 付款方式識別碼
        /// </summary>
        public string PayID { get; set; }

        /// <summary>
        /// 電支帳戶可用餘額
        /// </summary>
        public decimal AvailableCash { get; set; }

        /// <summary>
        /// 電支帳戶餘額
        /// </summary>
        public decimal AccountCash { get; set; }
            
        /// <summary>
        /// 是否開啟自動儲值(0：關, 1：開)
        /// </summary>
        public int IsAutoTopUp { get; set; }
    }
}
