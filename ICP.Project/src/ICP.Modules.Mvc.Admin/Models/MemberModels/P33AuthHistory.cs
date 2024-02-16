using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.MemberModels
{
    public class P33AuthHistory : PageModel
    {
        /// <summary>
        /// 會員編號
        /// </summary>
        public long? MID { get; set; }
        /// <summary>
        /// 電支編號
        /// </summary>
        public string ICPMID { get; set; }
        /// <summary>
        /// 驗證日期(起)
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// 驗證日期(迄)
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 身分證號/統編
        /// </summary>
        public string IDNO { get; set; }
        /// <summary>
        /// 是否通過驗證
        /// 0:未通過
        /// 1:通過
        /// 2:待審
        /// null : 全選
        /// </summary>
        public short? IsPass { get; set; }
    }
}
