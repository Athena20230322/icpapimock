using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    public class MemberGraphicLock : BaseResult
    {
        public long MID { get; set; }
        public string Password { get; set; }
        public bool PasswordSwitch { get; set; }
        public string CreateDate { get; set; }
        public string ModifyDate { get; set; }
        public DateTime? IgnoreDate { get; set; }
        public int PwdErrorCounts { get; set; }
    }
}
