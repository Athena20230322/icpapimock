using ICP.Library.Models.AuthorizationApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    public class GraphicLockRerquest : BaseAuthorizationApiRequest
    {
        //public long MID { get; set; }
        public string OriPassword { get; set; }
        public string NewPassword { get; set; }
        //public string PwdUpgradeDate { get; set; }
        //public long realIP { get; set; }
        //public long proxyIP { get; set; }
    }
}
