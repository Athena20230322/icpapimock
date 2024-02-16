using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Models.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.CustomerManager
{
    public class ListOverSeaUserQryVM : PageModel
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [Display(Name = "手機號碼")]
        public string CellPhone { get; set; }
        
        [Display(Name = "居留證字號")]
        public string UniformID { get; set; }

        [Display(Name = "姓名")]
        public string CName { get; set; }
    }
}
