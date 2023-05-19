namespace ICP.Library.Models.EinvoiceLibrary
{
    public class InvoiceBindLogModel
    {
        public long MID { get; set; }
        public string CarruerNum { get; set; }
        public int Status { get; set; }
        public string BindToken { get; set; }
        public int RealIP { get; set; }
        public int ProxyIP { get; set; }

    }
}