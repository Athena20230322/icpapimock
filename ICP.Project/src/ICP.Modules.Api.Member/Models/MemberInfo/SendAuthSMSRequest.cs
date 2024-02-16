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

    /// <summary>
    /// 簡訊驗證
    /// </summary>
    public class SendAuthSMSRequest: BaseAuthorizationApiRequest
    {
        [Required]
        [RegularExpression(RegexConst.CellPhone, ErrorMessage = "{0}格式錯誤，請重新輸入")]
        [Display(Name = "手機號碼")]
        public string CellPhone { get; set; }

        /// <summary>
        /// 驗證類別
        /// 1：註冊時發送簡訊 
        /// 2：忘記登入密碼發送簡訊
        /// 3：忘記登入帳號發送簡訊
        /// 4：忘記圖形鎖發送簡訊
        /// </summary>
        [Required]
        [Range(1, 8)]
        [Display(Name = "驗證類別")]
        public byte SMSAuthType { get; set; }
        
        [RegularExpression(RegexConst.UserCode, ErrorMessage = "{0}格式錯誤，請重新輸入")]
        [Display(Name = "帳號")]
        public string UserCode { get; set; }
    }
}
