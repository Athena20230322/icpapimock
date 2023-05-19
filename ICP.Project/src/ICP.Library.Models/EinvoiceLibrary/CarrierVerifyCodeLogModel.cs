using System;

namespace ICP.Library.Models.EinvoiceLibrary
{
    public class CarrierVerifyCodeLogModel
    {
        public long RowId { get; set; }
        public long MID { get; set; }
        public string CarrierNum { get; set; }
        public DateTime CreateDate { get; set; }
    }
}