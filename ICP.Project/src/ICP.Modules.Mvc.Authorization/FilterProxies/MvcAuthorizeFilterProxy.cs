using ICP.Infrastructure.Abstractions.Authorization;
using ICP.Infrastructure.Core.Web.Frameworks.FilterProxy;
using ICP.Library.Models.AuthorizationMvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;

namespace ICP.Modules.Mvc.Authorization.FilterProxies
{
    public class MvcAuthorizeFilterProxy : FilterProxy
    {
        private readonly IUserManager _userManager = null;

        public MvcAuthorizeFilterProxy(
            IAuthorizationFactory authorizationFactory
            )
        {
            _userManager = authorizationFactory.Create(AuthorizationType.Mvc);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext, params object[] args)
        {
            base.OnActionExecuting(filterContext, args);

            var attributes = filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true);
            bool allowAnonymous = (attributes != null && attributes.Length > 0);

            if (!allowAnonymous && !_userManager.IsLogin)
            {
                var IsWebView = _userManager.GetData<bool>(UserDataType.IsWebView);
                if (IsWebView)
                {
                    var contentResult = new ContentResult();
                    contentResult.Content = string.Empty;
                    filterContext.Result = contentResult;
                }
                else
                {
                    var redirectResult = new RedirectResult(FormsAuthentication.LoginUrl);
                    filterContext.Result = redirectResult;
                }
                return;
            }
        }
    }
}
