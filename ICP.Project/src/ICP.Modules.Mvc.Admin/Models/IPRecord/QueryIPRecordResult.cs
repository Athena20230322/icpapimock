using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.IPRecord
{
    public class QueryIPRecordResult : BaseListModel
    {
        /// <summary>
        /// 登入帳號
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 電支帳號
        /// </summary>
        public string ICPMID { get; set; }
        /// <summary>
        /// 登入 IP
        /// </summary>
        public long? RealIP { get; set; }
        /// <summary>
        /// 回應訊息
        /// </summary>
        public string RtnMsg { get; set; }
        /// <summary>
        /// 登入時間
        /// </summary>
        public DateTime? LogTime { get; set; }
        /// <summary>
        /// 手機號碼
        /// </summary>
        public string CellPhone { get; set; }
        /// <summary>
        /// 裝置ID
        /// </summary>
        public string DeviceID { get; set; }
        /// <summary>
        /// 回傳的成功失敗
        /// 1:成功
        /// </summary>
        public int RtnCode { get; set; }
    }
}
