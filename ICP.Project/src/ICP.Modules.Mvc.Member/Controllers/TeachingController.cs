using ICP.Infrastructure.Abstractions;
using ICP.Infrastructure.Abstractions.Authorization;
using ICP.Infrastructure.Abstractions.FilterProxy;
using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Web.Attributes;
using ICP.Infrastructure.Core.Web.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Member.Controllers
{
    public class TeachingController : BaseMvcController
    {
        private readonly IUserManager _userManager = null;
        private readonly ILogger _logger = null;

        public TeachingController(
            IAuthorizationFactory authorizationFactory,
            ILogger<TeachingController> logger)
        {
            _userManager = authorizationFactory.Create(AuthorizationType.Mvc);
            _logger = logger;
        }
        /// <summary>
        /// 使用教學
        /// </summary>
        /// <returns></returns>
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationMvc)]
        [LogRequest(Type = typeof(TeachingController))]
        public ActionResult Teaching()
        {
            return View();
        }
    }
}