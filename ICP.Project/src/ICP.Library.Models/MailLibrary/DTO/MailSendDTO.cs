using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.MailLibrary
{
    public class MailSendDTO : MailSendContent
    {
        /// <summary>
        /// 收件者清單
        /// </summary>
        public new List<string> MailTo { get; set; }
        /// <summary>
        /// 副本清單
        /// </summary>
        public new List<string> Scc { get; set; }
        /// <summary>
        /// 密件副本清單
        /// </summary>
        public new List<string> Sbcc { get; set; }
    }
}
