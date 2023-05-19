using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.CustomerSecurityManage
{
    public class TakeCashLimitQryVM : PageModel
    {
        /// <summary>
        /// 最近封鎖時間起日
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// 最近封鎖時間迄日
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// 電支帳號
        /// </summary>  
        public string ICPMID { get; set; }

        /// <summary>
        /// 手機號碼
        /// </summary>
        public string CellPhone { get; set; }

        /// <summary>
        /// 身份證字號
        /// </summary>
        public string IDNO { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

    }
}
