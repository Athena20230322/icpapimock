using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Authorization.Models;
using ICP.Modules.Mvc.Authorization.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace ICP.Modules.Mvc.Authorization.Services
{
    public class IdentifyService
    {
        private readonly HttpContext _httpContext = null;
        private readonly ILogger _logger = null;
        private static readonly string _cookeName = FormsAuthentication.FormsCookieName;

        public IdentifyService(
            HttpContext httpContext,
            ILogger<IdentifyService> logger)
        {
            _httpContext = httpContext;
            _logger = logger;
        }

        public void WriteCookie(long mid, string token, bool isWebView, DateTime? tokenExpired = null, string userName = "")
        {
            _logger.Trace("準備寫入 Cookie");

            string json = JsonConvert.SerializeObject(new UserDataModel
            {
                MID = mid,
                LoginToken = token,
                IsWebView = isWebView
            });

            int version = 1;
            string name = userName;
            DateTime issueDate = DateTime.Now;
            DateTime expiration = tokenExpired ?? DateTime.Today.AddYears(1);
            bool isPersistent = true;
            string userData = json;

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

            var cookie = new HttpCookie(_cookeName, value);
            cookie.Name = _cookeName;

            if (tokenExpired != null)
            {
                cookie.Expires = tokenExpired.Value;
            }

            if (!_httpContext.Request.IsLocal)
            {
                var host = _httpContext.Request.Url.Host;
                host.Substring(host.IndexOf('.') + 1);
                string domain = host.Substring(host.IndexOf('.') + 1);
                cookie.Domain = domain;
            }

            _httpContext.Response.Cookies.Add(cookie);

            _logger.Trace("寫入 Cookie 成功");
        }

        /// <summary>
        /// 讀取登入cookie
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="Request"></param>
        public static void LoadCookie(HttpContext Context, HttpRequest Request)
        {
            var authCookie = Request.Cookies[_cookeName];

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

        public void RemoveCookie()
        {
            _logger.Trace("準備移除 Cookie");

            _httpContext.Response.Cookies.Remove(_cookeName);

            _logger.Trace("移除 Cookie 成功");
        }

        public DataResult<UserDataModel> GetUserDataByCookie()
        {
            var result = new DataResult<UserDataModel>();
            result.SetError();

            if (!_httpContext.User.Identity.IsAuthenticated) return result;

            try
            {
                string json = ((FormsIdentity)_httpContext.User.Identity).Ticket.UserData;

                _logger.Trace($"準備反序列化 {nameof(json)}，長度 = {json?.Length}");

                var userData = JsonConvert.DeserializeObject<UserDataModel>(json);

                _logger.Trace($"反序列化 {nameof(json)} 成功");

                result.SetSuccess(userData);
            }
            catch (Exception ex)
            {
                result.SetCode(10029);
                _logger.Warning(ex, result.RtnMsg);
            }

            return result;
        }

        public BaseResult ValidUserData(UserDataModel userData)
        {
            var result = new BaseResult();
            result.SetError();

            // TODO: 驗證 UserDataModel
            // WebView 不用驗 LoginToken
            _logger.Trace($"準備驗證 UserDataModel MID = {userData?.MID}, LoginToken = {userData?.LoginToken}");

            result.SetSuccess();
            return result;
        }

        public DataResult<string> AddLoginToken(long mid)
        {
            var result = new DataResult<string>();
            result.SetError();
            
            // TODO: 新增 LoginToken
            _logger.Trace($"準備新增 LoginToken");
            string token = Guid.NewGuid().ToString();

            result.SetSuccess(token);
            return result;
        }

        public BaseResult RemoveLoginToken(long mid)
        {
            var result = new BaseResult();
            result.SetError();

            // TODO: 移除 LoginToken
            _logger.Trace($"準備移除 LoginToken");

            result.SetSuccess();
            return result;
        }
    }
}
