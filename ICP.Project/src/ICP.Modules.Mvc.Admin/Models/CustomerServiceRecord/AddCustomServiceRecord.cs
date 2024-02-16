using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.CustomerServiceRecord
{
    public class AddCustomServiceRecord
    {
        /// <summary>
        /// 問題類別 ID 關聯至 客服記錄查詢設定檔
        /// </summary>
        public byte Type { get; set; }
        /// <summary>
        /// 進線管道 ID 關聯至 客服記錄查詢設定檔
        /// </summary>
        public byte GateWay { get; set; }
        /// <summary>
        /// 案件進度 : 0建立案件 1 客服處理 2客服更改處理結果
        /// </summary>
        public byte Status { get; set; } = 0;
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
        /// 紀錄內容
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// 建立人員
        /// </summary>
        public string CreateUser { get; set; }
        /// <summary>
        /// RealIP
        /// </summary>
        public long RealIP { get; set; } = 0;
        /// <summary>
        /// ProxyIP
        /// </summary>
        public long ProxyIP { get; set; } = 0;
    }
}
