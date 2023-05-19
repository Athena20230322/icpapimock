using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace ICP.Host.Middleware.Mail.Api
{
    using Commands;
    using CommonServiceLocator;
    using ICP.Infrastructure.Core.Models;

    /// <summary>
    ///Mail 的摘要描述
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
    // [System.Web.Script.Services.ScriptService]
    public class Mail : System.Web.Services.WebService
    {
        private readonly MailCommand _mailCommand = null;

        public Mail()
        {
            _mailCommand = ServiceLocator.Current.GetInstance<MailCommand>();
        }

        /// <summary>
        /// 寄普通信
        /// </summary>
        /// <param name="MailFrom">寄件者</param>
        /// <param name="MailTo">收信者</param>
        /// <param name="Subject">主旨</param>
        /// <param name="Body">內文</param>
        /// <param name="Source">發送來源 0: 預設 1: Admin  2: Member  3: Payment 4: PaymentCenter 5: MiddleWare</param>
        /// <param name="MailKey">代碼 {Host}_{事件描述}</param>
        /// <param name="Args">參數 (string)</param>
        /// <param name="Scc">副本</param>
        /// <param name="Sbcc">密件副本</param>
        /// <returns></returns>
        [WebMethod]
        public BaseResult SendMail(List<string> MailTo, string Subject,string MailFrom=null, string Body = null, int Source = 0, string mailKey = null, string args = null, List<string> Scc = null, List<string> Sbcc = null, string SMTPIP = null)
        {
            return _mailCommand.SendMail( MailTo, Subject, MailFrom,Body,Source, mailKey, args, Scc, Sbcc);
        }

        [WebMethod]
        public BaseResult SendErrMail(string Subject, string Body, int Source = 0)
        {
            return _mailCommand.SendErrMail(Subject, Body, Source = 0);
        }
    }
}
