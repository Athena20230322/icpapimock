using System;

namespace ICP.Library.Models.EinvoiceLibrary
{
    public class EinvoiceByCarrierDetailModel
    {
        public long MID { get; set; }

        public string CarrierNum { get; set; }

        public string EinvoiceNum { get; set; }

        public string EinvoicePeriod { get; set; }

        public DateTime? EinvoiceCreateDate { get; set; }

        public string VerificationCode { get; set; }
    }
}