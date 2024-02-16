using ICP.Library.Models.AuthorizationApi;
using ICP.Library.Models.MemberModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    public class GetMemberPreferencesResult: BaseAuthorizationApiResult
    {
        public List<MemberPreferenceModel> Options { get; set; }
    }
}
