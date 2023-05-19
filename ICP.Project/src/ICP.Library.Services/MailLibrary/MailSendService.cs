using System;
using System.Collections.Generic;
using ICP.Infrastructure.Abstractions.Logging;

namespace ICP.Library.Services.MailLibrary
{
    using ICP.Infrastructure.Abstractions.EmailSender;
    using Infrastructure.Core.Extensions;
    using Infrastructure.Core.Models;
    using Library.Models.MailLibrary;
    using Library.Repositories.MailLibrary;
    using Library.Repositories.SystemRepositories;

    public class MailSendService
    {
        private readonly MailSendRepository _mailSendRepository = null;
        private readonly GlobalAppSetting _globalAppSetting = null;
        private readonly IEmailSender _emailSender = null;
        private readonly MailManageService _mailManageService = null;
        private readonly ConfigKeyValueRepository _configKeyValueRepository;
        private ILogger<MailSendService> _logger;

        public MailSendService(
            MailSendRepository mailSendRepository,
            IEmailSender emailSender,
            MailManageService mailManageService,
            GlobalAppSetting globalAppSetting,
            ConfigKeyValueRepository configKeyValueRepository, 
            ILogger<MailSendService> logger)
        {

            _mailSendRepository = mailSendRepository;
            
            _emailSender = emailSender;
            _mailManageService = mailManageService;
            _globalAppSetting = globalAppSetting;
            _configKeyValueRepository = configKeyValueRepository;
            _logger = logger;
        }

        #region 公開

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
        public BaseResult SendMail(List<string> MailTo, string Subject, string MailFrom=null,string Body = null, int Source = 0, string MailKey = null, string Args = null, List<string> Scc = null, List<string> Sbcc = null)
        {
            MailSendDTO model = new MailSendDTO();
            BaseResult result= new  BaseResult();

            if (MailKey != null && Args != null)
            {
                var content = _mailManageService.Generate(MailKey, Args);
                Body = content.Body;
            }

            model.MailFrom = MailFrom ?? _configKeyValueRepository.SMTP_DefaultMailFrom;
            model.MailTo = MailTo;
            model.Subject = Subject;
            model.Body = Body;
            model.MID = _globalAppSetting.UserID != null ? unchecked((int)_globalAppSetting.UserID) : 0;
            model.Sbcc = Sbcc;
            model.SMTPIP = _configKeyValueRepository.SMTP_IP;
            model.Source = Source;
            model.ErrorMail = false;

            var checkMail = _mailSendRepository.CheckMailResult(model);

            if (checkMail.IsSuccess) return LetterMail(model);

            
            result.SetError();
            result.RtnMsg = checkMail.RtnMsg;
            _logger.Error(result.RtnMsg);
            return result;
        }

        /// <summary>
        /// 寄錯誤信
        /// </summary>
        /// <param name="Subject">主旨</param>
        /// <param name="Body">內文</param>
        /// <param name="Source">發送來源 0: 預設 1: Admin  2: Member  3: Payment 4: PaymentCenter 5: MiddleWare</param>
        /// <returns></returns>
        public BaseResult SendErrorMail(string Subject, string Body, int Source = 0)
        {

            MailSendDTO model = new MailSendDTO();
            var MailList = new List<string>();
            BaseResult result= new  BaseResult();

            var args = new
            {
                content = Body
            };
            string ErrMailKey = "common";

            var content = _mailManageService.Generate(ErrMailKey, args);

            MailList.Add(_configKeyValueRepository.SMTP_ErrMailAddress);
            model.SMTPIP = _configKeyValueRepository.SMTP_IP;
            model.MailFrom = _configKeyValueRepository.SMTP_ErrorMailFrom;

            model.MailTo = MailList;
            model.Subject = Subject;
            model.Body = content.Body;
            model.MID = _globalAppSetting.UserID != null ? unchecked((int)_globalAppSetting.UserID) : 0;
            
            model.Source = Source;
            model.ErrorMail = true;

            var checkMail = _mailSendRepository.CheckMailResult(model);

            if (checkMail.IsSuccess) return LetterMail(model);

            
            result.SetError();
            result.RtnMsg = checkMail.RtnMsg;
            _logger.Error(result.RtnMsg);
            return result;

        }

