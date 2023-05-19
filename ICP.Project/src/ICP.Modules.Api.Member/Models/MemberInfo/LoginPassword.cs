using ICP.Library.Models.AuthorizationApi;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    public class LoginPassword : BaseAuthorizationApiRequest
    {
        
        [Display(Name = "登入密碼")]
        public string OriLoginPassword { get; set; }

        [Display(Name = "新登入密碼")]
        public string NewLoginPassword { get; set; }

        [Display(Name = "確認新登入密碼")]
        public string ConfirmLoginPassword { get; set; }
    }
}
