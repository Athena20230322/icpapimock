using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Services
{
    using Infrastructure.Core.Models;
    using Modules.Mvc.Admin.Repositories;
    using Models;
    using Models.ViewModels;
    using Infrastructure.Core.Extensions;

    public class UserService
    {
        UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public DataResult<int> AddUser(User model, long IP, ref string AuthCode, string Pwd)
        {
            AuthCode = Guid.NewGuid().ToString().Replace("-", "");
            return _userRepository.AddUser(model, AuthCode, IP, Pwd);
        }

        public BaseResult CheckUserAuthCode(string AuthCode)
        {
            return _userRepository.CheckUserAuthCode(AuthCode);
        }

        public BaseResult AuthUser(string AuthCode, string Pwd)
        {
            return _userRepository.AuthUser(AuthCode, Pwd);
        }

        public BaseResult CheckUserPwd(int UserID, string Pwd)
        {
            return _userRepository.CheckUserPwd(UserID, Pwd);
        }

        public BaseResult ChangeUserPwd(int UserID, string Pwd, long IP)
        {
            return _userRepository.ChangeUserPwd(UserID, Pwd, IP);
        }

        public CheckUserForgetTokenResult CheckUserForgetToken(string Token)
        {
            return _userRepository.CheckUserForgetToken(Token);
        }

        public DataResult<User> GetUser(int UserID)
        {
            var result = new DataResult<User>();
            result.SetError();

            var rtnData = _userRepository.GetUser(UserID);
            if (rtnData == null)
            {
                return result;
            }

            result.SetSuccess(rtnData);
            return result;
        }

        public List<UserQueryResult> ListUser(UserQuery query)
        {
            int TotalCount = 0;
            return ListUser(query, ref TotalCount);
        }
        public List<UserQueryResult> ListUser(UserQuery query, ref int TotalCount)
        {
            var list = _userRepository.ListUser(query);

            if (list.Count == 0)
            {
                TotalCount = 0;
            }
            else
            {
                TotalCount = list[0].TotalCount;
            }

            return list;
        }

        public BaseResult ResetUserPwd(string Token, string Pwd, long IP)
        {
            return _userRepository.ResetUserPwd(Token, Pwd, IP);
        }

        public BaseResult UpdateUser(int UserID, User model, long IP)
        {
            return _userRepository.UpdateUser(UserID, model, IP);
        }

        public BaseResult UpdateUserForgetToken(string Account, string UserEmail, string Token)
        {
            return _userRepository.UpdateUserForgetToken(Account, UserEmail, Token);
        }

        public BaseResult UpdateUserPwd(int UserID, string Pwd, long IP)
        {
            return _userRepository.UpdateUserPwd(UserID, Pwd, IP);
        }

        public BaseResult UpdateUserStatus(int UserID, byte UserStatus)
        {
            return _userRepository.UpdateUserStatus(UserID, UserStatus);
        }

        public BaseResult DeleteUser(int UserID)
        {
            return _userRepository.DeleteUser(UserID);
        }

        public List<UserQueryResult> ListUserByGroup(int UserGroupID)
        {
            return _userRepository.ListUserByGroup(UserGroupID);
        }

        public User GetUserByAccount(string Account)
        {
            return _userRepository.GetUserByAccount(Account);
        }

        /// <summary>
        /// 取得業務人員
        /// </summary>
        /// <returns></returns>
        public List<User> ListSales() => _userRepository.ListSales();
    }
}
