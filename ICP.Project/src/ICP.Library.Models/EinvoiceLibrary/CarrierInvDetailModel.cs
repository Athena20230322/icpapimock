using System;

namespace ICP.Library.Models.EinvoiceLibrary
{
    public class CarrierInvDetailModel
    {
        public string CarrierNum { get; set; }

        public string EinvoiceNum { get; set; }

        public string EinvoiceRandomNumber { get; set; }

        public string EinvoicePeriod { get; set; }

        public string EinvoiceSaleAmount { get; set; }

        public string EinvoiceStoreName { get; set; }

        public string EinvoiceItemDetail { get; set; }

        public int EinvoiceType { get; set; }

        public string EinvoiceAward { get; set; }

        public string EinvoiceAwardType { get; set; }

        public string EinvoiceAwardAmount { get; set; }

        public DateTime? EinvoiceCreateDate { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? ModifyDate { get; set; }

        public int? EinvoiceProductType { get; set; }

        public byte EinvoiceStatus { get; set; }

        public string VerificationCode { get; set; }
    }
}