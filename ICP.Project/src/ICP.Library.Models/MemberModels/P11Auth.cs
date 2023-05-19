using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.MemberModels
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
        /// 領補換日期(西元年)
        /// </summary>
        public DateTime IssueDate { get; set; }

        /// <summary>
        /// 領取類別
        /// 1 = 初發
        /// 2 = 補發
        /// 3 = 換發
        /// </summary>
        public byte ObtainType { get; set; }

        /// <summary>
        /// 證上有無照片
        /// 0 = 無
        /// 1 = 有
        /// </summary>
        public byte IsPicture { get; set; }

        /// <summary>
        /// 出生日期(西元年)
        /// </summary>
        public DateTime? BirthDay { get; set; }

        /// <summary>
        /// 發證地點編號
        /// </summary>
        public string IssueLocationID { get; set; }

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
