using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models
{
    public class UserExecuted
    {
        /// <summary>
        /// 流水號
        /// </summary>
        public long LogID { get; set; }

        /// <summary>
        /// 登入者編號
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// 登入者帳號
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// Controller名稱
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// Action名稱
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// 路徑
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Headers(不含cookie)
        /// </summary>
        public string Headers { get; set; }

        /// <summary>
        /// 網址參數
        /// </summary>
        public string UrlQuery { get; set; }

        /// <summary>
        /// 請求內容
        /// </summary>
        public string FormData { get; set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// RealIP
        /// </summary>
        public long RealIP { get; set; }

        /// <summary>
        /// ProxyIP
        /// </summary>
        public long ProxyIP { get; set; }

    }
}
