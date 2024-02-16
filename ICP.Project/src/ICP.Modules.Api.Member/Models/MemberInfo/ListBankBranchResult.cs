using ICP.Library.Models.AuthorizationApi;
using System.Collections.Generic;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    public class ListBankBranchResult : BaseAuthorizationApiResult
    {
        public List<ListBankBranchItem> BranchList { get; set; }
    }
}
