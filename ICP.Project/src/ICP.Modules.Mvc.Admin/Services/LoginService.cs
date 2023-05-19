using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web.Security;
using System.Web;
using Newtonsoft.Json;

namespace ICP.Modules.Mvc.Admin.Services
{
    using Infrastructure.Core.Models;
    using Infrastructure.Core.Helpers;
    using Infrastructure.Core.Extensions;
    using Enums;
    using Models;
    using Repositories;
    using System.Text.RegularExpressions;
    using ICP.Infrastructure.Core.Models.Consts;

    public class LoginService
    {
        User _user;
        ConfigRepository _configRepository;
        UserRepository _userRepository;

        public LoginService(UserRepository userRepository, ConfigRepository configRepository)
        {
            _userRepository = userRepository;
            _configRepository = configRepository;
        }

        private void CheckHttpContext()
        {
            if (HttpContext.Current == null)
            {
                throw new Exception("HttpContext is null");
            }
        }

        #region 登入
        /// <summary>
        /// 登入檢查
        /// </summary>
        /// <param name="Account"></param>
        /// <param name="Pwd"></param>
        /// <returns></returns>
        public UserLoginResult Login(string Account, string Pwd, long RemoteRealIP)
        {
            string _pwdEncrypt = PwdEncrypt(Pwd);
            return _userRepository.CheckUserLogin(Account, _pwdEncrypt);
        }

        public BaseResult VerifyLoginModel(string Account, string Pwd)
        {
            var result = new BaseResult();
            result.SetError();

            string accountPattern = "^[P]{1}[0-9]{8}$";
            var reg = new Regex(accountPattern);
            if (!reg.IsMatch(Account))
            {
                result.RtnMsg = "請輸入正確的帳號進行登入";
                return result;
            }

            string pwdPattern = RegexConst.AdminUserPwd;
            var pwdReg = new Regex(pwdPattern);
            if (!pwdReg.IsMatch(Pwd))
            {
                result.RtnMsg = "請輸入正確的密碼進行登入";
                return result;
            }

            result.SetSuccess();
            return result;
        }

        /// <summary>
        /// 更新使用者登入 Token
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="LoginToken"></param>
        /// <returns></returns>
        public UpdateUserLoginTokenResult UpdateUserLoginToken(int UserID, string LoginToken)
        {
            return _userRepository.UpdateUserLoginToken(UserID, LoginToken);
        }

        /// <summary>
        /// 檢查使用者登入 Token
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="LoginToken"></param>
        /// <returns></returns>
        public BaseResult CheckUserLoginToken()
        {
            if (userCookieModel == null)
            {
                return new BaseResult { RtnCode = 0, RtnMsg = "尚未登入" };
            }

            return _userRepository.CheckUserLoginToken(userCookieModel.UserID, userCookieModel.LoginToken);
        }

        /// <summary>
        /// 寫入登入cookie
        /// </summary>
        /// <param name="UserID">使用者識別編號</param>
        /// <param name="UserName">使用者名稱</param>
        public void SetLoginCookie(int UserID, string UserName, string LoginToken, DateTime? LoginTokenExpire)
        {
            CheckHttpContext();

            int version = 1;
            string name = UserName;
            DateTime issueDate = DateTime.Now;
            DateTime expiration = LoginTokenExpire ?? DateTime.Today.AddYears(1);
            bool isPersistent = true;
            string userData = JsonConvert.SerializeObject(new UserCookieModel
            {
                UserID = UserID,
                LoginToken = LoginToken
            });

            string value = FormsAuthentication.Encrypt(
                new FormsAuthenticationTicket(
                    version,
                    name,
                    issueDate,
                    expiration,
                    isPersistent,
                    userData,
                    FormsAuthentication.FormsCookiePath
                )
            );

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, value);
            cookie.Name = FormsAuthentication.FormsCookieName;
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 讀取登入cookie
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="Request"></param>
        public static void LoadCookie(HttpContext Context, HttpRequest Request)
        {
            var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie == null) return;

            FormsAuthenticationTicket authTicket;

            try
            {
                authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            }
            catch
            {
                return;
            }

            if (authTicket == null) return;

            Context.User = new System.Security.Principal.GenericPrincipal(new FormsIdentity(authTicket), new string[] { });
        }
        #endregion

        /// <summary>
        /// 寫入密碼到期cookie
        /// </summary>
        public void SetPwdExpiredCookie()
        {
            string cookieName = "PwdExpired";
            string cookieValue = "1";
            HttpCookie cookie = new HttpCookie(cookieName, cookieValue);
            cookie.Expires = DateTime.Now.AddHours(1);
            if (HttpContext.Current.Request.Cookies[cookieName] == null)
                HttpContext.Current.Response.Cookies.Add(cookie);
        }

        #region 取得ASP.NET通用識別
        /// <summary>
        /// 取得ASP.NET通用識別
        /// </summary>
        /// <returns></returns>
        private System.Security.Principal.GenericPrincipal GetGenericPrincipal()
        {
            CheckHttpContext();

            //var serivce = DependencyResolver.Current.GetService<LoginService>();

            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie == null) return null;

