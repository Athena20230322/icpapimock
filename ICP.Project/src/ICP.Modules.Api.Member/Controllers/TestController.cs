using ICP.Infrastructure.Abstractions.Authorization;
using ICP.Infrastructure.Abstractions.FilterProxy;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Web.Attributes;
using ICP.Infrastructure.Core.Web.Controllers;
using ICP.Infrastructure.Core.Web.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ICP.Modules.Api.Member.Controllers
{
    public class TestController : BaseApiController
    {
        private readonly IUserManager _userManager = null;

        public TestController(IAuthorizationFactory authorizationFactory)
        {
            _userManager = authorizationFactory.Create(AuthorizationType.Api);
        }

        public ActionResult Test()
        {
            // 取 realIP
            string realIP = Request.RemoteRealIP();
            
            return Content(base.GetType().FullName);
        }

        [HttpPost]
        [AllowAnonymous]
        [AllowOPAnonymous]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        public ActionResult FileUpload(HttpPostedFileBase img1 = null)
        {
            var result = new DataResult<BaseResult>();
            var checkResult = new BaseResult();

            if (img1 == null)
            {
                checkResult.RtnCode = 0;
                checkResult.RtnMsg = "img1 is null";
                result.SetError(checkResult);
                return AppResult(result);
            }

            checkResult.SetSuccess();
            result.SetSuccess(checkResult);
            return AppResult(result);
        }

    }
}
