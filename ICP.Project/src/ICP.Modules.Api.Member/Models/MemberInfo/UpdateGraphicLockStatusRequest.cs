using ICP.Library.Models.AuthorizationApi;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    public class UpdateGraphicLockStatusRequest : BaseAuthorizationApiRequest
    {
        [Required]
        public bool Status { get; set; }
    }
}
