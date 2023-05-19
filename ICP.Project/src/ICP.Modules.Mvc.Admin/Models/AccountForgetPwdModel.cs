using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Mvc.Admin.Models
{
    public class AccountForgetPwdModel
    {
        [Required]
        [StringLength(20)]
        [Display(Name = "使用者帳號")]
        public string Account { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        [Display(Name = "使用者Email")]
        public string Email { get; set; }
    }
}
