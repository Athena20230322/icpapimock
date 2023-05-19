using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICP.Host.Middleware.JCIC.Models
{
    public class P11Auth
    {
        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 身分證號
        /// </summary>
        public string IDNO { get; set; }

        /// <summary>
        /// 領補換日期 民國年(YYYMMDD)
        /// </summary>
        public string IssueDate { get; set; }
        
        /// <summary>
        /// 領補換代號
        /// 1 = 初領
        /// 2 = 補領
        /// 3 = 換領
        /// </summary>
        public int IssueType { get; set; }

        /// <summary>
        /// 出生日期 民國年(YYYMMDD)
        /// </summary>
        public string BirthDate { get; set; }

        /// <summary>
        /// 是否免列印相片
        /// 0 = 印
        /// 1 = 免印
        /// </summary>
        public int PicFree { get; set; }

        /// <summary>
        /// 領補換地點
        /// </summary>
        public string IssueLoc { get; set; }

        /// <summary>
        /// 呼叫來源
        /// 1 = APP
        /// 2 = 後台
        /// 3 = 排程
        /// </summary>
        public int Source { get; set; }

        /// <summary>
        /// 若為後台呼叫，傳入後台帳號；否則免傳此值
        /// </summary>
        public string UserName { get; set; }

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