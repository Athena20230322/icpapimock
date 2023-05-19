using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Abstractions.EmailSender
{
    public interface IEmailSender
    {
        bool SendMail(string to, string title, string body, bool isHtml = true);

        bool SendMailList(List<string> toList, string title, string body, string SMTP ="",string MailFrom="", bool isHtml = true, List<string> Sbcc = null, List<string> Scc = null);
    }
}
