namespace ICP.Library.Models.EinvoiceLibrary.DTO
{
    public class CarrierInvTitleResultDTO : BaseResultDTO
    {
        public string OnlyWinningInv { get; set; }

        public TilteDetail[] Details { get; set; }
    }

    public class TilteDetail
    {
        public string RowNum { get; set; }
        public string InvNum { get; set; }
        public InvDate InvDate { get; set; }
        public string SellerName { get; set; }
        public string InvStatus { get; set; }
        public string InvDonatable { get; set; }
        public string CardType { get; set; }
        public string CardNo { get; set; }
        public string Amount { get; set; }
        public string InvPeriod { get; set; }
        public string InvoiceTime { get; set; }
        public string SellerBan { get; set; }
        public string SellerAddress { get; set; }
        public string DonateMark { get; set; }
        public string Currency { get; set; }
    }

    public class InvDate
    {
        public string Date { get; set; }
        public string Day { get; set; }
        public string Hours { get; set; }
        public string Minutes { get; set; }
        public string Month { get; set; }
        public string Seconds { get; set; }
        public string Time { get; set; }
        public string TimezoneOffset { get; set; }
        public string Year { get; set; }
    }
}
