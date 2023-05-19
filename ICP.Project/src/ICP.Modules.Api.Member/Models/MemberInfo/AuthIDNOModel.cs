using ICP.Library.Models.MemberModels;
using System;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    public class AuthIDNOModel : AuthIDNO
    {
        public byte IssueType { get; set; }

        public bool IsTeenager { get; set; }
    }
}
