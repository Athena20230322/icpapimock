using ICP.Infrastructure.Abstractions.Authorization;
using ICP.Infrastructure.Abstractions.FilterProxy;
using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Web.Attributes;
using ICP.Infrastructure.Core.Web.Controllers;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Member.Controllers
{
    public class TermsController : BaseMvcController
    {
        private readonly IUserManager _userManager = null;
        private readonly ILogger _logger = null;

        public TermsController(
            IAuthorizationFactory authorizationFactory,
            ILogger<TermsController> logger)
        {
            _userManager = authorizationFactory.Create(AuthorizationType.Mvc);
            _logger = logger;
        }
        /// <summary>
        /// 使用條款
        /// </summary>
        /// <returns></returns>
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationMvc)]
        [LogRequest(Name = "Terms")]
        public ActionResult Terms()
        {
            return View();
        }
        /// <summary>
        /// 使用者契約
        /// </summary>
        /// <returns></returns>
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationMvc)]
        [LogRequest(Name = "UserTerms")]
        public ActionResult UserTerms()
        {
            return View();
        }
    }
}