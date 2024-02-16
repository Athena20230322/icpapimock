using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.MemberModels
{
    public class P11AuthHistoryQuery : BaseListModel
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
        /// 身分證號
        /// </summary>
        public string IDNO { get; set; }
        /// <summary>
        /// 領取類別
        /// 1 = 初領
        /// 2 = 補領
        /// 3 = 換領
        /// </summary>
        public short IssueType { get; set; }
        /// <summary>
        /// 領補換日期(民國YYYMMdd)
        /// </summary>
        public string IssueDate { get; set; }
        /// <summary>
        /// 領補換地點ID
        /// </summary>
        public string IssueLoc { get; set; }
        /// <summary>
        /// 出生日期(民國年YYYMMDD)
        /// </summary>
        public string BirthDate { get; set; }
        /// <summary>
        /// 是否免列印相片
        /// 0:印
        /// 1:不印
        /// </summary>
        public short PicFree { get; set; }
        /// <summary>
        /// 是否通過驗證
        /// 0:未通過
        /// 1:通過
        /// 2:待審
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
    }
}
