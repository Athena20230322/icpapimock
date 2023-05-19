using ICP.Infrastructure.Abstractions.Authorization;
using ICP.Infrastructure.Abstractions.FilterProxy;
using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Web.Attributes;
using ICP.Infrastructure.Core.Web.Controllers;
using ICP.Modules.Mvc.Payment.Commands.BaseMember;
using ICP.Modules.Mvc.Payment.Models.BaseMember;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Payment.Controllers
{
    /// <summary>
    /// 帳戶資訊
    /// </summary>
    public class BaseMemberController : BaseMvcController
    {
        private readonly ILogger _logger = null;
        AccountRecordCommand _accountRecordCommand = null;
        private readonly IUserManager _userManager = null;

        public BaseMemberController
        (
            ILogger<BaseMemberController> logger,
            AccountRecordCommand accountRecordCommand,
            IAuthorizationFactory authorizationFactory
        )
        {
            _logger = logger;
            _accountRecordCommand = accountRecordCommand;
            _userManager = authorizationFactory.Create(AuthorizationType.Mvc);
        }

        /// <summary>
        /// 帳戶紀錄列表
        /// </summary>
        /// <returns></returns>
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationMvc)]
        [LogRequestByActionName(LogTextResponse = true)]
        public ActionResult ListAccountRecord()
        {
            ViewBag.MID = _userManager.MID;

            return View();
        }

        /// <summary>
        /// 帳戶紀錄列表
        /// </summary>
        /// <param name="request">查詢物件</param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationMvc)]
        [LogRequestByActionName(LogTextResponse = true)]
        public ActionResult AccountRecordDetail(AccountRecordReq request)
        {
            //檢查是否登入
            request.MID = _userManager.MID;
            _logger.Info("_userManager.MID=" + _userManager.MID.ToString());

            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = _accountRecordCommand.ListAccountRecord(request);

            if (Request.IsAjaxRequest())
            {
                return PartialView("AccountRecordDetail", result.RtnData);
            }
            else
            {
                return View(result.RtnData);
            }
        }
    }
}
