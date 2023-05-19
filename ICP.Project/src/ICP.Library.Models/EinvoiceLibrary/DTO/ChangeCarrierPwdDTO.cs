namespace ICP.Library.Models.EinvoiceLibrary.DTO
{
    public class ChangeCarrierPwdDTO : BaseDTO
    {
        public string CardNo { get; set; }

        public string NewVerify { get; set; }

        public string OldVerify { get; set; }
    }
}
