using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    public class ListNotifyMessageItem
    {
        /// <summary>
        /// 訊息ID
        /// </summary>
        public long MsgID { get; set; }

        /// <summary>
        /// 訊息公告類別
        /// </summary>
        public string MsgType { get; set; }
        /// <summary>
        /// 訊息標題
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// 訊息建立日期
        /// 格式：2019/01/01 12:30
        /// </summary>
        public string CreateDate { get; set; }
        /// <summary>
        /// 0:未讀 1:已讀
        /// </summary>
        public byte isRead { get; set; }
    }
}
