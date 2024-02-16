using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.CustomerSecurityManage
{
    public class OTPBlackListLogModel : BaseListModel
    {
        /// <summary>
        /// 序號
        /// </summary>
        public int RowNo { get; set; }

        /// <summary>
        /// 手機號碼
        /// </summary>      
        public string CellPhone { get; set; }

        /// <summary>
        /// 身份證字號/居留證
        /// </summary>      
        public string IDNO { get; set; }

        /// <summary>
        /// Email
        /// </summary>      
        public string Email { get; set; }

        /// <summary>
        /// 鎖定人員
        /// </summary>
        public string LockUser { get; set; }

        /// <summary>
        /// 鎖定時間
        /// </summary>        
        public DateTime LockDate { get; set; }
                
        /// <summary>
        /// 修改人員
        /// </summary>
        public string UnLockUser { get; set; }

        /// <summary>
        /// 修改時間
        /// </summary>
        public DateTime? UnLockDate { get; set; }

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
     
    }
}
