using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.CustomerSecurityManage
{
    public class RegistIPBlackListModel
    {
        [Display(Name = "IP預警的流水號")]
        public long RowNo { get; set; }

        [Display(Name = "IP預警的IP")]
        public string RealIP { get; set; }

        [Display(Name = "IP預警的註冊次數")]
        public long Tcount { get; set; }
    }
}
