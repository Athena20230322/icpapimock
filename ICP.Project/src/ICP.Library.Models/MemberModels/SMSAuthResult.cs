using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.MemberModels
{
    using Infrastructure.Core.Models;

    public class SMSAuthResult: BaseResult
    {
        public byte AuthType { get; set; }

        public string CellPhone { get; set; }

        public long MID { get; set; }

        public string AuthCode { get; set; }

        public DateTime ExpireDT { get; set; }

        public int ExpireRange { get; set; }
    }
}
