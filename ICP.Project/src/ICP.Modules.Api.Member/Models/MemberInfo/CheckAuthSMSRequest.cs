using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    using Infrastructure.Core.Models.Consts;
    using Library.Models.AuthorizationApi;

    public class CheckAuthSMSRequest : BaseAuthorizationApiRequest
    {
        [Required]
        [RegularExpression(RegexConst.CellPhone, ErrorMessage = "{0} 格式錯誤")]
        [Display(Name = "手機號碼")]
        public string CellPhone { get; set; }

        [Required]
        [RegularExpression("^[2,3,4,6,7,8]{1}$", ErrorMessage = "{0} 錯誤")]
        [Display(Name = "驗證類別")]
        public long SMSAuthType { get; set; }

        [Required]
        [Display(Name = "驗證碼")]
        public string AuthCode { get; set; }
    }
}
