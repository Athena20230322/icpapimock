using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Services.MemberServices
{
    using ICP.Library.Models.AuthorizationApi;
    using Infrastructure.Core.Extensions;
    using Infrastructure.Core.Models;
    using Repositories.MemberRepositories;

    public class LibMemberLoginService
    {
        MemberConfigCyptRepository _configCyptRepository;
        MemberLoginRepository _memberLoginRepository;

        public LibMemberLoginService(
            MemberConfigCyptRepository configCyptRepository,
            MemberLoginRepository memberLoginRepository
            )
        {
            _configCyptRepository = configCyptRepository;
            _memberLoginRepository = memberLoginRepository;
        }

        /// <summary>
        /// 帳號密碼登入
        /// </summary>
        /// <param name="UserCode">帳號</param>
        /// <param name="UserPwd">密碼</param>
        /// <param name="LoginType">登入方式</param>
        /// <param name="RealIP">RealIP</param>
        /// <param name="ProxyIP">ProxyIP</param>
        /// <param name="checkMID">檢查是否為該 MID 登入</param>
        /// <param name="SMSAuthCode">OTP驗證碼</param>
        /// <param name="MockLogin">模擬登入 0:否 1:是</param>
        /// <param name="appRequest">app登入裝置資訊</param>
        /// <returns>回傳登入MID</returns>
        public DataResult<long> UserCodeLogin(string UserCode, string UserPwd, byte? LoginType, long RealIP, long ProxyIP, 
            long checkMID = 0,
            string SMSAuthCode = null,
            byte MockLogin = 0,
            BaseAuthorizationApiRequest appRequest = null
            )
        {
            if (LoginType == null)
            {
                LoginType = 1;
            }

            string jsonApp;
            if (appRequest == null)
            {
                jsonApp = null;
            }
            else
            {
                jsonApp = Newtonsoft.Json.JsonConvert.SerializeObject(appRequest);
            }

            string enUserCode = _configCyptRepository.Encrypt_UserCode(UserCode);
            string hashUserPwd = _configCyptRepository.Hash_UserPwd(UserPwd);

            return _memberLoginRepository.UserCodeLogin(enUserCode, hashUserPwd, LoginType.Value, checkMID, MockLogin, SMSAuthCode, jsonApp, RealIP, ProxyIP);
        }


        public BaseResult CheckChangeDevice(long MID, string DeviceID)
        {
            return _memberLoginRepository.CheckChangeDevice(MID, DeviceID);
        }
    }
}
