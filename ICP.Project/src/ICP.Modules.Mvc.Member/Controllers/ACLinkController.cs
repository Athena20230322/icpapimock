using AutoMapper;
using BotDetect.Web.Mvc;
using ICP.Infrastructure.Abstractions.Authorization;
using ICP.Infrastructure.Abstractions.FilterProxy;
using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Web.Attributes;
using ICP.Infrastructure.Core.Web.Controllers;
using ICP.Modules.Api.Member.Commands;
using ICP.Modules.Api.Member.Models.ACLink;
using ICP.Modules.Mvc.Member.Commands;
using ICP.Modules.Mvc.Member.Models.ACLink;
using Newtonsoft.Json;
using System;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Member.Controllers
{
    public class ACLinkController : BaseMvcController
    {
        private readonly IUserManager _userManager = null;
        private readonly IAuthorizationFactory _authorizationFactory = null;
        private ACLinkCommand _acLinkCommand = null;
        private ACLinkApiCommand _acLinkApiCommand = null;
        private readonly ILogger _logger = null;
        MemberInfoCommand _memberInfoCommand;

        public ACLinkController(
            IAuthorizationFactory authorizationFactory,
            MemberInfoCommand memberInfoCommand,
            ACLinkCommand acLinkCommand,
            ACLinkApiCommand acLinkApiCommand,
            ILogger<ACLinkController> logger
            )
        {
            _authorizationFactory = authorizationFactory;
            _userManager = authorizationFactory.Create(AuthorizationType.Mvc);
            _memberInfoCommand = memberInfoCommand;
            _acLinkCommand = acLinkCommand;
            _acLinkApiCommand = acLinkApiCommand;
            _logger = logger;
        }

        /// <summary>
        /// AccountLink綁定結果頁
        /// </summary>
        /// <param name="bankCode"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public ActionResult BindResult(string bankCode, string t)
        {
            _logger.Info($"AccountLink綁定結果頁: bankCode:{bankCode}, t:{t}");

            ViewBag.Mode = 0;   //0:內開 1:外開瀏覽器

            if (bankCode == "822")//中國信託
            {
                // 取得綁定結果
                ViewBag.Success = t == "0000" ? "Y" : "N";
                ViewBag.RtnMsg = _acLinkCommand.GetChinaTrustRtnMsg(t);
            }
            else if (bankCode == "007")//第一銀行
            {
                // 取得綁定結果
                ViewBag.Mode = 1;

                if (!string.IsNullOrEmpty(Request.QueryString["RTN_MSG"]))
                {
                    ViewBag.Success = "N";
                    ViewBag.RtnMsg = Request.QueryString["RTN_MSG"];
                }
                else
                {
                    ViewBag.Success = "Y";
                    ViewBag.RtnMsg = "";
                }
            }
            else if (bankCode == "013")//國泰世華
            {
                // 取得綁定結果
                ViewBag.Success = t == "0000" ? "Y" : "N";
                ViewBag.RtnMsg = t == "0000" ? "" : t;
                ViewBag.Mode = 1;
            }
            else
            {
                ViewBag.Success = "N";
                ViewBag.RtnMsg = "綁定異常";
            }

            return View();
        }

        #region 中國信託
        /// <summary>
        /// 中國信託電子支付連結存款帳戶約定條款
        /// </summary>
        /// <returns></returns>
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationMvc)]
        public ActionResult ChinaTrustTermsPage()
        {
            return View();
        }

        /// <summary>
        /// 中國信託申請帳戶連結綁定資訊輸入頁
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationMvc)]
        public ActionResult ChinaTrustACLinkApplyInfo()
        {
            long MID = _userManager.MID;
            ViewBag.agreeTime = DateTime.Now.ToString("yyyyMMddHHmmss");

            _logger.Info($"會員同意帳戶連結綁定條款: MID:{MID}, agreeTime:{ViewBag.agreeTime}");

            return View();
        }

        /// <summary>
        /// 中國信託申請帳戶連結綁定資訊確認頁
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationMvc)]
        [LogRequest(LogTextResponse = true)]
        [CaptchaValidation("captchaCode", "captchaId", "驗證碼錯誤")]
        public ActionResult ChinaTrustACLinkInfoConfirm(ChinaTrustACLinkApplyReq model)
        {
            _logger.Trace($"ChinaTrustACLinkInfoConfirm [Input]: {JsonConvert.SerializeObject(model)}");

            if (!ModelState.IsValid)
            {
                ViewBag.bankAccount = model.BankAccount;
                ViewBag.birth = model.Birth;
                ViewBag.errorMsg = "驗證碼錯誤";
                return View("ChinaTrustACLinkApplyInfo");
            }

            // 取得會員資料
            long MID = _userManager.MID;
            string idno = string.Empty;
            if (MID != 0)
            {
                var result = _memberInfoCommand.GetLoginInfo(MID);
                idno = result.RtnData.Idno;
            }

            ViewBag.idno = idno;
            ViewBag.mid = MID;
            ViewBag.birth = model.Birth;
            ViewBag.bankAccount = model.BankAccount;
            ViewBag.agreeTime = model.AgreeTime;

            return View();
        }

        /// <summary>
        /// 中國信託申請帳戶連結綁定資訊OTP驗證頁
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationMvc)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult ChinaTrustOtpAuth(ChinaTrustACLinkApplyReq model)
        {
            string authId = string.Empty;
            string bankRtnCode = "0";
            string bankCode = "822";

            _logger.Trace($"ChinaTrustOtpAuth [Input]: {JsonConvert.SerializeObject(model)}");

            // 組成送至ACLinkApi資料
            var getPostData = _acLinkCommand.ChinaTrustACLinkApply(model);
            if (getPostData.IsSuccess)
            {
                _logger.Trace($"ChinaTrustOtpAuth [送至ACLinkApi申請綁定]: {JsonConvert.SerializeObject(getPostData)}");

                // 送至ACLinkApi申請綁定
                ACLinkApplyReq acLinkReq = Mapper.Map<ACLinkApplyReq>(getPostData.RtnData);
                var result = _acLinkApiCommand.ACLinkApply(acLinkReq);

                _logger.Trace($"ChinaTrustOtpAuth [申請綁定回傳結果]: {JsonConvert.SerializeObject(result)}");

                if (result.RtnData != null)
                {
                    authId = result.RtnData.AuthId == null ? "" : result.RtnData.AuthId;
                    bankRtnCode = result.RtnData.ServiceCode == null ? result.RtnCode.ToString() : result.RtnData.ServiceCode;
                }
                else
                {
                    bankRtnCode = result.RtnCode.ToString();
                }

                // 綁定失敗: 導至綁定結果頁
                if (!result.IsSuccess || string.IsNullOrWhiteSpace(authId))
                {
                    return RedirectToAction("BindResult", new { t = bankRtnCode, bankCode });
                }
            }
            else
            {
                bankRtnCode = getPostData.RtnCode.ToString();
                return RedirectToAction("BindResult", new { t = bankRtnCode, bankCode });
            }

            ViewBag.idno = model.IDNO;
            ViewBag.mid = model.MID;
            ViewBag.birth = model.Birth;
            ViewBag.bankAccount = model.BankAccount;
            ViewBag.agreeTime = model.AgreeTime;

            // 取得回傳資訊顯示OTP驗證
            ViewBag.AuthId = authId;

            return View();
        }

        /// <summary>
        /// 送至ACLinkApi綁定
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationMvc)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult ChinaTrustACLinkBind(ChinaTrustACLinkApplyReq model)
        {
            string bankRtnCode = "0";
            string bankCode = "822";

            _logger.Trace($"ChinaTrustACLinkBind [Input]: {JsonConvert.SerializeObject(model)}");

            // 組成送至ACLinkApi資料
            var getPostData = _acLinkCommand.ChinaTrustACLinkBind(model);
            if(getPostData.IsSuccess)
            {
                _logger.Trace($"ChinaTrustACLinkBind [送至ACLinkApi綁定]: {JsonConvert.SerializeObject(getPostData)}");

                // 送至ACLinkApi綁定
                ACLinkBindReq acLinkReq = Mapper.Map<ACLinkBindReq>(getPostData.RtnData);
                var result = _acLinkApiCommand.ACLinkBind(acLinkReq);

                _logger.Trace($"ChinaTrustACLinkBind [綁定回傳結果]: {JsonConvert.SerializeObject(result)}");

                if (result.RtnData != null)
                {
                    bankRtnCode = result.RtnData.ServiceCode == null ? result.RtnCode.ToString() : result.RtnData.ServiceCode;
                }
                else
                {
                    bankRtnCode = result.RtnCode.ToString();
                }
            }
            else
            {
                bankRtnCode = getPostData.RtnCode.ToString();
            }

            // 導至綁定結果頁
            return RedirectToAction("BindResult", new { t = bankRtnCode, bankCode });
        }
        #endregion

        #region Mock
        /// <summary>
        /// 模擬取消連結帳戶綁定
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ContentResult MockACLinkCancel(long AccountID)
        {
            // 組成送至ACLinkApi資料
            ACLinkCancelBindReq postData = new ACLinkCancelBindReq
            {
                MID = 10010115,
                IDNO = "A123456789",
                BankCode = "822",
                AccountID = AccountID
            };

            // 送至ACLinkApi取消連結帳戶綁定
            var result = _acLinkApiCommand.ACLinkCancel(postData);
            string rtnResult = string.Format("RtnCode:{0}, RtnMsg:{1}", result.RtnCode, result.RtnMsg);

            return Content(rtnResult);
        }

        /// <summary>
        /// 模擬提領至約定帳戶
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ContentResult MockACLinkWithdrawal()
        {
            // 組成送至ACLinkApi資料
            ACLinkWithdrawalReq postData = new ACLinkWithdrawalReq
            {
                MID = 10010115,
                IDNO = "A123456789",
                BankCode = "822",
                INDTAccount = "12345678901234",
                Amount = 100,
                TradeNo = "10010011110005"
            };

            // 送至ACLinkApi提領至約定帳戶
            var result = _acLinkApiCommand.ACLinkWithdrawal(postData);
            string rtnResult = string.Format("RtnCode:{0}, RtnMsg:{1}", result.RtnCode, result.RtnMsg);

            return Content(rtnResult);
        }
        #endregion
    }
}
