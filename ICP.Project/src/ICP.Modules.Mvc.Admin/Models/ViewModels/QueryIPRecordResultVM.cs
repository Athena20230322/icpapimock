using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.ViewModels
{
    public class QueryIPRecordResultVM : BaseListModel
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
        public string RealIP { get; set; }
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
    }
}
