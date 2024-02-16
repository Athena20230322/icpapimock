namespace ICP.Library.Models.EinvoiceLibrary
{
    public class InvoiceBindReturn:BaseDataModel
    {
        public string card_ban { get; set; }
        public string card_no1 { get; set; }
        public string card_no2 { get; set; }
        public string card_type { get; set; }
        public string back_url { get; set; }
        public string token { get; set; }
        public string rtn_flag { get; set; }
        public string postUrl { get; set; }
    }
}