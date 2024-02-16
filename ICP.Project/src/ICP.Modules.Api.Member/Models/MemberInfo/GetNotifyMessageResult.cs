using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    public class GetNotifyMessageResult
    {
        /// <summary>
        /// 標題
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 內容
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 建立時間 yyyy/MM/dd HH:mm:ss
        /// </summary>
        public string CreateDate { get; set; }
    }
}
