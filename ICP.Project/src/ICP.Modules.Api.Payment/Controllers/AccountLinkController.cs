using ICP.Infrastructure.Abstractions.Authorization;
using ICP.Infrastructure.Abstractions.FilterProxy;
using ICP.Infrastructure.Core.Web.Attributes;
using ICP.Infrastructure.Core.Web.Controllers;
using ICP.Modules.Api.Payment.Commands;
using ICP.Modules.Api.Payment.Models.ACLink;
using System.Web.Mvc;

namespace ICP.Modules.Api.Payment.Controllers
{
    public class AccountLinkController : BaseApiController
    {
        private readonly IUserManager _userManager = null;
        public readonly AccountLinkCommand _acLinkCommand = null;

        public AccountLinkController(
            IAuthorizationFactory authorizationFactory, 
            AccountLinkCommand accountLinkCommand)
        {
            _userManager = authorizationFactory.Create(AuthorizationType.Api);
            _acLinkCommand = accountLinkCommand;
        }

        /// <summary>
        /// 用AccountLink儲值帳戶金額
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult TopUpByAccountLink(ACLinkTopUpReq request)
        {
            request.MID = _userManager.MID;
            var result = _acLinkCommand.TopUpByAccountLink(request);

            return AppResult(result);
        }
    }
}
