using ICP.Library.Models.AuthorizationApi;

namespace ICP.Modules.Api.Payment.Models.CreateBarcode
{
    public class TradeBarcodeRes : BaseAuthorizationApiRequest
    {
        /// <summary>
        /// 付款條碼
        /// </summary>
        public string Barcode { get; set; }
    }
}