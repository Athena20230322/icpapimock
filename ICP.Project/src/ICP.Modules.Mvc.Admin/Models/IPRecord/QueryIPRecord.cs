using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.IPRecord
{
    public class QueryIPRecord : PageModel
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
        /// 登入帳號
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 登入IP
        /// </summary>
        public long? UserIP { get; set; }
        /// <summary>
        /// 電支帳號
        /// </summary>
        public string ICPMID { get; set; }
        /// <summary>
        /// 手機號碼
        /// </summary>
        public string CellPhone { get; set; }
        /// <summary>
        /// 裝置ID
        /// </summary>
        public string DeviceID { get; set; }
    }
}
