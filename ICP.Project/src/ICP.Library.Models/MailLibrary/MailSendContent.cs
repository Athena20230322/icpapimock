using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.MailLibrary
{   /// <summary>
    /// 寄送SMTP信件
    /// </summary>
    public class MailSendContent
    {   
        /// <summary>
        /// 流水號
        /// </summary>
        public int MailID { get; set; }
        /// <summary>
        /// 會員編號
        /// </summary>
        public int MID { get; set; }
        /// <summary>
        /// 寄件者
        /// </summary>
        public string MailFrom { get; set; }
        /// <summary>
        /// 收件者
        /// </summary>
        public string MailTo { get; set; }
        /// <summary>
        /// 副本
        /// </summary>
        public string Scc { get; set; }
        /// <summary>
        /// 密件副本
        /// </summary>
        public string Sbcc { get; set; }
        /// <summary>
        /// SMTP IP
        /// </summary>
        public string SMTPIP { get; set; }
        /// <summary>
        /// 發送來源 0: 預設 1: Admin  2: Member  3: Payment 4: PaymentCenter 5: MiddleWare
        /// </summary>
        public int Source { get; set; }
        /// <summary>
        /// 是否為錯誤信  0:否 1:是
        /// </summary>
        public bool ErrorMail { get; set; }
        /// <summary>
        /// 信件主旨
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// 信件主體
        /// </summary>
        public string Body { get; set; }
    }
}