        /// <summary>
        /// 更新郵件發送時間與狀態
        /// </summary>
        /// <param name="MailID"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public BaseResult UpdateSendMailDate(int MailID, int Status)
        {
            return _mailSendRepository.UpdateSendMailDate(MailID, Status);
        }
        #endregion

        #region 不公開

        /// <summary>
        /// 錯誤信與普通信分信
        /// </summary>
        private BaseResult LetterMail(MailSendDTO model)
        {
            MailSendRepository.ResultMailModel resultMail = new MailSendRepository.ResultMailModel();
            BaseResult result = new BaseResult();
            result.SetError();

            try
            {
                if (model.ErrorMail)
                {
                    if (_emailSender.SendMailList(model.MailTo, model.Subject, model.Body, model.SMTPIP, model.MailFrom, true, model.Sbcc, model.Scc))
                    {
                        resultMail = _mailSendRepository.AddBodyMail(model);
                        UpdateSendMailDate(resultMail.MailID, (int)MailSendType.Status.Send);
                        result.SetSuccess();
                    }
                    else
                    {
                        resultMail = _mailSendRepository.AddBodyMail(model);
                        UpdateSendMailDate(resultMail.MailID, (int)MailSendType.Status.SendErr);
                    }

                }
                else
                {
                    resultMail = _mailSendRepository.AddBodyMail(model);
                    if (_emailSender.SendMailList(model.MailTo, model.Subject, model.Body, model.SMTPIP, model.MailFrom, true, model.Sbcc, model.Scc))
                    {
                        UpdateSendMailDate(resultMail.MailID, (int)MailSendType.Status.Send);
                        result.SetSuccess();
                    }
                    else
                    {
                        result = UpdateSendMailDate(resultMail.MailID, (int)MailSendType.Status.SendErr);
                    }
                }
            }
            catch (Exception e)
            {
                
                result.SetError();
                result.RtnMsg = "SMTP送信錯誤";
            }
            return result;
        }
        /* /// <summary>
         /// 送出SMTP郵件
         /// </summary>
         /// <param name="model"></param>
         private bool SubmitSMTPEmail(MailSendDTO model)
         {
             BaseResult result = new BaseResult();
             System.Net.Mail.MailMessage MailClient = new System.Net.Mail.MailMessage();
             MailClient.From = new System.Net.Mail.MailAddress(model.MailFrom);

             model.MailTo.ForEach(m => MailClient.To.Add(m));//設定收件者Email(List)

             if (model.Sbcc != null)
             {
                 model.Sbcc.ForEach(sb => MailClient.Bcc.Add(sb));//加入密件副本的Mail
             }

             if (model.Scc != null)
             {
                 model.Scc.ForEach(sc => MailClient.CC.Add(sc));//加入副本的Maill
             }


             MailClient.Subject = model.Subject;//設定信件主旨
             MailClient.Body = model.Body; //設定信件內容
             MailClient.IsBodyHtml = true; //是否使用html格式
             System.Net.Mail.SmtpClient MySMTP = new System.Net.Mail.SmtpClient(model.SMTPIP, _configKeyValueRepository.SMTP_Port);

             //是否帶入SMTP帳號密碼
             if (_configKeyValueRepository.SMTP_User != string.Empty)
             {
                 MySMTP.Credentials = new System.Net.NetworkCredential(_configKeyValueRepository.SMTP_User, _configKeyValueRepository.SMTP_Password);
             }

             try
             {
                 MySMTP.Send(MailClient);
                 MailClient.Dispose(); //釋放資源
                 return true;
             }
             catch (Exception ex)
             {
                 _logger.Warning(ex, "Mail發送失敗");
                 return false;
             }
         }*/

        #endregion
    }
}
