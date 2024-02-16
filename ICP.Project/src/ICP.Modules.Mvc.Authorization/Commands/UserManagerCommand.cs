using ICP.Infrastructure.Abstractions.Authorization;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.AuthorizationMvc;
using ICP.Modules.Mvc.Authorization.Models;
using ICP.Modules.Mvc.Authorization.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace ICP.Modules.Mvc.Authorization.Commands
{
    public class UserManagerCommand : IUserManager
    {
        GlobalAppSetting _globalAppSetting;

        #region 公開屬性

        public bool IsLogin => (_userData != null && MID > 0);

        public long MID
        {
            get
            {
                if (_userData != null && _userData.TryGetValue(UserDataType.MID, out object obj))
                {
                    return (long)obj;
                }
                else
                {
                    return 0;
                }
            }
        }

        #endregion

        private readonly IdentifyService _identifyService = null;
        private IDictionary<string, object> _userData = null;

        public UserManagerCommand(
            GlobalAppSetting globalAppSetting,
            IdentifyService identifyService
            )
        {
            _globalAppSetting = globalAppSetting;
            _identifyService = identifyService;
            initUserData();
            if (MID > 0) _globalAppSetting.LoadUserID(MID);
        }

        public T GetData<T>(string name)
        {
            if (_userData == null || !_userData.TryGetValue(name, out object value))
            {
                return default(T);
            }

            return (T)value;
        }

        public void Login(IDictionary<string, object> userData, DateTime? Expired = null)
        {
            if (userData == null || !userData.ContainsKey(UserDataType.MID))
            {
                throw new ArgumentException(nameof(userData));
            }

            _userData = userData;
            
            string token;

            bool IsWebView = false;
            if (userData.ContainsKey(UserDataType.IsWebView))
            {
                IsWebView = (bool)_userData[UserDataType.IsWebView];
            }

            if (!IsWebView)
            {
                var addLoginTokenResult = _identifyService.AddLoginToken(MID);
                if (!addLoginTokenResult.IsSuccess)
                {
                    throw new Exception(addLoginTokenResult.RtnMsg);
                }

                token = addLoginTokenResult.RtnData;
            }
            else
            {
                token = string.Empty;
            }

            _userData[UserDataType.LoginToken] = token;

            _identifyService.WriteCookie(MID, token, IsWebView, tokenExpired: Expired);

            _globalAppSetting.LoadUserID(MID);
        }

        public void Logout()
        {
            _userData?.Clear();

            _identifyService.RemoveCookie();
            _identifyService.RemoveLoginToken(MID);
        }

        private void initUserData()
        {
            var getUserDataByCookieResult = _identifyService.GetUserDataByCookie();
            if (!getUserDataByCookieResult.IsSuccess)
            {
                return;
            }

            var userData = getUserDataByCookieResult.RtnData;
            var validResult = _identifyService.ValidUserData(userData);
            if (!validResult.IsSuccess)
            {
                return;
            }

            _userData = new Dictionary<string, object>
            {
                { UserDataType.MID, userData.MID },
                { UserDataType.LoginToken, userData.LoginToken },
                { UserDataType.IsWebView, userData.IsWebView }
            };
        }
    }
}
