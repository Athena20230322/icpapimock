using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICP.Host.Middleware.SMS.Models
{
    public class MistakeTemp
    {
        public long AutoID { get; set; }

        public string Phone { get; set; }

        public string MsgData { get; set; }

        public int SmsType { get; set; }

        public string CreateDate { get; set; }

        public string SendDate { get; set; }

        public int States { get; set; }

        public string GUID { get; set; }

        public string Sender { get; set; }
    }
}