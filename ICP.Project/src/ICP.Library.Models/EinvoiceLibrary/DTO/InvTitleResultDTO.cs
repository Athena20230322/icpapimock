namespace ICP.Library.Models.EinvoiceLibrary.DTO
{
    public class InvTitleResultDTO : BaseResultDTO
    {
        public string InvNum { get; set; }

        public string InvData { get; set; }

        public string SellerName { get; set; }

        public string InvStatus { get; set; }

        public string InvPeriod { get; set; }

        public string SellerBan { get; set; }

        public string SellerAddress { get; set; }

        public string InvoiceTime { get; set; }

        public string Currency { get; set; }
    }
}
