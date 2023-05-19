using System;

namespace ICP.Batch.InvoiceCarrier.Models
{
    public class TitlePushModel
    {
        public long MID { get; set; }

        public string CarrierNum { get; set; }

        public DateTime DownloadDate { get; set; }

        public string VerificationCode { get; set; }
    }
}