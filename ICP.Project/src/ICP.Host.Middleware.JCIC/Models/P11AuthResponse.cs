using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICP.Host.Middleware.JCIC.Models
{
    public class P11AuthResponse
    {
        /// <summary>
        /// Log 流水號
        /// </summary>
        public long LogID { get; set; }

        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 身分證字號
        /// </summary>
        public string IDNO { get; set; }

        /// <summary>
        /// 領補換日期(民國年YYYMMDD)
        /// </summary>
        public string IssueDate { get; set; }

        /// <summary>
        /// 領補換代號
        /// 1 = 初領
        /// 2 = 補領
        /// 3 = 換領
        /// </summary>
        public byte IssueType { get; set; }

        /// <summary>
        /// 出生日期(民國年YYYMMDD)
        /// </summary>
        public string BirthDate { get; set; }

        /// <summary>
        /// 是否免列印相片
        /// 0 = 印
        /// 1 = 不印
        /// </summary>
        public byte PicFree { get; set; }

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
        public byte Source { get; set; }

        /// <summary>
        /// 後台帳號
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Real IP
        /// </summary>
        public long RealIP { get; set; }

        /// <summary>
        /// Proxy IP
        /// </summary>
        public long ProxyIP { get; set; }

        /// <summary>
        /// 呼叫端Server Real IP
        /// </summary>
        public long ServerRealIP { get; set; }

        /// <summary>
        /// 呼叫端Server Proxy IP
        /// </summary>
        public long ServerProxyIP { get; set; }

        /// <summary>
        /// API查詢結果代碼
        /// </summary>
        public int Return_Code { get; set; }

        /// <summary>
        /// API查詢結果訊息
        /// </summary>
        public string Return_Msg { get; set; }

        /// <summary>
        /// 聯徵Web Service回傳結果
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 聯徵Web Service回傳訊息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 聯徵登入後取得的SessionID
        /// </summary>
        public string SessionID { get; set; }

        /// <summary>
        /// 是否通過驗證
        /// 0 = 否
        /// 1 = 是
        /// </summary>
        public short IsPass { get; set; }

        /// <summary>
        /// 驗證結果代號
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        /// 驗證結果說明
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// API回應時間
        /// </summary>
        public DateTime RespTime { get; set; }
    }
}