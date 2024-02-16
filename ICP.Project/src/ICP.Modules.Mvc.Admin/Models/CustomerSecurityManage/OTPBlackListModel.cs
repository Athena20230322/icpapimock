using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.CustomerSecurityManage
{
    public class OTPBlackListModel : BaseListModel
    {

        /// <summary>
        /// 序號
        /// </summary>
        public long RowNo { get; set; }

        /// <summary> 黑名單號碼
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

        /// <summary> 鎖定狀態(0:解鎖, 1:封鎖)
        /// </summary>
        public int Status { get; set; }

        /// <summary> 鎖定類別(0:自動, 1:手動)
        /// </summary>
        public int LockType { get; set; }        

        /// <summary> 最近封鎖時間
        /// </summary>
        public DateTime LockDate { get; set; }

        /// <summary> 鎖定人員ID
        /// </summary>
        public string LockUser { get; set; }

        /// <summary> 鎖定人員名稱
        /// </summary>
        public string LockUserCName { get; set; }

        /// <summary> 最近解鎖時間
        /// </summary>
        public DateTime? UnLockDate { get; set; }

        /// <summary> 解鎖人員
        /// </summary>
        public string UnLockUser { get; set; }

        /// <summary> 最近封鎖原因
        /// </summary>
        public string LockMemo { get; set; }

        /// <summary> 最近解鎖原因
        /// </summary>
        public string UnLockMemo { get; set; }

    }
}
