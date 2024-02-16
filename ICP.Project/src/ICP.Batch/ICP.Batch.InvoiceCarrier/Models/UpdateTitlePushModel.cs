using System;

namespace ICP.Batch.InvoiceCarrier.Models
{
    public class UpdateTitlePushModel
    {
        public string CarrierNum { get; set; }

        public DateTime DownloadDate { get; set; }

        public byte Status { get; set; }
    }
}