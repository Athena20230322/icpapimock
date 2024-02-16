using System;

namespace ICP.Batch.MailSendReSend.Commands
{
    using ICP.Infrastructure.Abstractions.EmailSender;
    using ICP.Library.Models.MailLibrary;
    using Infrastructure.Abstractions.Logging;
    using Library.Services.MailLibrary;
    using Service;


    public class ProgramCommand
    {
        ILogger<ProgramCommand> _logger;
        MailSendService _mailSendService;
        ReSendMailService _reSendMailService;
        private readonly IEmailSender _emailSender = null;

        public ProgramCommand(
          ILogger<ProgramCommand> logger,
          MailSendService mailSendService,
          ReSendMailService reSendMailService,
          IEmailSender emailSender
            )
        {
            _logger = logger;
            _mailSendService = mailSendService;
            _reSendMailService = reSendMailService;
            _emailSender = emailSender;
        }

        public void Exec()
        {

            ExecReSendMail(ReSendMail);

        }

        /// <summary>
        /// 委派使用TryCatch
        /// </summary>
        /// <param name="action"></param>
        private void ExecReSendMail(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                _logger.Warning(ex.ToString(), "Mail重送錯誤");
            }

        }

        /// <summary>
        /// 重送Mail
        /// </summary>
        private void ReSendMail()
        {
            _logger.Info("Mail重送開始");
            var list = _reSendMailService.ReSendMail();

            _logger.Info("重送Mail {0} 筆", list.Count);

            int fail = 0;
            int success = 0;

            list.ForEach(to =>
            {
                if (_emailSender.SendMailList(to.MailTo, to.Subject, to.Body, to.SMTPIP ,to.MailFrom, true, to.Sbcc, to.Scc))
                {
                    _mailSendService.UpdateSendMailDate(to.MailID, (int)MailSendType.Status.Send);
                    success++;
                }
                else fail++;

            });
            
            _logger.Info("失敗Mail {0} 筆", fail);
            _logger.Info("成功Mail {0} 筆", success);
            _logger.Info("Mail重送結束");
        }
    }


}
