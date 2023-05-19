using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class AccountChangePwdModel: AccountResetPwdModel
    {
        [Required]
        [Display(Name = "原密碼")]
        public string OriginPwd { get; set; }
    }
}
