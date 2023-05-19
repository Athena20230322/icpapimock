using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.CustomerServiceRecord
{
    public class QueryCustomServiceRecordResult : BaseListModel
    {
        /// <summary>
        /// PK 紀錄編號流水號
        /// </summary>
        public long CustomerServiceID { get; set; }
        /// <summary>
        /// 案件編號(西元年末2碼+MM+DD+流水號3碼)
        /// </summary>
        public string CaseNo { get; set; }
        /// <summary>
        /// 問題類別 ID 關聯至 客服記錄查詢設定檔
        /// </summary>
        public byte Type { get; set; }
        /// <summary>
        /// 進線管道 ID 關聯至 客服記錄查詢設定檔
        /// </summary>
        public byte GateWay { get; set; }
        /// <summary>
        /// 問題類別描述
        /// </summary>
        public string TypeDescription { get; set; }
        /// <summary>
        /// 進線管道描述
        /// </summary>
        public string GateWayDescription { get; set; }
        /// <summary>
        /// 回報者姓名
        /// </summary>
        public string Cname { get; set; }
        /// <summary>
        /// 手機號碼
        /// </summary>
        public string CellPhone { get; set; }
        /// <summary>
        /// 電支帳號
        /// </summary>
        public string ICPMID { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 訂單編號
        /// </summary>
        public string TradeNo { get; set; }
        /// <summary>
        /// 紀錄建立時間
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 建立人員
        /// </summary>
        public string CreateUser { get; set; }
        /// <summary>
        /// 修改者
        /// </summary>
        public string Modifier { get; set; }
        /// <summary>
        /// 修改時間
        /// </summary>
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 案件進度 : 0建立案件 1 客服處理 2客服更改處理結果
        /// </summary>
        public byte Status { get; set; }
        /// <summary>
        /// 紀錄內容(最後紀錄內容)
        /// </summary>
        public string Note { get; set; }

        public List<CustomerServiceRecordNote> Notes { get; set; } = new List<CustomerServiceRecordNote>();
    }
}
