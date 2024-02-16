using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    using Infrastructure.Core.ValidationAttributes;
    using Library.Models.AuthorizationApi;
    using Library.Models.MemberModels;

    public class UpdateMemberPreferencesRequest: BaseAuthorizationApiRequest
    {
        [ValidateObject]
        public List<MemberPreferenceModel> Options { get; set; }
    }
}
