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
using ICP.Library.Services.AppRssLibrary;

namespace ICP.Modules.Mvc.Member.Controllers
{
    public class TestController : BaseMvcController
    {
        private readonly IUserManager _userManager = null;
        private readonly ILogger _logger = null;
        private AppRssService _appRssService;

        public TestController(
            IAuthorizationFactory authorizationFactory,
            ILogger<TestController> logger, 
            AppRssService appRssService)
        {
            _userManager = authorizationFactory.Create(AuthorizationType.Mvc);
            _logger = logger;
            _appRssService = appRssService;
        }

        [LogRequest(Type = typeof(TestController))]
        public ActionResult Test(string asdf)
        {
            return Content(base.GetType().FullName);
        }

        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationMvc)]
        [LogRequest(Name = "TestView")]
        public ActionResult TestView()
        {
            ViewBag.MID = _userManager.MID;
            return View();
        }

        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationMvc)]
        [LogRequest(Name = "TestAppRssPush")]
        public ActionResult TestAppRssPushResult()
        {
            ViewBag.MID = _userManager.MID;
            return View();
        }
    }
}