using ICP.Infrastructure.Core.Models;
using System;

namespace ICP.Modules.Mvc.Admin.Models.SystemLog.SystemError
{
    public class QrySystemErrorReq : PageModel
    {
        /// <summary>
        /// 查詢起日
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 查詢迄日
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 站臺名稱(0:全部, 1:會員系統, 2:交易系統, 3:金流系統, 4:後臺系統, 5:系統排程
        /// </summary>
        public int SiteType { get; set; }

        /// <summary>
        /// 錯誤類型(99:全部, 1:DB錯誤, 0:程式錯誤)
        /// </summary>
        public int ErrorType { get; set; } = 99;

        /// <summary>
        /// 伺服器名稱
        /// </summary>
        public string MachineName { get; set; }
    }
}
