using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICP.Host.Middleware.JCIC.Models
{
    public class P33Auth
    {
        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 身分證/居留證
        /// </summary>
        public string IDNO { get; set; }

        /// <summary>
        /// 後台帳號
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 呼叫來源
        /// 1 = APP
        /// 2 = 後台
        /// 3 = 排程
        /// </summary>
        public byte Source { get; set; }

        /// <summary>
        /// 客戶端IP
        /// </summary>
        public long RealIP { get; set; }

        /// <summary>
        /// 客戶端Proxy IP
        /// </summary>
        public long ProxyIP { get; set; }
    }
}