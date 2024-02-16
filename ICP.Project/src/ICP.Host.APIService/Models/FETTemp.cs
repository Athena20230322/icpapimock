using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICP.Host.APIService.Models
{
    public class FETTemp
    {
        public long AutoID { get; set; }

        public string Phone { get; set; }

        public string MsgData { get; set; }

        public byte? SmsType { get; set; }
    }
}