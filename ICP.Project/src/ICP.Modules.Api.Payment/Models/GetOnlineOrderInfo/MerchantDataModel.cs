namespace ICP.Modules.Api.Payment.Models.GetOnlineOrderInfo
{
    public class MerchantDataModel
    {
        /// <summary>
        /// 特店代碼
        /// </summary>
        public string MerchantID { get; set; }

        /// <summary>
        /// 特店icashpay電支帳號
        /// </summary>
        public string MerchantIcpMID { get; set; }

        /// <summary>
        /// 特店名稱
        /// </summary>
        public string MerchantName { get; set; }

        /// <summary>
        /// 特店icon URL
        /// </summary>
        public string MerchantIconUrl { get; set; }
    }
}

