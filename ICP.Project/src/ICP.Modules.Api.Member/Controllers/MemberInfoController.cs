using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ICP.Modules.Api.Member.Controllers
{
    using System.Web;
    using ICP.Infrastructure.Core.Helpers;
    using Infrastructure.Abstractions.Authorization;
    using Infrastructure.Abstractions.FilterProxy;
    using Infrastructure.Core.Web.Attributes;
    using Infrastructure.Core.Web.Controllers;
    using Library.Models.AuthorizationApi;
    using Library.Repositories.MemberRepositories;
    using Modules.Api.Member.Commands;
    using Modules.Api.Member.Models.Certificate;
    using Modules.Api.Member.Models.MemberInfo;
    using ICP.Library.Repositories.MemberRepositories;

    public class MemberInfoController : BaseApiController
    {
        private readonly IUserManager _userManager = null;
        private readonly IAuthorizationFactory _authorizationFactory = null;
        MemberInfoCommand _memberInfoCommand;
        MemberBankCommand _memberBankCommand;
        AdminCommand _adminCommand;
        MemberConfigRepository _memberConfigRepository;

        public MemberInfoController(
            MemberInfoCommand memberInfoCommand,
            IAuthorizationFactory authorizationFactory,
            MemberBankCommand memberBankCommand,
            AdminCommand adminCommand,
            MemberConfigRepository memberConfigRepository
            )
        {
            _authorizationFactory = authorizationFactory;
            _userManager = authorizationFactory.Create(AuthorizationType.Api);
            _memberInfoCommand = memberInfoCommand;
            _memberBankCommand = memberBankCommand;
            _adminCommand = adminCommand;
            _memberConfigRepository = memberConfigRepository;
        }

        #region M0000 取得選單XML
        [HttpPost]
        [AllowAnonymous]
        [AllowOPAnonymous]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult getAppXmlSetting(GetAppXmlSettingRequest request)
        {
            long MID = _userManager.MID;

            string APPXMLPath = Server.MapPath(_memberConfigRepository.APPXMLPath);

            var result = _memberInfoCommand.GetAppXmlSetting(MID, request, APPXMLPath);

            return AppResult(result);
        }
        #endregion

        #region M0001 取得手機號碼
        [HttpPost]
        [AllowAnonymous]
        [AllowOPAnonymous]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult getCellphone(GetCellphoneRequest request)
        {
            if (_memberConfigRepository.AllowMock)
            {
                if (!string.IsNullOrEmpty(request.UserCode))
                {
                    var mockResult = _memberInfoCommand.GetCellphone_Mock(request.UserCode);

                    return AppResult(mockResult);
                }
            }

            var result = _memberInfoCommand.GetCellphone(request);

            return AppResult(result);
        }
        #endregion

        #region M0002 檢查會員驗證狀態
        [HttpPost]
        [AllowAnonymous]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult CheckVerifyStatus()
        {
            long MID = _userManager.MID;

            var result = _memberInfoCommand.CheckVerifyStatus(MID);

            return AppResult(result);
        }
        #endregion

        #region M0003 設定註冊資料
        [HttpPost]
        [AllowAnonymous]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult SetRegisterInfo(SetRegisterInfoRequest request)
        {
            string OPMID = _userManager.GetData<string>(UserDataType.OPMID);

            var result = _memberInfoCommand.SetRegisterInfo(OPMID, request, RealIP, ProxyIP);

            return AppResult(result);
        }
        #endregion

        #region M0004 會員資料填寫及驗證
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult AuthIDNO(AuthIDNORequest request, HttpPostedFileBase img1, HttpPostedFileBase img2)
        {
            long MID = _userManager.MID;

            request.Files = new HttpPostedFileBase[] { img1, img2 };

            var result = _memberInfoCommand.AuthIDNO(MID, request, base.RealIP, base.ProxyIP);

            return AppResult(result);
        }
        #endregion

        #region M0005 會員登入
        [HttpPost]
        [AllowAnonymous]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(Masks = new string[] { "request.UserCode", "request.UserPwd" }, LogTextResponse = true)]
        public ActionResult UserCodeLogin(UserCodeLoginRequest request)
        {
            long AppTokenID = _userManager.GetData<long>(UserDataType.AppTokenID);

            var keyContext = _userManager.GetData<AuthorizationApiKeyContext>(UserDataType.AuthorizationApiKeyContext);

            long ClientCertId = keyContext.ClientAesCert.ClientCertId;

            var result = _memberInfoCommand.UserCodeLogin(AppTokenID, ClientCertId, request, RealIP, ProxyIP);

            return AppResult(result);
        }
        #endregion

        #region M0006 取得登入回傳資訊
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult GetLoginInfo()
        {
            long MID = _userManager.MID;

            var result = _memberInfoCommand.GetLoginInfo(MID);

            return AppResult(result);
        }
        #endregion
        
        #region M0007 發送簡訊驗證_共用
        [HttpPost]
        [AllowAnonymous]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult SendAuthSMS(SendAuthSMSRequest request)
        {
            long AppTokenID = _userManager.GetData<long>(UserDataType.AppTokenID);

            long MID = _userManager.MID;

            var result = _memberInfoCommand.SendAuthSMS(AppTokenID, MID, request, RealIP, ProxyIP);

            return AppResult(result);
        }
        #endregion

        #region M0008 發送簡訊驗證_修改手機號碼
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult ChangePhoneSendAuthSMS(ChangePhoneSendAuthSMSRequest request)
        {
            long MID = _userManager.MID;

            var result = _memberInfoCommand.ChangePhoneSendAuthSMS(MID, request, RealIP, ProxyIP);

            return AppResult(result);
        }
        #endregion

        #region M0009 檢查簡訊驗證碼_共用
        [HttpPost]
        [AllowAnonymous]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult CheckAuthSMS(CheckAuthSMSRequest request)
        {
            long AppTokenID = _userManager.GetData<long>(UserDataType.AppTokenID);

            var result = _memberInfoCommand.CheckAuthSMS(AppTokenID, request);

            return AppResult(result);
        }
        #endregion

        #region M0010 檢查簡訊驗證碼_註冊
        [HttpPost]
        [AllowAnonymous]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult CheckRegisterAuthSMS(CheckRegisterAuthSMSRequest request)
        {
            long AppTokenID = _userManager.GetData<long>(UserDataType.AppTokenID);

            var keyContext = _userManager.GetData<AuthorizationApiKeyContext>(UserDataType.AuthorizationApiKeyContext);

            long ClientCertId = keyContext.ClientAesCert.ClientCertId;

            var result = _memberInfoCommand.CheckRegisterAuthSMS(AppTokenID, ClientCertId, request);

            return AppResult(result);
        }
        #endregion
        
        #region M0011 變更登入密碼   
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult ChangeLoginPwd(ChangeLoginPwdRequest request)
        {
            long mid = _userManager.MID;

            var vaildChangeLoginPwd = _memberInfoCommand.VaildChangeLoginPwd(request);
            if (!vaildChangeLoginPwd.IsSuccess)
            {
                return AppResult(vaildChangeLoginPwd);
            }

            var result = _memberInfoCommand.ChangeLoginPwd(mid, request, RealIP, ProxyIP);

            if (result.IsSuccess)
            {
                var VerifyStatusResult = _memberInfoCommand.CheckVerifyStatus(mid);
                return AppResult(VerifyStatusResult);
            }

            return AppResult(result);
        }
        #endregion

        #region M0012 變更安全密碼  
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult ChangeSecurityPwd(ChangeSecurityPwdRequest request)
        {
            long mid = _userManager.MID;

            var result = _memberInfoCommand.ChangeSecurityPwd(mid, request, RealIP, ProxyIP);

            if (result.IsSuccess)
            {
               var VerifyStatusResult = _memberInfoCommand.CheckVerifyStatus(mid);
               return AppResult(VerifyStatusResult);
            }

            return AppResult(result);
        }
        #endregion

        #region M0013 檢查安全密碼 
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult CheckSecurityPwd(CheckPayPwdRequest request)
        {
            long mid = _userManager.MID;

            var result = _memberInfoCommand.CheckSecurityPwd(mid, request, RealIP, ProxyIP);

            return AppResult(result);

        }
        #endregion

        #region M0014 變更圖形密碼     
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult ChangeGraphicLock(ChangeGraphicLockRequest request)
        {
            long mid = _userManager.MID;

            var result = _memberInfoCommand.ChangeGraphicLock(mid, request, RealIP, ProxyIP);

            if (result.IsSuccess)
            {
                var VerifyStatusResult = _memberInfoCommand.CheckVerifyStatus(mid);
                return AppResult(VerifyStatusResult);
            }

            return AppResult(result);
        }
        #endregion

        #region M0015 檢查圖形密碼  
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult CheckGraphicLock(CheckGraphicLockRequest request)
        {
            long mid = _userManager.MID;

            var result = _memberInfoCommand.CheckGraphicLock(mid, request, RealIP, ProxyIP);

            return AppResult(result);
        }
        #endregion

        #region M0016 設定指紋辨識及FaceID開關       
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult UpdateFingerPrintPwdStatus(FingerPrintPwdStatusRequest request)
        {
            long mid = _userManager.MID;

            var result = _memberInfoCommand.UpdateFingerPrintPwdStatus(mid, request, RealIP,  ProxyIP);

            return AppResult(result);
        }
        #endregion

        #region M0017 驗證指紋辨識及FaceID  
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult CheckFingerPrintPwd(CheckFingerPrintPwdRequest request)
        {

            long mid = _userManager.MID;

            var result = _memberInfoCommand.CheckFingerPrintPwd(mid, request, RealIP, ProxyIP);

            return AppResult(result);

        }
        #endregion

        #region M0018 設定圖形密碼開關
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult UpdateGraphicLockStatus(GraphicLockStatusRequest request)
        {
            long mid = _userManager.MID;

            var result = _memberInfoCommand.UpdateGraphicLockStatus(mid, request, RealIP, ProxyIP);

            return AppResult(result);

        }
        #endregion

        #region M0019 取得密碼設定開關狀態   
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult GetPasswordStatus(GetPasswordStatusRequest request)
        {
            long mid = _userManager.MID;

            var result = _memberInfoCommand.GetPasswordStatus(mid, request);

            return AppResult(result);

        }
        #endregion

        #region M0024 重設登入密碼(未登入)
        [HttpPost]
        [AllowAnonymous]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult ResetLoginPwd(ResetLoginPwdRequest request)
        {
            long AppTokenID = _userManager.GetData<long>(UserDataType.AppTokenID);

            var result = _memberInfoCommand.ResetLoginPwd(AppTokenID, request, RealIP, ProxyIP);

            return AppResult(result);
        }
        #endregion

        #region M0023 修改Email
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult UpdateEmailAddress(UpdateEmailAddressRequest request)
        {
            long mid = _userManager.MID;

            var result = _memberInfoCommand.UpdateEmailAddress(mid, request);

            return AppResult(result);
        }
        #endregion

        #region M0022 修改暱稱
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult UpdateNickName(UpdateNickNameRequest request)
        {
            long mid = _userManager.MID;

            var result = _memberInfoCommand.UpdateNickName(mid, request);

            return AppResult(result);

        }

        #endregion

        #region M0039 記錄略過修改安全密碼
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult SetSecPwdIgnorDate()
        {
            
            long mid = _userManager.MID;
            
            var result = _memberInfoCommand.SetPayPwdIgnoreDate(mid);

            return AppResult(result);

        }

        #endregion

        #region M0040 記錄略過修改圖形密碼
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult SetGraphicLockIgnorDate()
        {
            long mid = _userManager.MID;

            var result = _memberInfoCommand.SetGraphicLockIgnorDate(mid, RealIP, ProxyIP);

            return AppResult(result);

        }

        #endregion
               
        #region M0038 記錄略過修改登入密碼
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult SetLoginPwdIgnorDate()
        {
            long mid = _userManager.MID;

            var result = _memberInfoCommand.SetLoginPwdIgnorDate(mid);

            return AppResult(result);

        }

        #endregion


        #region M0021 取得會員偏好設定
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult GetMemberPreferences(GetMemberPreferencesRequest request)
        {
            long MID = _userManager.MID;

            var result = _memberInfoCommand.GetMemberPreferences(MID, request);

            return AppResult(result);
        }
        #endregion


        #region M0025 會員登出
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult UserCodeLogout()
        {
            long MID = _userManager.MID;

            var keyContext = _userManager.GetData<AuthorizationApiKeyContext>(UserDataType.AuthorizationApiKeyContext);

            long ClientCertId = keyContext.ClientAesCert.ClientCertId;

            var result = _memberInfoCommand.Logout(MID, ClientCertId);

            _userManager.Logout();

            return AppResult(result);
        }
        #endregion


        #region M0030 取得指定範圍訊息中心清單
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult GetRangeNotifyMessageList(GetRangeNotifyMessageListRequest request)
        {
            long MID = _userManager.MID;

            var result = _memberInfoCommand.GetRangeNotifyMessageList(MID, request);

            return AppResult(result);
        }
        #endregion

        #region M0032 設定訊息已讀
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult SetReadMessage(SetReadMessageRequest request)
        {
            long MID = _userManager.MID;

            var result = _memberInfoCommand.SetReadMessage(MID, request);

            return AppResult(result);
        }
        #endregion

        #region M0033 刪除訊息
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult DeleteNotifyMessage(DeleteNotifyMessageRequest request)
        {
            long MID = _userManager.MID;

            var result = _memberInfoCommand.DeleteNotifyMessage(MID, request);

            return AppResult(result);
        }
        #endregion

        #region 訊息中心查詢
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult ListNotifyMessage()
        {
            long MID = _userManager.MID;

            var result = _memberInfoCommand.ListNotifyMessage(MID);

            return AppResult(result);
        }
        #endregion

        #region M0076 訊息中心同步
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult ListSynNotifyMessage(ListSynNotifyMessageRequest request)
        {
            long MID = _userManager.MID;

            var result = _memberInfoCommand.ListSynNotifyMessage(MID, request);

            return AppResult(result);
        }
        #endregion
        
        #region 取得未讀數量
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult GetAppNotifyCounts()
        {
            long MID = _userManager.MID;

            var result = _memberInfoCommand.GetAppNotifyCounts(MID);

            return AppResult(result);
        }
        #endregion

        #region M0077 取得訊息內容
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult GetNotifyMessage(GetNotifyMessageRequest request)
        {
            long MID = _userManager.MID;

            var result = _memberInfoCommand.GetNotifyMessage(MID, request);

            return AppResult(result);
        }
        #endregion

        #region M0026 取得銀行清單
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult GetListBankInfo()
        {
            var result = _memberInfoCommand.GetListBankInfo();

            return AppResult(result);
        }
        #endregion

        #region M0027 銀行帳戶驗證
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult BankAccountAuth(BankAccountAuthRequest request)
        {
            long MID = _userManager.MID;

            var result = _memberBankCommand.BankAccountAuth(MID, request);

            return AppResult(result);
        }
        #endregion

        #region M0028 取得提領資訊
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult GetWithdrawBalanceInfo(BaseAuthorizationApiRequest request)
        {
            long MID = _userManager.MID;

            var result = _memberInfoCommand.GetWithdrawBalanceInfo(MID);

            return AppResult(result);
        }
        #endregion

        #region M0029 更新會員同意項目
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult SetMemberAgree(SetMemberAgreeRequestItems request)
        {
            long MID = _userManager.MID;

            var result = _memberInfoCommand.SetMemberAgree(MID, request, RealIP, ProxyIP);

            return AppResult(result);
        }
        #endregion

        #region M0035 刪除綁定銀行帳戶
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult DelAccountBind(DelAccountBindRequest request)
        {
            long MID = _userManager.MID;

            var result = _memberBankCommand.DelAccountBind(MID, request);

            return AppResult(result);
        }
        #endregion

        #region M0036 取得綁定銀行帳號
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult GetBindAccountList(GetBindAccountListRequest request)
        {
            long MID = _userManager.MID;

            var result = _memberInfoCommand.GetBindAccountList(MID, request);

            return AppResult(result);
        }
        #endregion

        #region M0020 會員偏好設定
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult UpdateMemberPreferences(UpdateMemberPreferencesRequest request)
        {
            long MID = _userManager.MID;

            var result = _memberInfoCommand.UpdateMemberPreferences(MID, request, RealIP, ProxyIP);

            return AppResult(result);
        }
        #endregion


        #region M0041 Socket Server使用者驗證
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult PrivateUserAuth()
        {
            long MID = _userManager.MID;

            var result = _memberInfoCommand.PrivateUserAuth(MID);

            return AppResult(result);
        }
        #endregion

        #region M0042 取得載具資訊

        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult GetEInvoiceCarrierInfo()
        {
            long MID = _userManager.MID;

            var result = _memberInfoCommand.GetEInvoiceCarrierInfo(MID);

            return AppResult(result);
        }
        #endregion

        #region M0043 驗證手機條碼
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult AuthCellPhoneCarrier(GetEInvoiceCarrierInfoRequest request)
        {
            long MID = _userManager.MID;
            
            var result = _memberInfoCommand.AuthCellPhoneCarrier(request, MID);

            return AppResult(result);
        }


        #endregion

        #region M0044 取得發票清單

        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult QueryEinvoiceList(QueryEinvoiceListRequest request)
        {
            long MID = _userManager.MID;

            var result = _memberInfoCommand.QueryEinvoiceList(MID, request);

            return AppResult(result);
        }

        #endregion

        #region M0045 取得發票明細

        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult QueryEinvoiceDetail(QueryEinvoiceDetailRequest request)
        {
            long MID = _userManager.MID;

            var result = _memberInfoCommand.QueryEinvoiceDetail(MID, request);

            return AppResult(result);
        }

        #endregion

        #region M0052 新增提領轉入帳戶
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult AddWithdrawAccountBind(AddWithdrawAccountBindRequest request)
        {
            long MID = _userManager.MID;

            var result = _memberInfoCommand.AddWithdrawAccountBind(MID, request);

            return AppResult(result);
        }
        #endregion

        #region M0053 餘額提領
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult AddWithdrawBalance(AddWithdrawBalanceRequest request)
        {
            long MID = _userManager.MID;

            var result = _memberBankCommand.AddWithdrawBalance(MID, request, RealIP, ProxyIP);

            return AppResult(result);
        }
        #endregion

        #region M0037 首次登入設定帳密
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        public ActionResult FirstLoginSetAccount(FirstLoginSetAccountRequest request)
        {
            long mid = _userManager.MID;

            var result = _memberInfoCommand.FirstLoginSetAccount(mid, request, RealIP, ProxyIP);

            return AppResult(result);

    }
        #endregion

        #region M0046 取得帳戶狀態資訊
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult GetAccountStatusInfo()
        {
            long MID = _userManager.MID;

            var result = _adminCommand.GetAccountStatusInfo(MID);

            return AppResult(result);
        }
        #endregion

        #region M0047 帳戶結清
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult CloseMemberAccount()
        {
            long MID = _userManager.MID;

            byte Source = 1;

            var result = _adminCommand.CloseMemberAccount(MID, Source, RealIP, ProxyIP);

            return AppResult(result);
        }
        #endregion

        #region M0058 取得系統維護開關
        [HttpPost]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        [LogRequest(LogTextResponse = true)]
        public ActionResult GetMaintainStatus(GetMaintainStatusRequest request)
        {
            var result = _memberInfoCommand.GetMaintainStatus(request);

            return AppResult(result);
        }
        #endregion

        #region M0075 取得分行代碼清單
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        public ActionResult ListBankBranch(ListBankBranchRequest request)
        {
            var result = _memberInfoCommand.ListBankBranch(request);

            return AppResult(result);
        }
        #endregion
    }
}
