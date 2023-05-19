using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.CustomerManager
{
    public class MemberNotifyMessageDetailModel: MemberNotifyMessageModel
    {
        /// <summary>
        /// 訊息內文
        /// </summary>
        public string Body { get; set; }
    }
}
