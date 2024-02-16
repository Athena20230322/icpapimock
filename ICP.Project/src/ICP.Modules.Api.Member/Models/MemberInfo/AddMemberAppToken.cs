using ICP.Library.Models.MemberModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    public class AddMemberAppToken: MemberAppToken
    {
        public string OPErrorCode { get; set; }

        public string OPErrorMessage { get; set; }
    }
}
