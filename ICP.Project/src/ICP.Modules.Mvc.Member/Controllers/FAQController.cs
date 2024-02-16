using ICP.Infrastructure.Abstractions.Authorization;
using ICP.Infrastructure.Abstractions.FilterProxy;
using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Web.Attributes;
using ICP.Infrastructure.Core.Web.Controllers;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Member.Controllers
{
    public class FAQController : BaseMvcController
    {
        private readonly IUserManager _userManager = null;
        private readonly ILogger _logger = null;

        public FAQController(
            IAuthorizationFactory authorizationFactory,
            ILogger<FAQController> logger)
        {
            _userManager = authorizationFactory.Create(AuthorizationType.Mvc);
            _logger = logger;
        }
        /// <summary>
        /// 常見問題清單
        /// </summary>
        /// <returns></returns>
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationMvc)]
        [LogRequest(Type = typeof(FAQController))]
        public ActionResult FAQList()
        {
            return View();
        }
        /// <summary>
        /// 儲值問題
        /// </summary>
        /// <returns></returns>
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationMvc)]
        [LogRequest(Name = "RefillFAQ")]
        public ActionResult RefillFAQ()
        {
            return View();
        }
    }
}