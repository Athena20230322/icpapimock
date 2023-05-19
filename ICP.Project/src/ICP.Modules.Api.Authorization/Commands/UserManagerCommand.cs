using System.Linq;
using ICP.Infrastructure.Abstractions.Authorization;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.AuthorizationApi;
using System.Collections.Generic;
using System;

namespace ICP.Modules.Api.Authorization.Commands
{
    public class UserManagerCommand : IUserManager
    {
        GlobalAppSetting _globalAppSetting;

        #region 公開屬性

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

        public bool IsLogin
        {
            get
            {
                if (_userData != null && _userData.TryGetValue(UserDataType.IsLogin, out object obj))
                {
                    return (bool)obj;
                }
                else
                {
                    return false;
                }
            }
        }

        public UserManagerCommand(
            GlobalAppSetting globalAppSetting
            )
        {
            _globalAppSetting = globalAppSetting;
        }

        #endregion

        private IDictionary<string, object> _userData = null;

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
            _userData = userData;
            if (_userData != null && _userData.ContainsKey(UserDataType.MID))
            {
                _userData[UserDataType.IsLogin] = true;

                _globalAppSetting.LoadUserID((long)_userData[UserDataType.MID]);
            }
        }

        public void Logout()
        {
            if (_userData == null) return;

            _userData.Keys
                //保留 KeyContext, 以便 Response 內容加密
                .Where(key => key != UserDataType.AuthorizationApiKeyContext)
                .ToList()
                .ForEach(key =>
                {
                    _userData.Remove(key);
                });

            //_userData?.Clear();
        }
    }
}
