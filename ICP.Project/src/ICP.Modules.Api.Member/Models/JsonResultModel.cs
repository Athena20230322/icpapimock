using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models
{
    public class JsonResultModel
    {
        /// <summary>
        /// 回傳代碼
        /// </summary>
        public string RtnCode { get; set; }
        /// <summary>
        /// 回傳訊息
        /// </summary>
        public string RtnMsg { get; set; }
    }
}
