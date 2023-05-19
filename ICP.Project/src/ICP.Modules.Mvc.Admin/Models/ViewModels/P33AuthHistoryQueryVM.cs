using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.ViewModels
{
    public class P33AuthHistoryQueryVM : BaseListModel
    {
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
        /// null : 全選
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

        public List<P33AuthData> P33AuthDatas { get; set; } = new List<P33AuthData>();
    }
    public class P33AuthData
    {
        /// <summary>
        /// 通報種類
        /// </summary>
        public string TYPE { get; set; }
        /// <summary>
        /// 通報單位
        /// </summary>
        public string CRI_PLACE { get; set; }
        /// <summary>
        /// 發生日期 民國年(YYYMMDD)
        /// </summary>
        public string CRI_DATE { get; set; }
        /// <summary>
        /// 通報日期 民國年(YYYMMDD)
        /// </summary>
        public string DOC_DATE { get; set; }
        /// <summary>
        /// 中文戶名
        /// </summary>
        public string CNAME { get; set; }
        /// <summary>
        /// 單據號碼 / 人頭帳戶
        /// </summary>
        public string INVOICE_NO { get; set; }
        /// <summary>
        /// 說明 1
        /// </summary>
        public string REMARK_1 { get; set; }
        /// <summary>
        /// 說明 2
        /// </summary>
        public string REMARK_2 { get; set; }
        /// <summary>
        /// 說明 3
        /// </summary>
        public string REMARK_3 { get; set; }
        /// <summary>
        /// 說明 4
        /// </summary>
        public string REMARK_4 { get; set; }
        /// <summary>
        /// 通報單位中文名稱
        /// </summary>
        public string DOC_NAME { get; set; }
        /// <summary>
        /// 警示帳戶解除代碼
        /// </summary>
        public string REL_CODE { get; set; }
        /// <summary>
        /// 警示帳戶解除原因
        /// </summary>
        public string REL_REASON { get; set; }
        /// <summary>
        /// 警示帳戶解除日期 民國年(YYYMMDD)
        /// </summary>
        public string REL_DATE { get; set; }
        /// <summary>
        /// 保留欄位
        /// </summary>
        public string FILLER { get; set; }
    }
}
