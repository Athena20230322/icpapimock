namespace ICP.Library.Models.EinvoiceLibrary.DTO
{
    public class CarrierInvDetailResultDTO : BaseResultDTO
    {
        public string InvNum { get; set; }

        public string SellerName { get; set; }

        public int Amount { get; set; }

        public string InvStatus { get; set; }

        public string InvPeriod { get; set; }

        public string InvDate { get; set; }

        public string InvoiceTime { get; set; }

        public string SellerBan { get; set; }

        public string SellerAddress { get; set; }

        public string Currency { get; set; }

        public Detail[] Details { get; set; }
    }

    public class Detail
    {
        public string RowNum { get; set; }

        public string Description { get; set; }

        public string Quantity { get; set; }

        public string UnitPrice { get; set; }

        public string Amount { get; set; }
    }
}
