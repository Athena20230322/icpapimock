using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.AuthorizationApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    public class SecPasswordResult : BaseResult
    {
        public string Email { get; set; }
    }
}
