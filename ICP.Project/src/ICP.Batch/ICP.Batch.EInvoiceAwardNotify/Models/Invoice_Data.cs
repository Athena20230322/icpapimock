using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.EInvoiceAwardNotify.Models
{
    public class Invoice_Data
    {
        public Invoice_Award master { get; set; }

        public List<Invoice_AwardDetail> details { get; set; }
    }
}
