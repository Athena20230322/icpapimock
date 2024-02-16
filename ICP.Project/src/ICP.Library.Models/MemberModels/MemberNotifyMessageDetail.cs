using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.MemberModels
{
    public class MemberNotifyMessageDetail: MemberNotifyMessage
    {
        /// <summary>
        /// 訊息內文
        /// </summary>
        public string Body { get; set; }
    }
}
