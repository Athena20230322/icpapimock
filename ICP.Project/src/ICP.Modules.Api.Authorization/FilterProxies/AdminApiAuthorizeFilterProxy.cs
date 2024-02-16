using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Models.Consts;
using ICP.Infrastructure.Core.Web.Frameworks.FilterProxy;
using ICP.Library.Repositories.MemberRepositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ICP.Modules.Api.Authorization.FilterProxies
{
    public class AdminApiAuthorizeFilterProxy : FilterProxy
    {
        private readonly MemberConfigRepository _memberConfigRepository = null;

        public AdminApiAuthorizeFilterProxy(
            MemberConfigRepository memberConfigRepository
            )
        {
            _memberConfigRepository = memberConfigRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext, params object[] args)
        {
            base.OnActionExecuting(filterContext);

            var ip = filterContext.HttpContext.Request.UserHostAddress;
            if (ip == "::1") ip = "127.0.0.1";

            var adminIp = _memberConfigRepository.AdminIP;

            if (ip != adminIp)
            {
                filterContext.Result = getResult(new BaseResult
                {
                    RtnCode = 0,
                    RtnMsg = "非合法IP"
                });
                return;
            }
        }

        private ContentResult getResult(BaseResult result)
        {
            return new ContentResult
            {
                ContentEncoding = Encoding.UTF8,
                ContentType = MimeTypes.ApplicationJson,
                Content = JsonConvert.SerializeObject(new BaseResult
                {
                    RtnCode = result.RtnCode,
                    RtnMsg = result.RtnMsg
                }),
            };
        }
    }
}
