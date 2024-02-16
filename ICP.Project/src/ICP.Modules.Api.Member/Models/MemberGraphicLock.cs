using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICP.Modules.Api.Member.Models
{
    public class MemberGraphicLock
    {
        public long MID { get; set; }
        public string Password { get; set; }
        public bool PasswordSwitch { get; set; }
        public string CreateDate { get; set; }
        public string ModifyDate { get; set; }
        public DateTime? IgnoreDate { get; set; }
        public int PwdErrorCounts { get; set; }
      
    }

    public class GraphicLockRerquest
    {
        public long MID { get; set; }
        public string OriPassword { get; set; }
        public string NewPassword { get; set; }
        public string PwdUpgradeDate { get; set; }
        public long realIP { get; set; }
        public long proxyIP { get; set; }
    }

    public class GetAppGraphicDataLog
    {
        public long MID { get; set; }
        public string DeviceID { get; set; }
        public string CreateDate { get; set; }
        public long realIP { get; set; }
        public long ProxyIP { get; set; }
    }
}