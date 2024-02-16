namespace ICP.Library.Models.EinvoiceLibrary.DTO
{
    public class CarrierUnderTypeDTO : BaseDTO
    {
        /// <summary>
        /// 手機條碼
        /// </summary>
        public string CardNo { get; set; }

        /// <summary>
        /// 手機條碼驗證碼
        /// </summary>
        public string CardEncrypt { get; set; }
    }
}
