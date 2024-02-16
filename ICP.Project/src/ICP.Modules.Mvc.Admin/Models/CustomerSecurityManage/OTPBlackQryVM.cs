using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.CustomerSecurityManage
{
    public class OTPBlackQryVM : PageModel
    {
        ///// <summary>
        ///// 查詢日期 啟始
        ///// </summary>
        //public DateTime CreateDateBegin { get; set; }

        ///// <summary>
        ///// 查詢日期 結束
        ///// </summary>
        //public DateTime CreateDateEnd { get; set; }

        /// <summary>
        /// 最近封鎖時間起日
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// 最近封鎖時間迄日
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// 手機號碼
        /// </summary>  
        public string CellPhone { get; set; }

        /// <summary>
        /// 身份證字號/居留證
        /// </summary>
        public string IDNO { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

    }
}
