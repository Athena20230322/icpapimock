using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.AdminModels
{
    public class AppFunctionSwitch
    {
        /// <summary>
        /// APP名稱
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// 功能流水號
        /// </summary>
        public byte FunctionID { get; set; }

        /// <summary>
        /// 功能代碼
        /// </summary>
        public string FunctionCode { get; set; }

        /// <summary>
        /// 功能名稱
        /// </summary>
        public string FunctionName { get; set; }

        /// <summary>
        /// 啟用狀態
        /// 0 = 關閉
        /// 1 = 開啟
        /// </summary>
        public byte Status { get; set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 最後修改日期
        /// </summary>
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// 最後修改人
        /// </summary>
        public string Modifier { get; set; }
    }
}
