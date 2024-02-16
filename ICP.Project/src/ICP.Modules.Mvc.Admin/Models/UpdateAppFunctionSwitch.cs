using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models
{
    public class UpdateAppFunctionSwitch
    {
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
        [Range(0, 1)]
        public byte Status { get; set; }
    }
}
