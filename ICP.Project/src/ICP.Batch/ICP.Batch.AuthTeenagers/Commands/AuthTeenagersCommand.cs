using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.AuthTeenagers.Commands
{
    using Infrastructure.Abstractions.Logging;
    using Infrastructure.Core.Models;
    using Infrastructure.Core.Extensions;
    using Library.Services.MemberServices;
    using Library.Services.SMSLibrary;
    using Library.Models.MemberModels;
    using Services;

    public class AuthTeenagersCommand
    {
        ILogger<AuthTeenagersCommand> _logger;
        EMailNotifyService _eMailNotifyService;
        LibMemberInfoService _libMemberInfoService;
        LibMemberAuthService _libMemberAuthService;
        LibPersonalAuthService _libPersonalAuthService;
        SMSNotifyService _sMSNotifyService;
        SMSService _smsService;
        AuthTeenagersService _authTeenagersService;

        public AuthTeenagersCommand(
            ILogger<AuthTeenagersCommand> logger,
            EMailNotifyService eMailNotifyService,
            LibMemberInfoService libMemberInfoService,
            LibMemberAuthService libMemberAuthService,
            LibPersonalAuthService libPersonalAuthService,
            SMSNotifyService sMSNotifyService,
            AuthTeenagersService authTeenagersService, 
            SMSService smsService)
        {
            _logger = logger;
            _eMailNotifyService = eMailNotifyService;
            _libMemberInfoService = libMemberInfoService;
            _libMemberAuthService = libMemberAuthService;
            _libPersonalAuthService = libPersonalAuthService;
            _smsService = smsService;
            _authTeenagersService = authTeenagersService;
            _smsService = smsService;
        }

        public void exec()
        {
            execWithTryCatch(AuthTeenagers_Verify);
        }

        /// <summary>
        /// 使用 try_catch 執行
        /// </summary>
        /// <param name="action"></param>
        private void execWithTryCatch(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                _eMailNotifyService.SendErrorEmail(ex);
            }
        }

        /// <summary>
        /// 重送驗證及更新原未成年為成年
        /// </summary>
        private void AuthTeenagers_Verify()
        {
            string commandMethod = "AuthTeenagers_Verify";
            var errorResults = new List<BaseResult>();
            var result = new BaseResult();
            result.SetError();

            _logger.Info("未成年會員身分驗證 Begin");

            var list = _authTeenagersService.ListAuthTeenagersData();

            int failure = 0;
            int success = 0;

            foreach (var item in list)
            {
                var updateResult = _authTeenagersService.UpdateMemberType(item.MID);
                if (updateResult.IsSuccess)
                    success++;
                else
                    failure++;

                var delResult = _authTeenagersService.DeleteAuthTeenagers(item.MID);
                if (updateResult.IsSuccess)
                    success++;
                else
                    failure++;

                if (item.IDNOStatus == 0)
                {
                    #region 判斷身分證是否重複
                    var checkRepeatResult = _libMemberAuthService.CheckIdnoRepeat(item.IDNO, item.MID, IsOversea: false);
                    if (checkRepeatResult.Repeat)
                    {
                        _logger.Warning($"{item.MID}: {item.IDNO} 身分證重複");
                        failure++;
                        continue;
                    }
                    #endregion

                    #region P33 驗證
                    var p33Auth = new P33Auth
                    {
                        MID = item.MID,
                        IDNO = item.IDNO,
                        Source = 1,
                        RealIP = 0,
                        ProxyIP = 0
                    };
                    var p33AuthResult = _libPersonalAuthService.AddAuthP33(p33Auth);
                    if (!p33AuthResult.IsSuccess)
                    {
                        p33AuthResult.RtnMsg = $"{item.MID}: {p33AuthResult.RtnMsg}";

                        _logger.Error(p33AuthResult.RtnMsg);
                        errorResults.Add(p33AuthResult);
                        continue;
                    }
                    #endregion

                    #region P11 驗證
                    var p11Auth = new P11Auth
                    {
                        MID = item.MID,
                        IDNO = item.IDNO,
                        IssueDate = item.IssueDate,
                        ObtainType = item.ObtainType,
                        IsPicture = 1,
                        BirthDay = item.Birthday,
                        IssueLocationID = item.IssueLocationID,
                        Source = 1,
                        RealIP = 0,
                        ProxyIP = 0
                    };
                    var p11AuthResult = _libPersonalAuthService.AddAuthP11(p11Auth);
                    if (!p11AuthResult.IsSuccess)
                    {
                        p11AuthResult.RtnMsg = $"{item.MID}: {p11AuthResult.RtnMsg}";

                        SendP11FailedSMS(item.MID);

                        _logger.Error(p11AuthResult.RtnMsg);
                        errorResults.Add(p11AuthResult);
                        continue;
                    }
                    #endregion

                    SendSuccessSMS(item.MID);
                }
            }

            _logger.Info("未成年會員身分驗證 End");

            if (errorResults.Any())
            {
                #region EMAIL 錯誤結果
                string errMsg = string.Empty;

                errorResults.ForEach(t =>
                {
                    errMsg += $"RtnCode: {t.RtnCode}, RtnMsg: {t.RtnMsg}" + Environment.NewLine;
                });

                result.RtnMsg = errMsg;

                _eMailNotifyService.SendResultEmail(commandMethod, result);
                #endregion
            }
        }

        #region 簡訊通知
        /// <summary>
        /// 發送驗證成功簡訊
        /// </summary>
        /// <param name="MID"></param>
        private void SendSuccessSMS(long MID)
        {
            var memberData = _libMemberInfoService.GetMemberData(MID);

            if (string.IsNullOrWhiteSpace(memberData.detail.CellPhone)) return;

            string msg = "法定代理人皆已核准您的icash Pay註冊申請，您可使用申請時設定的帳號密碼登入。";

            _smsService.SendSMS(memberData.detail.CellPhone, msg, 0);
        }

        /// <summary>
        /// 發送P11驗證失敗簡訊
        /// </summary>
        /// <param name="MID"></param>
        private void SendP11FailedSMS(long MID)
        {
            var memberData = _libMemberInfoService.GetMemberData(MID);

            if (string.IsNullOrWhiteSpace(memberData.detail.CellPhone)) return;

            string msg = "您的icash Pay註冊申請資料有誤，請洽客服0800-233-888。";

            _smsService.SendSMS(memberData.detail.CellPhone, msg, 0);
        }
        #endregion
    }
}
