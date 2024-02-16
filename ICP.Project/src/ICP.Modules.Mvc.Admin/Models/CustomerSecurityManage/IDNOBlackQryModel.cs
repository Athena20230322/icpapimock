using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.CustomerSecurityManage
{
    public class IDNOBlackQryModel : PageModel
    {
        /// <summary>
        /// 身份證字號/居留證
        /// </summary>  
        public string IDNO { get; set; }
    }
}
