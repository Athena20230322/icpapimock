namespace ICP.Library.Models.EinvoiceLibrary.DTO
{
    public class CarrierInvTitleDTO : BaseDTO
    {
        public string CardNo { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string OnlyWinningInv { get; set; }

        public string CardEncrypt { get; set; }
    }
}
