using ICP.Infrastructure.Abstractions.EmailSender;
using ICP.Infrastructure.Abstractions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace ICP.Infrastructure.Core.Frameworks.EmailSender
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly SmtpClient _smtpClient = null;
        private readonly string _from = null;
        private readonly ILogger _logger = null;

        public SmtpEmailSender(string from, SmtpClient smtpClient, ILogger<SmtpEmailSender> logger)
        {
            _from = from;
            _smtpClient = smtpClient;
            _logger = logger;
        }

        public bool SendMailList(List<string> toList, string title, string body, string SMTP = "", string MailFrom = "", bool isHtml = true, List<string> Sbcc = null, List<string> Scc = null)
        {

            var mailMessage = new MailMessage
            {
                Sender = new MailAddress(MailFrom != string.Empty ? MailFrom : _from),
                From = new MailAddress(MailFrom != string.Empty ? MailFrom : _from),
                IsBodyHtml = isHtml,
                Subject = title,
                Body = body
            };

            toList.ForEach(to => mailMessage.To.Add(new MailAddress(to)));

            _smtpClient.Host = SMTP == string.Empty ? _smtpClient.Host : SMTP;

            if (Sbcc != null)
            {
                Sbcc.ForEach(sbcc => mailMessage.CC.Add(new MailAddress(sbcc)));
            }

            if (Scc != null)
            {
                Scc.ForEach(scc => mailMessage.Bcc.Add(new MailAddress(scc)));
            }

            try
            {

                _smtpClient.Send(mailMessage);

                return true;
            }
            catch (Exception ex)
            {
                string json = JsonConvert.SerializeObject(new
                {
                    ex.Message,
                    toList,
                    title,
                    body
                }, CustomJsonSerializerSettings.IgnoreException);

                _logger.Error(ex, json);
                return false;
            }
            finally
            {
                mailMessage.Dispose();
                mailMessage = null;
            }
        }
        public bool SendMail(string to, string title, string body, bool isHtml = true)
        {
            var mailMessage = new MailMessage
            {
                Sender = new MailAddress(_from),
                From = new MailAddress(_from),
                IsBodyHtml = isHtml,
                Subject = title,
                Body = body
            };

            mailMessage.To.Add(new MailAddress(to));

            try
            {
#if !DEBUG

                _smtpClient.Send(mailMessage);

#endif
                return true;
            }
            catch (Exception ex)
            {
                string json = JsonConvert.SerializeObject(new
                {
                    ex.Message,
                    to,
                    title,
                    body
                }, CustomJsonSerializerSettings.IgnoreException);

                _logger.Error(ex, json);
                return false;
            }
            finally
            {
                if (_smtpClient != null)
                {
                    _smtpClient.Dispose();
                }

                mailMessage.Dispose();
                mailMessage = null;
            }
        }
    }
}
