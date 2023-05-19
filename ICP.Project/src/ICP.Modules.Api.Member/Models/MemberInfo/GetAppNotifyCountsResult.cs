using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    public class GetAppNotifyCountsResult
    {
        /// <summary>
        /// 訊息中心未讀
        /// </summary>
        public int Counts { get; set; }

        /// <summary>
        /// 總未讀數
        /// </summary>
        public int TotalUnread { get; set; }
    }
}
