using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Commands
{
    using Infrastructure.Core.Extensions;
    using Infrastructure.Abstractions.EmailSender;
    using ICP.Modules.Mvc.Admin.Models;
    using Infrastructure.Core.Models;
    using Services;
    using ICP.Modules.Mvc.Admin.Repositories;

    public class AccountCommand
    {
        LoginService _loginService;
        UserService _userService;
        Library.Services.MailLibrary.MailSendService _emailSender;
        MailManageService _mailManageService;
        ConfigRepository _configRepository;

        public AccountCommand(
            LoginService loginService,
            UserService userService,
            Library.Services.MailLibrary.MailSendService emailSender,
            MailManageService mailManageService,
            ConfigRepository configRepository
            )
        {
            _loginService = loginService;
            _userService = userService;
            _emailSender = emailSender;
            _mailManageService = mailManageService;
            _configRepository = configRepository;
        }

        /// <summary>
        /// 登入並寫入Cookie
        /// </summary>
        /// <param name="Account"></param>
        /// <param name="Pwd"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool UserLogin(string Account, string Pwd, long RemoteRealIP, ref string errorMsg, ref int UserID)
        {
            //帳號密碼格式檢查
            if (_configRepository.ProductMode)
            {
                var verifyResult = _loginService.VerifyLoginModel(Account, Pwd);
                if (!verifyResult.IsSuccess)
                {
                    errorMsg = verifyResult.RtnMsg;
                    return false;
                }
            }

            //登入檢查
            var result = _loginService.Login(Account, Pwd, RemoteRealIP);
            if (!result.IsSuccess)
            {
                errorMsg = result.RtnMsg;
                return false;
            }

            //更新登入 Token
            var loginTokenResult = _loginService.UpdateUserLoginToken(result.UserID, result.LoginToken);
            if (!loginTokenResult.IsSuccess)
            {
                errorMsg = loginTokenResult.RtnMsg;
                return false;
            }

            //寫入登入 Cookie
            string LoginToken = loginTokenResult.LoginToken;
            DateTime? LoginTokenExpire = loginTokenResult.LoginTokenExpire;
            _loginService.SetLoginCookie(result.UserID, result.CName, LoginToken, LoginTokenExpire);
            UserID = result.UserID;

            return true;
        }

        /// <summary>
        /// 登出
        /// </summary>
        public void UserLogout()
        {
            _loginService.Logout();
        }

        /// <summary>
        /// 忘記密碼
        /// </summary>
        /// <param name="Account"></param>
        /// <param name="UserEmail"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool ForgetPwd(string Account, string UserEmail, ref string errorMsg)
        {
            string Token = Guid.NewGuid().ToString().Replace("-", "");

            var result = _userService.UpdateUserForgetToken(Account, UserEmail, Token);
            if (!result.IsSuccess)
            {
                errorMsg = result.RtnMsg;
                return false;
            }
            SendResetPwd(Token, UserEmail, Account);

            return true;
        }

        /// <summary>
        /// 發送修改密碼通知信
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public BaseResult SendResetPwd(string Token, string UserEmail, string Account)
        {
            var result = new BaseResult();
            result.SetError();

            string Mailkey = "admin_user_ForgetPwd";

            string Host_Admin_Domain = _configRepository.AdminDomain;

            var userData = _userService.GetUserByAccount(Account);

            var genArgs = new
            {
                Token,
                Host_Admin_Domain,
                userData.CName
            };

            var content = _mailManageService.Generate(Mailkey, genArgs);

            var mailTo = new List<string>
            {
                UserEmail
            };

            _emailSender.SendMail(mailTo, content.Title, Body: content.Body);

            result.SetSuccess();
            return result;
        }

        /// <summary>
        /// 檢查忘記密碼 Token
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool CheckUserForgetToken(string Token, ref string errorMsg)
        {
            var result = _userService.CheckUserForgetToken(Token);
            if (!result.IsSuccess)
            {
                errorMsg = result.RtnMsg;
                return false;
            }

            return true;
        }

        /// <summary>
        /// 重設密碼
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="Pwd"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool ResetUserPwd(string Token, string Pwd, long IP, ref string errorMsg)
        {
            string encryptPwd = _loginService.PwdEncrypt(Pwd);
            var result = _userService.ResetUserPwd(Token, encryptPwd, IP);
            if (!result.IsSuccess)
            {
                errorMsg = result.RtnMsg;
                return false;
            }

            return true;
        }

        /// <summary>
        /// 修改密碼
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="Pwd"></param>
        /// <returns></returns>
        public bool ChangeUserPwd(int UserID, string OriginPwd, string Pwd, long IP, ref string errorMsg)
        {
            string encryptOriginPwd = _loginService.PwdEncrypt(OriginPwd);
            var checkResult = _userService.CheckUserPwd(UserID, encryptOriginPwd);
            if (!checkResult.IsSuccess)
            {
                errorMsg = checkResult.RtnMsg;
                return false;
            }

            string encryptPwd = _loginService.PwdEncrypt(Pwd);
            var result = _userService.ChangeUserPwd(UserID, encryptPwd, IP);
            if (!result.IsSuccess)
            {
                errorMsg = result.RtnMsg;
                return false;
            }

            return true;
        }

        public BaseResult CheckUserAuthCode(string AuthCode)
        {
            return _userService.CheckUserAuthCode(AuthCode);
        }

        public BaseResult AuthUser(string AuthCode, AccountResetPwdModel model)
        {
            var result = new BaseResult();
            result.SetError();

            if (!model.IsValid())
            {
                result.RtnMsg = model.GetFirstErrorMessage();
                return result;
            }

            string _pwdEncrypt = _loginService.PwdEncrypt(model.Pwd);

            return _userService.AuthUser(AuthCode, _pwdEncrypt);
        }

        public BaseResult AccountResetPwd(int UserID, AccountResetPwdModel model, long IP)
        {
            var result = new BaseResult();
            result.SetError();

            if (!model.IsValid())
            {
                result.RtnMsg = model.GetFirstErrorMessage();
                return result;
            }

            string errorMsg = null;
            if (!ChangeUserPwd(UserID, _configRepository.DefaultPwd, model.Pwd, IP, ref errorMsg))
            {
                result.RtnMsg = errorMsg;
                return result;
            }

            result.SetSuccess();
            return result;
        }

        public DataResult<UserSecurity> GetUserSecurity(int UserID)
        {
            return _loginService.GetUserSecurity(UserID);
        }

        /// <summary>
        /// 確認密碼是否到期
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public bool CheckPwdExpire(int UserID)
        {
            return _loginService.CheckPwdExpire(UserID);
        }
    }
}
