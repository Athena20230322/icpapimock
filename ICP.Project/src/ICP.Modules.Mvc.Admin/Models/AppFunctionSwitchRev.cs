using System;
using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Mvc.Admin.Models
{
    public class AppFunctionSwitchRev
    {
        /// <summary>
        /// 預約記錄編號
        /// </summary>
        public int RevID { get; set; }

        /// <summary>
        /// APP名稱
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// 功能流水號
        /// </summary>
        public int FunctionID { get; set; }

        /// <summary>
        /// 啟用狀態
        /// 0 = 關閉
        /// 1 = 開啟
        /// </summary>
        [Required(ErrorMessage = "請選擇開關狀態")]
        [Range(0, 1)]
        public byte FunctionStatus { get; set; }

        /// <summary>
        /// 起始時間
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 結束時間
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 修改時間
        /// </summary>
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// 最後修改人
        /// </summary>
        public string Modifier { get; set; }
    }
}
