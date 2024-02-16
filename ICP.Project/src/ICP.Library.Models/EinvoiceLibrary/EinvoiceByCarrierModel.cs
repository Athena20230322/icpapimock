using System;

namespace ICP.Library.Models.EinvoiceLibrary
{
    public class EinvoiceByCarrierModel
    {
        public string EinvoiceItemDetail { get; set; }
        public string InvNum { get; set; }
        public DateTime EinvoiceCreateDate { get; set; }
        public string InvPeriod { get; set; }
    }
}