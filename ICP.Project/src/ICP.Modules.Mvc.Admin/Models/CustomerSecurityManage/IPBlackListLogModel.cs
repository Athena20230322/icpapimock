using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.CustomerSecurityManage
{
    public class IPBlackListLogModel : BaseListModel
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
        /// 建立人員
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>        
        public DateTime CreateDate { get; set; }
                
        /// <summary>
        /// 修改人員
        /// </summary>
        public string Modifier { get; set; }

        /// <summary>
        /// 修改時間
        /// </summary>
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// 狀態 1:封鎖 0:已解鎖
        /// </summary>        
        public int Status { get; set; }
        
        /// <summary>
        /// 鎖定原因
        /// </summary>
        public string LockMemo { get; set; }

        /// <summary>
        /// 解鎖原因
        /// </summary>
        public string UnLockMemo { get; set; }

        public long Sn { get; set; }
    }
}
