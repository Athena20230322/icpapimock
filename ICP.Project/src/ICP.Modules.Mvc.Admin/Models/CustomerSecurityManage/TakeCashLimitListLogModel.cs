using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.CustomerSecurityManage
{
    public class TakeCashLimitListLogModel : BaseListModel
    {
        /// <summary>
        /// 編號
        /// </summary>
        public long RowNo { get; set; }

        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 身份證字號/居留證號
        /// </summary>
        public string IDNO { get; set; }

        /// <summary>
        /// 手機號碼
        /// </summary>
        public string CellPhone { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary> 鎖定狀態(0:解鎖, 1:封鎖)
        /// </summary>
        public int Status { get; set; }

        /// <summary> 解鎖時間
        /// </summary>
        public DateTime? UnLockDate { get; set; }

        /// <summary> 解鎖人員
        /// </summary>
        public string UnLockUser { get; set; }

        /// <summary> 解鎖原因
        /// </summary>
        public string UnLockMemo { get; set; }

        /// <summary> 封鎖時間
        /// </summary>
        public DateTime LockDate { get; set; }

        /// <summary> 封鎖人員
        /// </summary>
        public string LockUser { get; set; }

        /// <summary> 封鎖原因
        /// </summary>
        public string LockMemo { get; set; }

        /// <summary> 建立時間
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 建立人員
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// 電支帳號
        /// </summary>
        public string ICPMID { get; set; }

        /// <summary>
        /// 會員姓名
        /// </summary>
        public string CName { get; set; }

    }
}
