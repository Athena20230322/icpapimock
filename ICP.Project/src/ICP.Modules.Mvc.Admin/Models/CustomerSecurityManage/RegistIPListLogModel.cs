using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.CustomerSecurityManage
{
    public class RegistIPListLogModel : BaseListModel
    {
        /// <summary>
        /// 序號
        /// </summary>
        public int RowNo { get; set; }

        /// <summary>
        /// IP位置
        /// </summary>      
        public string IP { get; set; }                

        /// <summary>
        /// 建立時間
        /// </summary>        
        public DateTime CreateDate { get; set; }
                
        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 電支帳號
        /// </summary>
        public string ICPMID { get; set; }

        /// <summary>
        /// IP預警的註冊次數
        /// </summary>
        public long Tcount { get; set; }

        /// <summary>
        /// IP黑名單狀態 0:未在鎖定IP黑名單內 1:已在鎖定IP黑名單內
        /// </summary>
        public int Status { get; set; }
    }
}
