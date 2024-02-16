using System;
using System.Reflection;

namespace ICP.Batch.AccountLink.Services
{
    using Infrastructure.Abstractions.EmailSender;
    using Infrastructure.Core.Models;   

    public class EMailNotifyService
    {
        IEmailSender _emailSender;

        public EMailNotifyService(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        /// <summary>
        /// 發送錯誤信
        /// </summary>
        public void SendResultEmail(string commandMethod, BaseResult result)
        {
            string msg = $"{commandMethod}: {result.RtnMsg}";

            SendErrorEmail(msg, !result.IsSuccess);
        }

        /// <summary>
        /// 發送錯誤信
        /// </summary>
        public void SendErrorEmail(Exception ex)
        {
            SendErrorEmail(ex.Message + Environment.NewLine + ex.StackTrace);
        }

        /// <summary>
        /// 發送錯誤信
        /// </summary>
        public void SendErrorEmail(string msg, bool isError = true)
        {
            var ThisAssembly = Assembly.GetExecutingAssembly();

            string sNamespace = ThisAssembly.GetName().Name;

            string to = "";

            string title = sNamespace + " " + (isError ? "Error" : "Info");

            string body = string.Format(@"[{0}] {1} at ",
                DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), title
            );

            _emailSender.SendMail(to, title, body, false);
        }
    }
}
