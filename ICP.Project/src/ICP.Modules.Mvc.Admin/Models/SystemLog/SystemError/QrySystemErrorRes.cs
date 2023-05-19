using ICP.Infrastructure.Core.Models;
using System;

namespace ICP.Modules.Mvc.Admin.Models.SystemLog.SystemError
{
    public class QrySystemErrorRes : BaseListModel
    {
        /// <summary>
        /// 站臺名稱(0:全部, 1:會員系統, 2:交易系統, 3:金流系統, 4:後臺系統, 5:系統排程
        /// </summary>
        public int SiteType { get; set; }

        /// <summary>
        /// 程式路徑
        /// </summary>
        public string RequestUrl { get; set; }

        /// <summary>
        /// 伺服器名稱
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// Log流水碼
        /// </summary>
        public long LogId { get; set; }

        /// <summary>
        /// Log建立時間
        /// </summary>
        public DateTime CreateDT { get; set; }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 完整錯誤訊息
        /// </summary>
        public string Exception { get; set; }

        /// <summary>
        /// 錯誤類型(0:程式錯誤 1:DB 錯誤)
        /// </summary>
        public int ErrorType { get; set; }
    }
}
