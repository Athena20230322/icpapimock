using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.CustomerSecurityManage
{
    public class TakeCashLimitListModel
    {
        /// <summary>
        /// 編號
        /// </summary>
        public long RowNo { get; set; }

        /// <summary>
        /// 身份證字號/居留證號
        /// </summary>
        public string IDNO { get; set; }

        /// <summary> 鎖定狀態(0:解鎖, 1:封鎖)
        /// </summary>
        public int Status { get; set; }

        /// <summary> 最新解鎖時間
        /// </summary>
        public DateTime? LastUnLockDate { get; set; }

        /// <summary> 最新解鎖人員
        /// </summary>
        public string LastUnLockUser { get; set; }

        /// <summary> 最新解鎖原因
        /// </summary>
        public string LastUnLockMemo { get; set; }

        /// <summary> 最新封鎖時間
        /// </summary>
        public DateTime? LastLockDate { get; set; }

        /// <summary> 最新封鎖人員
        /// </summary>
        public string LastLockUser { get; set; }

        /// <summary> 最新封鎖原因
        /// </summary>
        public string LastLockMemo { get; set; }

        /// <summary> 最新異動時間
        /// </summary>
        public DateTime? LastModifyDate { get; set; }


    }
}
