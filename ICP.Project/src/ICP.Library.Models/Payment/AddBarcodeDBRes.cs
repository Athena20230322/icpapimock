using ICP.Infrastructure.Core.Models;

namespace ICP.Library.Models.Payment
{
    public class AddBarcodeDBRes : BaseResult
    {
        /// <summary>
        /// 條碼
        /// </summary>
        public string Barcode { get; set; }
    }
}
