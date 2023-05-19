using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Mvc.Admin.Models
{
    public class AccountLoginModel
    {
        [Required]
        [StringLength(20)]
        [Display(Name = "使用者帳號")]
        public string Account { get; set; }

        [Required]
        [Display(Name = "使用者密碼")]
        public string Pwd { get; set; }

        //[Required]
        //[Display(Name = "驗證碼")]
        //public string VaildCode { get; set; }
    }
}
