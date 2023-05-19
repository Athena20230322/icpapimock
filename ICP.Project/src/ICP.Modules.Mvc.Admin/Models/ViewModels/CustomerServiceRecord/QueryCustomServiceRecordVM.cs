using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Models.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.ViewModels.CustomerServiceRecord
{
    public class QueryCustomServiceRecordVM : PageModel
    {
        /// <summary>
        /// 起始日期
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 結束時間
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 回報者姓名
        /// </summary>
        public string CName { get; set; }
        /// <summary>
        /// 電支帳號
        /// </summary>
        public string ICPMID { get; set; }
        /// <summary>
        /// 手機號碼
        /// </summary>
        public string CellPhone { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 訂單編號
        /// </summary>
        public string TradeNo { get; set; }
        /// <summary>
        /// 案件編號
        /// </summary>
        public string CaseNo { get; set; }
        /// <summary>
        /// 案件進度
        /// </summary>
        public byte? Status { get; set; }
    }
}
