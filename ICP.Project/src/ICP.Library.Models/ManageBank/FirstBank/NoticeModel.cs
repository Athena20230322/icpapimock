using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.ManageBank.FirstBank
{
    /// <summary>
    /// 通知內容
    /// </summary>
    public class NoticeModel
    {
        /// <summary>
        /// 通知方式
        /// </summary>
        public string NoticeMethod { get; set; }

        /// <summary>
        /// 受通知傳真號碼
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// 受通知eMail
        /// </summary>
        public string EMail { get; set; }
    }
}
