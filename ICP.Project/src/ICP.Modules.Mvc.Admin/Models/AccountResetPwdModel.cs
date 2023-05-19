using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Mvc.Admin.Models
{
    using ICP.Infrastructure.Core.Models;
    using Infrastructure.Core.Models.Consts;

    public class AccountResetPwdModel: ValidatableObject
    {
        [Required]
        [Display(Name = "新密碼")]
        [RegularExpression(RegexConst.AdminUserPwd, ErrorMessage = "請輸入6-20碼英數混合新密碼")]
        public string Pwd { get; set; }

        [Required]
        [Display(Name = "確認密碼")]
        [Compare("Pwd")]
        public string ConfirmPwd { get; set; }
    }
}
