using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.ManageBank.FirstBank
{
    /// <summary>
    /// 回傳結果
    /// </summary>
    public class B2BResult
    {
        public string System { get; set; }

        /// <summary>
        /// 錯誤代碼
        /// </summary>
        public string StatusCode { get; set; }

        /// <summary>
        /// 錯誤訊息說明
        /// </summary>
        public string StatusDesc { get; set; }
    }
}
