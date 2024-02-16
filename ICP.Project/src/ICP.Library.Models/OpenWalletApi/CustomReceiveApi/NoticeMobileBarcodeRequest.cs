using ICP.Library.Models.OpenWalletApi.CustomSendApi;

namespace ICP.Library.Models.OpenWalletApi.CustomReceiveApi
{
    /// <summary>
    /// 會員手機條碼異動
    /// </summary>
    public class NoticeMobileBarcodeRequest: BaseCustomReceiveApiRequest
    {
        /// <summary>
        /// 會員mid
        /// </summary>
        public string mid { get; set; }
        /// <summary>
        /// 手機條碼載具
        /// </summary>
        public string mobile_barcode { get; set; }
    }
}