            FormsAuthenticationTicket authTicket;

            try
            {
                authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            }
            catch
            {
                return null;
            }

            if (authTicket == null) return null;

            return new System.Security.Principal.GenericPrincipal(new FormsIdentity(authTicket), new string[] { });
        }
        #endregion

        #region 使用者是否已登入
        /// <summary>
        /// 使用者是否已登入
        /// </summary>
        /// <returns></returns>
        public bool CheckLogin()
        {
            AuthorizeResultType authorizeResultType = AuthorizeResultType.Success;
            return CheckLogin(ref authorizeResultType);
        }

        /// <summary>
        /// 使用者是否已登入
        /// </summary>
        /// <param name="authorizeResultType"></param>
        /// <returns></returns>
        public bool CheckLogin(ref AuthorizeResultType authorizeResultType)
        {
            User user;
            BaseResult checkTokenReuslt;

            #region 檢查是否登入
            if (!IsLogin)
            {
                authorizeResultType = AuthorizeResultType.Logout;
                return false;
            }
            #endregion

            #region 檢查登入Token
            if (!(checkTokenReuslt = CheckUserLoginToken()).IsSuccess)
            {
                authorizeResultType = AuthorizeResultType.LoginToken;
                return false;
            }
            #endregion

            #region 檢查帳號狀態
            if ((user = GetCurrentUser()) != null && user.UserStatus != 1)
            {
                authorizeResultType = AuthorizeResultType.UserStatus;
                return false;
            }
            #endregion

            #region 檢查密碼是否到期
            if (CheckPwdExpire(user.UserID) && !IsSkipPwdExpired())
            {
                authorizeResultType = AuthorizeResultType.PwdExpired;
                return false;
            }
            #endregion

            authorizeResultType = AuthorizeResultType.Success;
            return true;
        }
        #endregion

        #region 使用者是否已登入
        /// <summary>
        /// IsLogin
        /// </summary>
        /// <returns></returns>
        private bool IsLogin
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    return false;
                }

                return HttpContext.Current.User.Identity.IsAuthenticated;
            }
        }
        #endregion

        #region 登入使用者登入 Cookie 資料
        private bool _userCookieModel_loaded;
        private UserCookieModel _userCookieModel;
        private UserCookieModel userCookieModel
        {
            get
            {
                if (IsLogin && !_userCookieModel_loaded)
                {
                    string json = ((FormsIdentity)HttpContext.Current.User.Identity).Ticket.UserData;
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        try
                        {
                            _userCookieModel = JsonConvert.DeserializeObject<UserCookieModel>(json);
                        }
                        catch { }
                    }
                    _userCookieModel_loaded = true;
                }
                return _userCookieModel;
            }
        }
        #endregion

        #region 取得當前登入使用者
        /// <summary>
        /// 取得當前登入使用者ID
        /// </summary>
        /// <returns></returns>
        public int GetCurrentUserID()
        {
            if (!IsLogin) return 0;

            if (userCookieModel == null) return 0;

            return userCookieModel.UserID;
        }

        /// <summary>
        /// 取得當前登入使用者
        /// </summary>
        /// <returns></returns>
        public User GetCurrentUser()
        {
            if (_user == null)
            {
                int UserID = GetCurrentUserID();

                if (UserID <= 0) return null;

                _user = _userRepository.GetUser(UserID);
            }

            return _user;
        }
        #endregion

        #region 登出
        /// <summary>
        /// 登出
        /// </summary>
        public void Logout()
        {
            CheckHttpContext();

            HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];

            authCookie.Expires = DateTime.Now.AddYears(-1);

            HttpContext.Current.Response.SetCookie(authCookie);
        }
        #endregion

        #region 密碼加密
        /// <summary>
        /// 密碼加密
        /// </summary>
        /// <param name="Pwd"></param>
        /// <returns></returns>
        public string PwdEncrypt(string Pwd)
        {
            HashCryptoHelper cryptoHelper = new HashCryptoHelper();
            return cryptoHelper.HashSha256(
                _configRepository.AdminLoginUserPwdKey +
                Pwd +
                _configRepository.AdminLoginUserPwdIV
            );
        }
        #endregion

        #region 取得UserSecurity
        public DataResult<UserSecurity> GetUserSecurity(int UserID)
        {
            var result = new DataResult<UserSecurity>();
            result.SetError();

            var rtnData = _userRepository.GetUserSecurity(UserID);
            if (rtnData == null)
            {
                return result;
            }

            result.SetSuccess(rtnData);
            return result;
        }
        #endregion

        /// <summary>
        /// 確認密碼是否到期
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public bool CheckPwdExpire(int UserID)
        {
            var SecurityResult = GetUserSecurity(UserID);
            UserSecurity security = SecurityResult.RtnData;

            if (security.LastChangePwdAt != null)
            {
                DateTime startDate = security.LastChangePwdAt.Value.AddMonths(3);

                if (startDate < DateTime.Now)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsSkipPwdExpired()
        {
            string action = HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString();

            if (action == "ChangePwd")
            {
                return true;
            }

            return false;
        }
    }
}