using ICP.Infrastructure.Core.Models.Consts;
using ICP.Library.Models.AuthorizationApi;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    public class UpdateNickNameRequest : BaseAuthorizationApiRequest
    {
        [Required]
        [RegularExpression(RegexConst.NickName, ErrorMessage = "您輸入的格式不符")]
        [Display(Name = "暱稱")]
        public string NickName { get; set; }
    }
}
