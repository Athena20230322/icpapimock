using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.CustomerSecurityManage
{
    public class RegistIPBlackQryModel : PageModel
    {
        /// <summary>
        /// IP位置
        /// </summary> 
        public string IP { get; set; }
    }
}
