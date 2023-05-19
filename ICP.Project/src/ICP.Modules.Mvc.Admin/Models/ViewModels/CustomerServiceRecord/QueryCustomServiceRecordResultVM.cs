using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.ViewModels.CustomerServiceRecord
{
    public class QueryCustomServiceRecordResultVM : BaseListModel
    {
        /// <summary>
        /// PK 紀錄編號流水號
        /// </summary>
        public long CustomerServiceID { get; set; }
        /// <summary>
        /// 案件編號
        /// </summary>
        public string CaseNo { get; set; }
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
        public string CName { get; set; }
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
        /// 案件進度名稱
        /// </summary>
        public string StatusName { get; set; }
        /// <summary>
        /// 案件紀錄Log
        /// </summary>
        public List<CustomerServiceRecordNoteVM> Notes { get; set; } = new List<CustomerServiceRecordNoteVM>();
    }
}
