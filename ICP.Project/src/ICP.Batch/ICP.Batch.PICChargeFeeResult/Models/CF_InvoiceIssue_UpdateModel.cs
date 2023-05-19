using ICP.Batch.PICChargeFeeResult.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.PICChargeFeeResult.Models
{
    public class CF_InvoiceIssue_UpdateModel
    {
        public string InvoiceNo { get; set; }

        public string InvoiceNumber { get; set; }

        public string InvoiceDate { get; set; }

        public Issue_StatusEnum Issue_Status { get; set; }

        public StateEnums State { get; set; }

        public string RtnCode { get; set; }

        public string RtnMsg { get; set; }

        public string Modifier { get; set; }

    }
}
