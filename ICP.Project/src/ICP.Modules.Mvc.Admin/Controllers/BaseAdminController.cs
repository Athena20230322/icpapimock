using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Controllers
{
    using Infrastructure.Core.Web.Controllers;
    using Models;
    using Services;

    [Attributes.UserLoginAuth]
    [Attributes.UserExecutedLogAttribute]
    public abstract class BaseAdminController : BaseMvcController
    {
        public void Injection(LoginService loginService)
        {
            this.loginService = loginService;
        }

        //由 UserLoginAuth 注入
        public LoginService loginService { get; set; }

        public int CurrentUserID
        {
            get
            {
                return loginService.GetCurrentUserID();
            }
        }

        public User CurrentUser
        {
            get
            {
                return loginService.GetCurrentUser();
            }
        }
    }
}
