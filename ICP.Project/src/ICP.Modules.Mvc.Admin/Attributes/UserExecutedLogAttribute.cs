using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web;
using System.Web.Security;
using System.Web.Helpers;

namespace ICP.Modules.Mvc.Admin.Attributes
{
    using System.Web.Mvc;
    using Infrastructure.Core.Models;
    using ICP.Infrastructure.Core.Web.Extensions;
    using ICP.Infrastructure.Abstractions.Logging;
    using Services;
    using Models;
    using ICP.Infrastructure.Core.Frameworks.Logging;

    public class IgnoreActionLogAttribute : Attribute { }

    public class UserExecutedLogAttribute : ActionFilterAttribute
    {
       
        public LoginService loginService { get; set; }
        public UserExecutedService userExecutedService { get; set; }
       

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            NLogLogger _Logger = new NLogLogger<UserExecutedLogAttribute>();
            if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(IgnoreActionLogAttribute), false).Length > 0 || !loginService.CheckLogin()) {
                base.OnActionExecuting(filterContext);
                return;
            }

            int UserID = 0;
            string Account = string.Empty;
            string ControllerName = string.Empty;
            string ActionName = string.Empty;
            string FormData = string.Empty;
         
            try
            {
                string[] ignoreKeyWord = { "password", "Pwd" };

                // 設定操作記錄相關資訊
                var queryString = filterContext.HttpContext.Request.Unvalidated().QueryString;
                var form = filterContext.HttpContext.Request.Unvalidated().Form;
                var headers = filterContext.HttpContext.Request.Headers;

                UserID = loginService.GetCurrentUserID();
                Account = loginService.GetCurrentUser().Account;
                ControllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                ActionName = filterContext.ActionDescriptor.ActionName;
                FormData = userExecutedService.ToDictionary(form, ignoreKeyWord);

            
                // 新增
                userExecutedService.AddActionLog(new UserExecuted()
                {
                    UserID = UserID,
                    Account = Account,
                    ControllerName = ControllerName,
                    ActionName = ActionName,
                    Path = filterContext.HttpContext.Request.Path,
                    Headers = userExecutedService.ToDictionary(headers, "Cookie"),
                    UrlQuery = userExecutedService.ToDictionary(queryString),
                    FormData = FormData,
                    RealIP = filterContext.HttpContext.Request.RealIP(),
                    ProxyIP = filterContext.HttpContext.Request.ProxyIP()
                });

            }
            catch (Exception ex) {
                _Logger.Trace($"新增使用者操作記錄，登入 ID : {UserID.ToString()}, Controller :{ControllerName} ActionName : {ActionName} , Error : {ex.Message}");
            }
            finally
            {
                base.OnActionExecuting(filterContext);
            }
          

        }
    }
}
