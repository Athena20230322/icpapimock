using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICP.Host.APIService.Models
{
    public class FETRtnModel
    {
        public string SysId { get; set; }

        public string MessageId { get; set; }

        public string DestAddress { get; set; }

        public string DeliveryStatus { get; set; }

        public string ErrorCode { get; set; }

        public string SubmitDate { get; set; }

        public string DoneDate { get; set; }

        public string Seq { get; set; }
    }
}