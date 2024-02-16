using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.MemberModels
{
    public class P33AuthHistoryQuery : BaseListModel
    {
        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }
        /// <summary>
        /// ICASH 電支帳號
        /// </summary>
        public string ICPMID { get; set; }
        /// <summary>
        /// 驗證時間
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 身分證號/統一編號
        /// </summary>
        public string IDNO { get; set; }
        /// <summary>
        /// 是否通過驗證
        /// 0:未通過
        /// 1:通過
        /// 2:待審
        /// null : 未知狀態(預設)
        /// </summary>
        public short? IsPass { get; set; }
        /// <summary>
        /// 驗證結果代號
        /// </summary>
        public string Answer { get; set; }
        /// <summary>
        /// 驗證結果說明
        /// </summary>
        public string Result { get; set; }
        /// <summary>
        /// 呼叫來源
        /// 1 : App
        /// 2 : 後台
        /// 3 : 排程
        /// </summary>
        public short Source { get; set; }
        /// <summary>
        /// 後台帳號(若呼叫來源為後台的時候此欄位才會帶值)
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 聯徵回傳的通報案件資料筆數
        /// </summary>
        public int DataCount { get; set; }
        /// <summary>
        /// 聯徵中心回傳的資料列表(JSON)
        /// </summary>
        public string DataList { get; set; }
    }
}
