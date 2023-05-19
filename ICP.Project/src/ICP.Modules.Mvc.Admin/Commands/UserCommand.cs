using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Commands
{
    using Infrastructure.Core.Models;
    using Infrastructure.Core.Extensions;
    using Infrastructure.Abstractions.EmailSender;
    using Models;
    using Models.ViewModels;
    using Services;
    using ICP.Modules.Mvc.Admin.Repositories;

    public class UserCommand
    {
        UserService _userService;
        LoginService _loginService;
        IEmailSender _emailSender;
        MailManageService _mailManageService;
        ConfigRepository _configRepository;

        public UserCommand(
            UserService userService, 
            LoginService loginService,
            IEmailSender emailSender,
            MailManageService mailManageService,
            ConfigRepository configRepository
            )
        {
            _userService = userService;
            _loginService = loginService;
            _emailSender = emailSender;
            _mailManageService = mailManageService;
            _configRepository = configRepository;
        }

        public DataResult<int> AddUser(User model, long IP)
        {
            var result = new DataResult<int>();
            result.SetError();

            string AuthCode = null;
            string Pwd = _loginService.PwdEncrypt(_configRepository.DefaultPwd);
            var addResult = _userService.AddUser(model, IP, ref AuthCode, Pwd);
            if (!addResult.IsSuccess)
            {
                result.SetError(addResult);
                return result;
            }

            int UserID = addResult.RtnData;

            result.SetSuccess(UserID);
            return result;
        }

        public BaseResult ChangeUserPwd(int UserID, string Pwd, long IP)
        {
            string encryptPwd = _loginService.PwdEncrypt(Pwd);
            return _userService.ChangeUserPwd(UserID, encryptPwd, IP);
        }

        public BaseResult CheckUserForgetToken(string Token)
        {
            return _userService.CheckUserForgetToken(Token);
        }

        public DataResult<User> GetUser(int UserID)
        {
            return _userService.GetUser(UserID);
        }

        public List<UserQueryResult> ListUser(UserQuery query)
        {
            int TotalCount = 0;
            var list = _userService.ListUser(query, ref TotalCount);
            return list;
        }

        public List<UserQueryResult> ListUserByGroup(int UserGroupID)
        {
            return _userService.ListUserByGroup(UserGroupID);
        }

        public BaseResult UpdateUser(int UserID, User model, long IP)
        {
            return _userService.UpdateUser(UserID, model, IP);
        }

        public BaseResult UpdateUserForgetToken(string Account, string UserEmail, string Token)
        {
            return _userService.UpdateUserForgetToken(Account, UserEmail, Token);
        }

        public BaseResult UpdateUserPwd(int UserID, string Pwd, long IP)
        {
            string encryptPwd = _loginService.PwdEncrypt(Pwd);
            return _userService.UpdateUserPwd(UserID, encryptPwd, IP);
        }

        public BaseResult UpdateUserStatus(int UserID, byte UserStatus)
        {
            return _userService.UpdateUserStatus(UserID, UserStatus);
        }

        public BaseResult DeleteUser(int UserID)
        {
            return _userService.DeleteUser(UserID);
        }
    }
}
