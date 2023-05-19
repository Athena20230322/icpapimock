using ICP.Batch.PICChargeFeeOpen.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.PICChargeFeeOpen.Models
{
    public class CF_InvoiceIssue_UpdateStatusModel
    {
        public long InvoiceID { get; set; }

        public string InvoiceNo { get; set; }

        public string RtnCode { get; set; }

        public string RtnMsg { get; set; }

        public Issue_StatusEnum Issue_Status { get; set; }

        public StateEnums State { get; set; }

        public string Modifier { get; set; }
    }
}
