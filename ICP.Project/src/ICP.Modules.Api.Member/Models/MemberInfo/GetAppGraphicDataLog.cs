using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    public class GetAppGraphicDataLog
    {
        public long MID { get; set; }
        public string DeviceID { get; set; }
        public string CreateDate { get; set; }
        public long realIP { get; set; }
        public long ProxyIP { get; set; }
    }
}
