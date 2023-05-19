namespace ICP.Library.Models.EinvoiceLibrary.DTO
{
    public class InvDetailResultDTO : BaseResultDTO
    {
        public string InvNum { get; set; }

        public string InvDate { get; set; }

        public string SellerName { get; set; }

        public string InvStatus { get; set; }

        public string InvPeriod { get; set; }

        public string SellerBan { get; set; }

        public string SellerAddress { get; set; }

        public string InvoiceTime { get; set; }

        public string Currency { get; set; }

        public Detail2[] Details { get; set; }
    }

    public class Detail2
    {
        public string Amount { get; set; }

        public string Description { get; set; }

        public string UnitPrice { get; set; }

        public string Quantity { get; set; }

        public string RowNum { get; set; }
    }
}
