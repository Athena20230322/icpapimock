using ICP.Library.Models.AuthorizationApi;

namespace ICP.Modules.Api.Payment.Models.GetOnlineOrderInfo
{
    public class GetOnlineOrderInfoReq : BaseAuthorizationApiRequest
    {
        /// <summary>
        /// 會員選擇的付款方式ID (OP提供)
        /// </summary>
        public string PayID { get; set; }

        /// <summary>
        /// 平台商代碼
        /// </summary>
        public long PlatformID { get; set; }

        /// <summary>
        /// 廠商代碼
        /// </summary>
        public long MerchantID { get; set; }

        /// <summary>
        /// 交易編號
        /// </summary>
        public string MerchantTradeNo { get; set; }
    }
}
