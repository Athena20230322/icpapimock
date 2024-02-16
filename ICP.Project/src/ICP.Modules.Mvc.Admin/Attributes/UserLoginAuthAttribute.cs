using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web;
using System.Web.Security;

namespace ICP.Modules.Mvc.Admin.Attributes
{
    using Infrastructure.Core.Models;
    using System.Web.Mvc;
    using Services;
    using Enums;
    using ICP.Infrastructure.Core.Models.Consts;

    class UserLoginAuthAttribute : AuthorizeAttribute
    {
        public LoginService loginService { get; set; }

        public PrivilegeService privilegeService { get; set; }

        public string MappingMethod { get; set; }

        public string Controller { get; set; }

        public MappingMethodAction Action { get; set; }

        private bool IsAuthorized(AuthorizationContext filterContext, ref AuthorizeResultType authorizeResultType)
        {
            if (!loginService.CheckLogin(ref authorizeResultType))
            {
                return false;
            }

            string controllerName = string.IsNullOrEmpty(Controller) ? filterContext.ActionDescriptor.ControllerDescriptor.ControllerName : Controller;

            if (Action != MappingMethodAction.None && !string.IsNullOrEmpty(MappingMethod))
            {
                #region 檢查功能權限
                int UserID = loginService.GetCurrentUserID();
                int ActionSum = privilegeService.GetFunctionActionByUser(UserID, controllerName, MappingMethod);
                var iAction = (int)Action;
                if ((iAction & ActionSum) != iAction)
                {
                    authorizeResultType = AuthorizeResultType.NoPermission;
                    return false;
                }
                #endregion
            }

            if (!string.IsNullOrEmpty(MappingMethod))
            {
                #region 檢查功能開關
                byte FunctionStatus = privilegeService.GetFunctionStatus(controllerName, MappingMethod);
                if (FunctionStatus != 1)
                {
                    authorizeResultType = AuthorizeResultType.FuncMaintain;
                    return false;
                }
                #endregion
            }

            authorizeResultType = AuthorizeResultType.Success;
            return true;
        }

        private bool IsSkipAuthorization(AuthorizationContext filterContext)
        {
            return filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!IsSkipAuthorization(filterContext))
            {
                AuthorizeResultType authorizeResultType = AuthorizeResultType.Success;
                if (!IsAuthorized(filterContext, ref authorizeResultType))
                {
                    HandleUnauthorizedRequest(filterContext, authorizeResultType);
                    return;
                }
            }

            if (filterContext.Controller is Controllers.BaseAdminController)
            {
                ((Controllers.BaseAdminController)filterContext.Controller).Injection(loginService);
            }
        }

        private void HandleUnauthorizedRequest(AuthorizationContext filterContext, AuthorizeResultType authorizeResultType)
        {
            if (authorizeResultType == AuthorizeResultType.Logout)
            {
                HandleLogoutRequest(filterContext);
            }
            else if (authorizeResultType == AuthorizeResultType.LoginToken)
            {
                HandleLoginTokenRequest(filterContext);
            }
            else if (authorizeResultType == AuthorizeResultType.UserStatus)
            {
                HandleUserStatusRequest(filterContext);
            }
            else if (authorizeResultType == AuthorizeResultType.NoPermission)
            {
                HandleNoPermissionRequest(filterContext);
            }
            else if (authorizeResultType == AuthorizeResultType.PwdExpired)
            {
                HandlePwdExpired(filterContext);
            }
            else if (authorizeResultType == AuthorizeResultType.FuncMaintain)
            {
                HandleFuncMaintain(filterContext);
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }

        private void SetResult(AuthorizationContext filterContext, string errorMsg, Func<ActionResult> func)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                var jsonResult = new JsonResult();
                jsonResult.Data = new BaseResult { RtnMsg = errorMsg };
                jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                filterContext.Result = jsonResult;
            }
            else
            {
                filterContext.Result = func();
            }
        }

        private void HandleLogoutRequest(AuthorizationContext filterContext)
        {
            string errorMsg = "你尚未登入";

            SetResult(filterContext, errorMsg, () => 
            {
                string url = FormsAuthentication.LoginUrl;

                var sb = new StringBuilder();
                sb.Append("<script>");
                sb.AppendFormat("window.alert('{0}');", errorMsg);
                sb.AppendFormat("window.location.href = '{0}';", url);
                sb.Append("</script>");

                var result = new ContentResult();
                result.ContentEncoding = Encoding.UTF8;
                result.ContentType = MimeTypes.TextHtml;
                result.Content = sb.ToString();

                return result;
            });
        }

        private void HandleLoginTokenRequest(AuthorizationContext filterContext)
        {
            string errorMsg = "登入 Token 已失效";

            SetResult(filterContext, errorMsg, () => 
            {
                string url = "/Account/Logout?msg=" + HttpUtility.UrlEncode(errorMsg);
                return new RedirectResult(url, false);
            });
        }

        private void HandleUserStatusRequest(AuthorizationContext filterContext)
        {
            string errorMsg = "帳號已被停權";

            SetResult(filterContext, errorMsg, () =>
            {
                string url = "/Account/Logout?msg=" + HttpUtility.UrlEncode(errorMsg);
                return new RedirectResult(url, false);
            });
        }

        private void HandleNoPermissionRequest(AuthorizationContext filterContext)
        {
            string errorMsg = "你的權限不足";

            SetResult(filterContext, errorMsg, () =>
            {
                string url = "javascript:history.back()";
                string text = "回到上一頁";

                var result = new ContentResult();
                result.ContentEncoding = Encoding.UTF8;
                result.ContentType = MimeTypes.TextHtml;
                result.Content = string.Format(@"
{0}<br />
<a href=""{1}"">{2}</a>", errorMsg, url, text);

                return result;
            });
        }

        private void HandlePwdExpired(AuthorizationContext filterContext)
        {
            string errorMsg = "密碼已到期，請修改密碼";

            SetResult(filterContext, errorMsg, () =>
            {
                string url = "/Account/ChangePwd?Expire=1";
                return new RedirectResult(url, false);
            });
        }

        private void HandleFuncMaintain(AuthorizationContext filterContext)
        {
            string errorMsg = "功能維護中";

            SetResult(filterContext, errorMsg, () =>
            {
                string url = "javascript:history.back()";
                string text = "回到上一頁";

                var result = new ContentResult();
                result.ContentEncoding = Encoding.UTF8;
                result.ContentType = MimeTypes.TextHtml;
                result.Content = string.Format(@"
{0}<br />
<a href=""{1}"">{2}</a>", errorMsg, url, text);

                return result;
            });
        }
    }
}
