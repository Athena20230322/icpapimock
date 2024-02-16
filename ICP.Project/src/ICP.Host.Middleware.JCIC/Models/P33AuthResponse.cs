using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICP.Host.Middleware.JCIC.Models
{
    public class P33AuthResponse
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
        /// 身分證/居留證號
        /// </summary>
        public string IDNO { get; set; }

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
        /// 2 = 待審
        /// </summary>
        public short IsPass { get; set; }

        /// <summary>
        /// 聯徵回傳的通報案件資料筆數
        /// </summary>
        public int DataCount { get; set; }

        /// <summary>
        /// 聯徵中心回傳的資料列表(JSON)
        /// </summary>
        public string DataList { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// API回應時間
        /// </summary>
        public DateTime? RespTime { get; set; }
    }
}