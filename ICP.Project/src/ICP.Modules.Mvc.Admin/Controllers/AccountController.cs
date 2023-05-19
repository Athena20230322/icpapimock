using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web.Mvc;

namespace ICP.Modules.Mvc.Admin.Controllers
{
    using Attributes;
    using Models;
    using Commands;
    using Infrastructure.Core.Web.Extensions;
    using ICP.Modules.Mvc.Admin.Repositories;

    public class AccountController : BaseAdminController
    {
        AccountCommand _accountCommand;

        public AccountController(
            AccountCommand accountCommand
            )
        {
            _accountCommand = accountCommand;
        }

        public ActionResult Index()
        {
            return View();
        }

        #region 修改密碼
        public ActionResult ChangePwd(byte Expire = 0)
        {
            ViewBag.Expire = Expire;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePwd(AccountChangePwdModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string redirectUrl = Url.Action("Content", "Frame");
            if (_accountCommand.CheckPwdExpire(base.CurrentUserID))
            {
                redirectUrl = Url.Action("Index", "Frame");
            }

            string errorMsg = null;
            if (!_accountCommand.ChangeUserPwd(base.CurrentUserID, model.OriginPwd, model.Pwd, RealIP, ref errorMsg))
            {
                ModelState.AddModelError("", errorMsg);
                return View(model);
            }

            return RedirectAndAlert(redirectUrl, "修改密碼成功");
        }
        #endregion

        #region 設定新密碼
        [AllowAnonymous]
        public ActionResult AccountResetPwd()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult AccountResetPwd(AccountResetPwdModel model)
        {
            var result = _accountCommand.AccountResetPwd(CurrentUserID, model, RealIP);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.RtnMsg);
                return View();
            }

            return RedirectAndAlert(Url.Action("Logout"), "密碼設定成功，請重新登入");
        }
        #endregion

        #region 登入
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AccountLoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string errorMsg = null;
            int userID = 0;
            if (!_accountCommand.UserLogin(model.Account, model.Pwd, RealIP, ref errorMsg, ref userID))
            {
                ModelState.AddModelError("", errorMsg);
                return View(model);
            }

            var SecurityResult = _accountCommand.GetUserSecurity(userID);
            UserSecurity security = SecurityResult.RtnData;
            if (security.LastChangePwdAt == null)
            {
                return RedirectToAction("AccountResetPwd", "Account");
            }

            return RedirectToAction("Index", "Frame");
        }
        #endregion

        #region 登出
        [AllowAnonymous]
        public ActionResult Logout(string msg = null)
        {
            _accountCommand.UserLogout();

            string script = string.Empty;

            if (!string.IsNullOrWhiteSpace(msg))
            {
                script += string.Format("<script>alter(escape('{0}'))</script>", System.Web.HttpUtility.UrlEncode(msg));
            }

            string url = System.Web.Security.FormsAuthentication.LoginUrl;
            script += string.Format("<script>top.location.href='{0}';</script>", url);

            return Content(script, "text/html", Encoding.UTF8);
        }
        #endregion

        #region 忘記密碼
        [AllowAnonymous]
        public ActionResult ForgetPwd()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgetPwd(AccountForgetPwdModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string errorMsg = null;
            if (!_accountCommand.ForgetPwd(model.Account, model.Email, ref errorMsg))
            {
                ModelState.AddModelError("", errorMsg);
                return View(model);
            }

            return RedirectAndAlert(Url.Action("Login"), "重設密碼連結已送出");
        }
        #endregion

        #region 重設密碼
        [AllowAnonymous]
        public ActionResult ResetPwd(string Token)
        {
            string errorMsg = null;
            if (!_accountCommand.CheckUserForgetToken(Token, ref errorMsg))
            {
                return RedirectAndAlert(Url.Action("ForgetPwd"), errorMsg);
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPwd(string Token, AccountResetPwdModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string errorMsg = null;
            if (!_accountCommand.ResetUserPwd(Token, model.Pwd, RealIP, ref errorMsg))
            {
                return RedirectAndAlert(Url.Action("ResetPwd", new { Token }), errorMsg);
            }

            return RedirectAndAlert(System.Web.Security.FormsAuthentication.LoginUrl, "重設密碼成功，請重新登入");
        }
        #endregion
    }
}