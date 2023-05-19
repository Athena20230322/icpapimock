namespace ICP.Library.Models.EinvoiceLibrary.DTO
{
    public class InvDetailDTO : BaseDTO
    {
        public string RandomNumber { get; set; }

        public string SellerID { get; set; }

        public string Type { get; set; }

        public string InvNum { get; set; }

        public string InvTerm { get; set; }

        public string InvDate { get; set; }

        public string Encrypt { get; set; }
    }
}
