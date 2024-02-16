using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models
{
    public class FunctionCategoryStatusRev
    {
        /// <summary>
        /// 預約紀錄編號
        /// </summary>
        public int RevID { get; set; }

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
