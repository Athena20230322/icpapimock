using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICP.Host.Middleware.Mail.Commands
{
    using ICP.Library.Services.MailLibrary;
    using Infrastructure.Core.Models;
    public class MailCommand
    {
        MailSendService _mailSendService;
        public MailCommand(MailSendService mailSendService)
        {
            _mailSendService = mailSendService;
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
        /// <param name="args">參數 (string)</param>
        /// <param name="Scc">副本</param>
        /// <param name="Sbcc">密件副本</param>
        /// <returns></returns>
        public BaseResult SendMail(List<string> MailTo, string Subject,string MailFrom=null, string Body = null, int Source = 0, string MailKey = null, string args = null, List<string> Scc = null, List<string> Sbcc = null)
        {
            return _mailSendService.SendMail(MailTo, Subject,MailFrom, Body, Source, MailKey, args, Scc, Sbcc);
        }

        public BaseResult SendErrMail(string Subject, string Body, int Source = 0)
        {
            return _mailSendService.SendErrorMail( Subject,  Body,  Source = 0);
        }

    }
}