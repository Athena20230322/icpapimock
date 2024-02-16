using ICP.Infrastructure.Core.Models.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.CustomerManager
{
    public class EditCellPhoneModel
    {


        [Required(ErrorMessage = "{0}格式錯誤")]
        [RegularExpression(RegexConst.CellPhone, ErrorMessage = "{0}格式錯誤")]
        [Display(Name = "手機號碼")]
        public string CellPhone { get; set; }

        public long MID { get; set; }

        [Required(ErrorMessage = "{0}不得為空")]
        [MaxLength(100, ErrorMessage = "{0}格式錯誤")]
        [Display(Name = "修改原因")]
        public string Remark { get; set; }

        public string ModifyUser { get; set; }

        public long RealIP { get; set; }

        public long ProxyIP { get; set; }
    }
}
