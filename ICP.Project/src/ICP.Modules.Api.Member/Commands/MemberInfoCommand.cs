using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ICP.Infrastructure.Core.Models.Consts;
using Castle.Core.Internal;
using ICP.Infrastructure.Core.Models.Consts;
using ICP.Library.Models.EinvoiceLibrary;
using ICP.Library.Services.EinvoiceLibrary;


namespace ICP.Modules.Api.Member.Commands
{
    using ICP.Infrastructure.Core.Helpers;
    using ICP.Infrastructure.Core.Utils;
    using ICP.Library.Repositories.MemberRepositories;
    using ICP.Library.Services.Payment;
    using Infrastructure.Core.Extensions;
    using Infrastructure.Core.Models;
    using Library.Models.MemberModels;
    using Library.Services.AdminServices;
    using Library.Services.MemberApi;
    using Library.Services.MemberServices;
    using Models.MemberInfo;
    using Modules.Api.Member.Enums;
    using Services;
    using ICP.Library.Services.Payment;
    using ICP.Library.Services.AppRssLibrary;
    using ICP.Library.Services.MailLibrary;

    public class MemberInfoCommand
    {
        MemberInfoService _memberInfoService;
        LibMemberInfoService _libMemberInfoService;
        LibMemberLoginService _libMemberLoginService;
        LibMemberRegService _libMemberRegService;
        LibMemberAuthService _libMemberAuthService;
        LibMemberBankService _libMemberBankService;
        LibPersonalAuthService _libPersonalAuthService;
		CertificateService _certificateService;
		MemberGraphicLockService _memberGraphicLockService;
        CommonService _commonService;
        CertificateMemberApiService _certificateMemberApiService;

        LibAdminService _libAdminService;
        MemberConfigCyptRepository _configCyptRepository;
		
        LibtMemberNotifyMessageService _libtMemberNotifyMessageService;
        EinvoiceAPPService _einvoiceAppService;
        AppManagementService _appManagementService;
        TopUpService _topUpService;
        NotifyManageService _notifyManageService;

        public MemberInfoCommand(
            MemberInfoService memberInfoService, 
            LibMemberInfoService libMemberInfoService, 
            LibMemberLoginService libMemberLoginService,
            LibMemberRegService libMemberRegService, 
            LibMemberAuthService libMemberAuthService,

            LibMemberBankService libMemberBankService,
            LibPersonalAuthService libPersonalAuthService,
            CertificateService certificateService,
            MemberGraphicLockService memberGraphicLockService,
            CommonService commonService,
            CertificateMemberApiService certificateMemberApiService,
            LibAdminService libAdminService,
            LibtMemberNotifyMessageService libtMemberNotifyMessageService,
            EinvoiceAPPService einvoiceAppService,
            MemberConfigCyptRepository memberConfigCyptRepository,
            AppManagementService appManagementService,
            TopUpService topUpService,
            NotifyManageService notifyManageService
            )
        {
            _memberInfoService = memberInfoService;
            _libMemberInfoService = libMemberInfoService;
            _libMemberLoginService = libMemberLoginService;
            _libMemberRegService = libMemberRegService;
            _libMemberAuthService = libMemberAuthService;
            _libMemberBankService = libMemberBankService;

            _libPersonalAuthService = libPersonalAuthService;
            _certificateService = certificateService;
            _memberGraphicLockService = memberGraphicLockService;
            _commonService = commonService;
            _certificateMemberApiService = certificateMemberApiService;
            _configCyptRepository = memberConfigCyptRepository;
            _libAdminService = libAdminService;
            _libtMemberNotifyMessageService = libtMemberNotifyMessageService;
            _einvoiceAppService = einvoiceAppService;
            _appManagementService = appManagementService;
            _topUpService = topUpService;
            _notifyManageService = notifyManageService;
        }

        public DataResult<GetAppXmlSettingResult> GetAppXmlSetting(long MID, GetAppXmlSettingRequest request,string APPXMLPath)
        {
            return _appManagementService.GetAppXmlSetting(MID, request, APPXMLPath);
        }

        public DataResult<GetCellphoneResult> GetCellphone_Mock(string UserCode)
        {
            var result = new DataResult<GetCellphoneResult>();
            result.SetError();

            var getResult = _memberInfoService.GetMemberAppTokenByAccount(UserCode);
            if (!getResult.IsSuccess)
            {
                result.SetError(getResult);
                return result;
            }

            return GetCellphone(getResult.RtnData);
        }

        public DataResult<GetCellphoneResult> GetCellphone(GetCellphoneRequest request)
        {
            var result = new DataResult<GetCellphoneResult>();
            result.SetError();

            //檢查資料格式
            if (!request.IsValid())
            {
                result.RtnMsg = request.GetFirstErrorMessage();
                return result;
            }

            bool mock = false;
            var authVCodeResult = _memberInfoService.GetMemberAuthVCode(request.AuthV, ref mock);
            if (!authVCodeResult.IsSuccess)
            {
                result.SetError(authVCodeResult);
                return result;
            }
            var authVCode = authVCodeResult.RtnData;

            //不存在則新増
            var getResult = _memberInfoService.GetMemberAppTokenByAuthV(authVCode);
            if (!getResult.IsSuccess)
            {
                var addResult = _memberInfoService.AddMemberAppToken(authVCode, mock);
                if (!addResult.IsSuccess)
                {
                    result.SetError(addResult);
                    return result;
                }

                getResult = _memberInfoService.GetMemberAppTokenByAuthV(authVCode);
            }

            return GetCellphone(getResult.RtnData);
        }

        public DataResult<GetCellphoneResult> GetCellphone(MemberAppToken appToken)
        {
            var result = new DataResult<GetCellphoneResult>();
            result.SetError();

            //過期更新
            if (appToken.LoginTokenExpired < DateTime.Now)
            {
                var updateExpiredResult = _memberInfoService.UpdateMemberAppTokenExpired(appToken.AppTokenID);
                if (!updateExpiredResult.IsSuccess)
                {
                    result.SetError(updateExpiredResult);
                    return result;
                }

                var getResult = _memberInfoService.GetMemberAppToken(appToken.AppTokenID);
                if (!getResult.IsSuccess)
                {
                    result.SetError(getResult);
                    return result;
                }

                appToken = getResult.RtnData;
            }

            var getCellPhoneResult = _libMemberInfoService.GetCellPhone(appToken.MID, appToken.OPCellPhone);
            if (!getCellPhoneResult.IsSuccess)
            {
                result.SetError(getCellPhoneResult);
                return result;
            }

            string CellPhone = getCellPhoneResult.RtnData;
            result.SetSuccess(new GetCellphoneResult
            {
                CellPhone = CellPhone,
                LoginTokenID = appToken.LoginTokenID,
                LoginTokenExpired = appToken.LoginTokenExpired,
                RegisterStatus = _memberInfoService.GetRegisterStatus(appToken.MID)
            });
            return result;
        }

        public DataResult<CheckVerifyStatusResult> CheckVerifyStatus(long MID)
        {
            var result = new DataResult<CheckVerifyStatusResult>();
            result.SetError();

            var verifyStatus = _memberInfoService.GetMemberVerifyStatus(MID);

            var rtnData = _memberInfoService.GetCheckVerifyStatusResultResult(verifyStatus);

            result.SetSuccess(rtnData);
            return result;
        }

        public DataResult<BaseResult> SetRegisterInfo(string OPMID, SetRegisterInfoRequest request, long RealIP = 0, long ProxyIP = 0)
        {
            var result = new DataResult<BaseResult>();
            result.SetError();
                        

            //檢查資料格式
            if (!request.IsValid())
            {
                result.RtnMsg = request.GetFirstErrorMessage();
                return result;
            }

            // 檢查帳號格式
            var checkFormatResult = _libMemberInfoService.CheckFormat_UserCode(request.UserCode);
            if (!checkFormatResult.IsSuccess)
            {
                result.SetError(checkFormatResult);
                return result;
            }

            //判斷 是否使用 AuthV 呼叫 M0001 產生 MemberAppToken (取得 OP DATA)
            var getMemberAppTokenResult = _memberInfoService.GetMemberAppTokenByAuthV(request.AuthV);
            if (!getMemberAppTokenResult.IsSuccess)
            {
                result.SetError(getMemberAppTokenResult);
                return result;
            }

            // 判斷 是否為IP黑名單
            var checkIPBlackList = _libMemberInfoService.CheckIPBlackList(RealIP);
            if (!checkIPBlackList.IsSuccess)
            {
                result.SetError(checkIPBlackList);
                return result;
            }

            //判斷登入帳號是否註冊過
            var checkUserCodeUniqueResult = _libMemberInfoService.CheckUserCodeUnique(request.UserCode);
            if (!checkUserCodeUniqueResult.IsSuccess)
            {
                result.SetError(checkUserCodeUniqueResult);
                return result;
            }

            //判斷手機號碼是否註冊過
            var checkCellPhoneResult = _libMemberInfoService.CheckCellPhone(request.CellPhone);
            if (!checkCellPhoneResult.IsSuccess)
            {
                result.SetError(checkCellPhoneResult);
                return result;
            }

            //手機簡訊記錄(Use TempMID)
            var MID = 0;
            var smsAuthModel = new SMSAuth
            {
                AuthType = _memberInfoService.ApiSMSAuthType_toDBAuthType(1),
                CellPhone = request.CellPhone,
                MID = MID,
                RealIP = RealIP,
                ProxyIP = ProxyIP
            };

            //判斷此手機當日是否已發送5次簡訊驗證
            var checkNewCellPhoneSMSCountResult = _libMemberAuthService.CheckAuthSMSCount(smsAuthModel);
            if (!checkNewCellPhoneSMSCountResult.IsSuccess)
            {
                result.SetError(checkNewCellPhoneSMSCountResult);
                return result;
            }            

            //判斷推薦碼是否正確
            long ReferrerMID = 0;
            if (!string.IsNullOrWhiteSpace(request.RCCode))
            {
                var checkRCCodeResult = _libMemberRegService.CheckReferrerCode(request.RCCode);
                if (!checkRCCodeResult.IsSuccess)
                {
                    result.SetError(checkRCCodeResult);
                    return result;
                }

                ReferrerMID = checkRCCodeResult.RtnData;
            }

            //儲存會員資料至 Temp
            var tempRegisterData = new AddTempRegisterDataModel
            {
                OPMID = OPMID,
                Account = request.UserCode,
                CellPhone = request.CellPhone,
                Source = 0,
                ReferrerMID = ReferrerMID,
                Pwd = request.UserPwd,
                RealIP = RealIP,
                ProxyIP = ProxyIP
            };
            var addTempRegisterResult = _libMemberRegService.AddTempRegisterData(tempRegisterData);
            if (!addTempRegisterResult.IsSuccess)
            {
                result.SetError(addTempRegisterResult);
                return result;
            }

            result.SetSuccess();
            return result;
        }

        public DataResult<GetLoginInfoResult> GetLoginInfo(long MID)
        {
            var result = new DataResult<GetLoginInfoResult>();
            result.SetError();

            //檢查更新會員等級
            var memberLevelResult = _libMemberInfoService.GetMemberLevelID(MID);

            //取得會員資料
            var memberData = _libMemberInfoService.GetMemberData(MID);

            //取得會員驗證狀態
            var verifyStatus = _memberInfoService.GetMemberVerifyStatus(MID);

            //取得 OP APP Token 資料
            var appTokenResult = _memberInfoService.GetMemberAppTokenByMID(memberData.basic.MID);

            //取得會員同意項目
            var agreeItems = _libMemberInfoService.ListMemberAgreeItem(MID, 4);

            //取得密碼設定開關狀態
            var getPasswordStatus = _memberInfoService.GetPasswordStatus(MID);

            //取得會員儲值條碼
            string topUpBarcode = null;
            var getTopUpBarcodeResult = _topUpService.GetTopUpBarCode(new Library.Models.TopUp.GetTopUpBarcodeReq { MID = MID, Amount = 0, PaymentType = 2 });
            if (getTopUpBarcodeResult.IsSuccess) { topUpBarcode = getTopUpBarcodeResult.RtnData; }

            //取得登入回傳資訊
            var rtnData = _memberInfoService.GetLoginInfoResult(memberData, verifyStatus, appTokenResult.RtnData, agreeItems, getPasswordStatus, topUpBarcode);

            result.SetSuccess(rtnData);
            return result;
        }

        public DataResult<SendAuthSMSResult> SendAuthSMS(long AppTokenID, long MID, SendAuthSMSRequest request, long RealIP = 0, long ProxyIP = 0)
        {
            var result = new DataResult<SendAuthSMSResult>();
            result.SetError();           

            //檢查資料格式
            if (!request.IsValid())
            {
                result.RtnMsg = request.GetFirstErrorMessage();
                return result;
            }

            //取得 appToken
            var getAppTokenResult = _memberInfoService.GetMemberAppToken(AppTokenID);
            if (!getAppTokenResult.IsSuccess)
            {
                result.SetError(getAppTokenResult);
                return result;
            }
            var appToken = getAppTokenResult.RtnData;


            //檢查對應 驗證類別 是否帶入 LoginTokenID(用 OPMID 檢查), 是否登入(用 MID 檢查)
            var checkSMSAuthTypeResult = _memberInfoService.CheckSMSAuthType(request.SMSAuthType, appToken, MID);
            if (!checkSMSAuthTypeResult.IsSuccess)
            {
                result.SetError(checkSMSAuthTypeResult);
                return result;
            }

            //取得 MID
            MID = appToken.MID;

            //未註冊(MID < 0)用 OPCellPhone, 已註冊(MID > 0)用 Detail.CellPhone
            //檢核手機號碼 requestCellPhone
            var getCellPhone = _libMemberInfoService.GetCellPhone(MID, appToken.OPCellPhone, requestCellPhone: request.CellPhone);
            if (!getCellPhone.IsSuccess)
            {
                result.SetError(getCellPhone);
                return result;
            }

            //取得 CellPhone
            string CellPhone = getCellPhone.RtnData;

            //手機簡訊記錄
            var smsAuthModel = new SMSAuth
            {
                AuthType = _memberInfoService.ApiSMSAuthType_toDBAuthType(request.SMSAuthType),
                CellPhone = CellPhone,
                MID = MID,
                RealIP = RealIP,
                ProxyIP = ProxyIP
            };

            //判斷 是否為 OTP黑名單
            var checkOTPBlackListResult = _libMemberInfoService.CheckOTPBlackList(CellPhone);
            if (!checkOTPBlackListResult.IsSuccess)
            {
                result.SetError(checkOTPBlackListResult);
                return result;
            }

            //判斷此手機當日是否已發送5次簡訊驗證
            var checkNewCellPhoneSMSCountResult = _libMemberAuthService.CheckAuthSMSCount(smsAuthModel);
            if (!checkNewCellPhoneSMSCountResult.IsSuccess)
            {
                result.SetError(checkNewCellPhoneSMSCountResult);
                return result;
            }

            // 檢查登入帳號
            if (request.SMSAuthType == 2)
            {
                var getMIDResult = _libMemberInfoService.GetMID(Account: request.UserCode);
                if (!getMIDResult.IsSuccess)
                {
                    result.SetError(getMIDResult);
                    return result;
                }
            }

            //新增手機簡訊記錄
            var addAuthSMSResult = _libMemberAuthService.AddAuthSMS(smsAuthModel);
            if (!addAuthSMSResult.IsSuccess)
            {
                result.SetError(addAuthSMSResult);
                return result;
            }

            //取得簡訊驗證碼
            string AuthCode = string.Empty;
            if (!_configCyptRepository.ProductMode)//測試
            {
                AuthCode = addAuthSMSResult.AuthCode;
            }
            else
            {
                //todo: 發送簡訊驗證碼至輸入的手機號碼中
            }

            var rtnData = new SendAuthSMSResult
            {
                AuthCode = AuthCode,
                ExpireRange = addAuthSMSResult.ExpireRange
            };

            result.SetSuccess(rtnData);
            return result;
        }

        public DataResult<SendAuthSMSResult> ChangePhoneSendAuthSMS(long MID, ChangePhoneSendAuthSMSRequest request, long RealIP = 0, long ProxyIP = 0)
        {
            var result = new DataResult<SendAuthSMSResult>();
            result.SetError();

            //檢查資料格式
            if (!request.IsValid())
            {
                result.RtnMsg = request.GetFirstErrorMessage();
                return result;
            }

            //判斷手機號碼是否註冊過
            var checkCellPhoneResult = _libMemberInfoService.CheckCellPhone(request.CellPhone, MID);
            if (!checkCellPhoneResult.IsSuccess)
            {
                result.SetError(checkCellPhoneResult);
                return result;
            }

            //取得 CellPhone
            string CellPhone = request.CellPhone;

            //手機簡訊記錄
            var smsAuthModel = new SMSAuth
            {
                AuthType = _memberInfoService.ApiSMSAuthType_toDBAuthType(8),
                CellPhone = CellPhone,
                MID = MID,
                RealIP = RealIP,
                ProxyIP = ProxyIP
            };

            //判斷 是否為 OTP黑名單
            var checkOTPBlackListResult = _libMemberInfoService.CheckOTPBlackList(CellPhone);
            if (!checkOTPBlackListResult.IsSuccess)
            {
                result.SetError(checkOTPBlackListResult);
                return result;
            }


            //判斷此手機當日是否已發送5次簡訊驗證
            var checkNewCellPhoneSMSCountResult = _libMemberAuthService.CheckAuthSMSCount(smsAuthModel);
            if (!checkNewCellPhoneSMSCountResult.IsSuccess)
            {
                //todo: 當日已發送5次簡訊驗證
                result.SetError(checkNewCellPhoneSMSCountResult);
                return result;
            }

            //新增手機簡訊記錄
            var addAuthSMSResult = _libMemberAuthService.AddAuthSMS(smsAuthModel);
            if (!addAuthSMSResult.IsSuccess)
            {
                //todo: 新增簡訊失敗
                result.SetError(addAuthSMSResult);
                return result;
            }

            //取得簡訊驗證碼
            string AuthCode = string.Empty;
            if (true)//測試
            {
                AuthCode = addAuthSMSResult.AuthCode;
            }
            else
            {
                //todo: 發送簡訊驗證碼至輸入的手機號碼中
            }

            var rtnData = new SendAuthSMSResult
            {
                AuthCode = AuthCode,
                ExpireRange = addAuthSMSResult.ExpireRange
            };

            result.SetSuccess(rtnData);
            return result;
        }

        public DataResult<CheckAuthSMSResult> CheckAuthSMS(long AppTokenID, CheckAuthSMSRequest request)
        {
            var rtnData = new CheckAuthSMSResult();
            var result = new DataResult<CheckAuthSMSResult>();
            result.SetError();
            result.RtnData = rtnData;

            //檢查資料格式
            if (!request.IsValid())
            {
                result.RtnMsg = request.GetFirstErrorMessage();
                return result;
            }

            //取得 appToken
            var getAppTokenResult = _memberInfoService.GetMemberAppToken(AppTokenID);
            if (!getAppTokenResult.IsSuccess)
            {
                result.SetError(getAppTokenResult);
                return result;
            }
            var appToken = getAppTokenResult.RtnData;

            //取得 MID
            long MID = appToken.MID;

            //檢查 MID
            var checkMIDTypeResult = _memberInfoService.CheckMID(MID, CheckMIDType.Register);
            if (!checkMIDTypeResult.IsSuccess)
            {
                result.SetError(checkMIDTypeResult);
                return result;
            };

            //手機簡訊驗證資料
            var model = new SMSAuthVerify
            {
                AuthType = _memberInfoService.ApiSMSAuthType_toDBAuthType(request.SMSAuthType),
                CellPhone = request.CellPhone,
                AuthCode = request.AuthCode,
                MID = MID
            };

            //驗證簡訊
            SMSAuthVerifyResult updateResult;
            if (request.SMSAuthType == 8)
            {
                updateResult = _libMemberAuthService.UpdateAuthCellPhoneStatus(model);
            }
            else
            {
                updateResult = _libMemberAuthService.UpdateAuthSMS(model);
            }

            rtnData.AuthErrorCount = updateResult.AuthErrorCount;
            if (!updateResult.IsSuccess)
            {
                result.SetError(updateResult);
                return result;
            }

            //登入帳號 (SMSAuthType = 3，且驗證簡訊碼正確時才會回傳)
            if (request.SMSAuthType == 3)
            {
                var member = _libMemberInfoService.GetMemberData(MID);
                rtnData.UserCode = member.basic.Account;
            }

            result.SetSuccess(rtnData);
            return result;
        }

        public DataResult<CheckRegisterAuthSMSResult> CheckRegisterAuthSMS(long AppTokenID, long ClientCertId, CheckRegisterAuthSMSRequest request)
        {
            var rtnData = new CheckRegisterAuthSMSResult();
            var result = new DataResult<CheckRegisterAuthSMSResult>();
            result.SetError();
            result.RtnData = rtnData;

            //檢查資料格式
            if (!request.IsValid())
            {
                result.RtnMsg = request.GetFirstErrorMessage();
                return result;
            }

            //取得 appToken
            var appTokenResult = _memberInfoService.GetMemberAppToken(AppTokenID);
            if (!appTokenResult.IsSuccess)
            {
                result.SetError(appTokenResult);
                return result;
            }

            //取得 TempMID
            var appToken = appTokenResult.RtnData;
            var TempMID = appToken.MID;

            //檢查 TempMID
            var checkMIDTypeResult = _memberInfoService.CheckMID(TempMID, CheckMIDType.TempRegister);
            if (!checkMIDTypeResult.IsSuccess)
            {
                result.SetError(checkMIDTypeResult);
                return result;
            };

            //檢查 Temp 資料
            var checkMemberTempResult = _libMemberRegService.CheckMemberTemp(TempMID);
            if (!checkMemberTempResult.IsSuccess)
            {
                result.SetError(checkMemberTempResult);
                return result;
            }

            //手機簡訊驗證資料
            var model = new SMSAuthVerify
            {
                CellPhone = request.CellPhone,
                AuthCode = request.AuthCode,
                MID = TempMID
            };

            //判斷 是否為 OTP黑名單
            var checkOTPBlackListResult = _libMemberInfoService.CheckOTPBlackList(request.CellPhone);
            if (!checkOTPBlackListResult.IsSuccess)
            {
                result.SetError(checkOTPBlackListResult);
                return result;
            }

            //驗證簡訊
            var updateResult = _libMemberAuthService.UpdateAuthCellPhoneStatus(model);
            rtnData.AuthErrorCount = updateResult.AuthErrorCount;
            if (!updateResult.IsSuccess)
            {
                result.SetError(updateResult);
                return result;
            }

            //暫存會員資料轉正式
            var tempToMemberResult = _libMemberRegService.TempRegisterDataToMember(TempMID, MemberType: 128);
            if (!tempToMemberResult.IsSuccess)
            {
                result.SetError(tempToMemberResult);
                return result;
            }

            //取得正式 MID
            var MID = tempToMemberResult.RtnData;

            //MID 回壓到 AES/RSA 金鑰上
            var updateClientCertResult = _certificateService.UpdateClientCertFromMember(ClientCertId, MID);
            if (!updateClientCertResult.IsSuccess)
            {
                result.SetError(updateClientCertResult);
                return result;
            }

            rtnData.MID = MID;
            rtnData.UserCode = string.Empty;
            result.SetSuccess(rtnData);
            return result;
        }

        public DataResult<GetLoginInfoResult> UserCodeLogin(long AppTokenID, long ClientCertId, UserCodeLoginRequest request, long RealIP = 0, long ProxyIP = 0)
        {
            var result = new DataResult<GetLoginInfoResult>();

            //檢查資料格式
            if (!request.IsValid())
            {
                result.RtnMsg = request.GetFirstErrorMessage();
                return result;
            }

            //取得 appToken
            var appTokenResult = _memberInfoService.GetMemberAppToken(AppTokenID);
            if (!appTokenResult.IsSuccess)
            {
                result.SetError(appTokenResult);
                return result;
            }
            var appToken = appTokenResult.RtnData;

            //取得 綁定 MID
            long checkMID = appToken.MID;

            //模擬登入
            var loginResult = _libMemberLoginService.UserCodeLogin(request.UserCode, request.UserPwd, request.LoginType, RealIP, ProxyIP, checkMID: checkMID, appRequest: request, MockLogin: 1);
            if (!loginResult.IsSuccess)
            {
                result.SetError(loginResult);
                return result;
            }

            //取得登入MID
            long MID = loginResult.RtnData;

            // 取得會員資料
            var memberData = _libMemberInfoService.GetMemberData(MID);

            if (string.IsNullOrEmpty(request.SMSAuthCode))
            {
                //檢查換機驗證
                var checkDeviceResult = _libMemberLoginService.CheckChangeDevice(MID, request.DeviceID);
                if (!checkDeviceResult.IsSuccess)
                {
                    result.SetError(checkDeviceResult);

                        //回傳換機驗證手機號碼
                        var detail = memberData.detail;
                        result.RtnData = new GetLoginInfoResult { CellPhone = detail.CellPhone };

                    return result;
                }
            }

            //實際登入
            loginResult = _libMemberLoginService.UserCodeLogin(request.UserCode, request.UserPwd, request.LoginType, RealIP, ProxyIP, checkMID: checkMID, appRequest: request, SMSAuthCode: request.SMSAuthCode);
            if (!loginResult.IsSuccess)
            {
                result.SetError(loginResult);
                return result;
            }


            // 檢查 OP 綁定
            var checkOPBindResult = _libMemberAuthService.CheckOPBind(memberData, appToken, Source: 0, RealIP: RealIP, ProxyIP: ProxyIP);
            if (!checkOPBindResult.IsSuccess)
            {
                result.SetError(checkOPBindResult);
                return result;
            }

            //MID 回壓到 AES/RSA 金鑰上, 其他同 MID 的金鑰壓失效
            var updateClientCertResult = _certificateService.UpdateClientCertFromMember(ClientCertId, MID);
            if (!updateClientCertResult.IsSuccess)
            {
                result.SetError(updateClientCertResult);
                return result;
            }

            //回傳登入資訊
            return GetLoginInfo(MID);
        }

        public DataResult<BaseResult> Logout(long MID, long ClientCertId)
        {
            var result = new DataResult<BaseResult>();
            result.SetError();

            var updateResult = _certificateMemberApiService.UpdateClientCertExpired(ClientCertId, MID);
            if (!updateResult.IsSuccess)
            {
                result.SetError(updateResult);
                return result;
            }

            result.SetSuccess(updateResult);
            return result;
        }

        #region M0011 變更登入密碼

        public DataResult<BaseResult> VaildChangeLoginPwd(ChangeLoginPwdRequest request)
        {
            var result = new DataResult<BaseResult>();
            result.SetError();

            if (request.ChangeType == 1 && string.IsNullOrEmpty(request.OriginalLoginPwd))
            {
                result.SetError(new BaseResult { RtnMsg = "原登入密碼為必填欄位" });
                return result;
            }

            result.SetSuccess();
            return result;

        }

        /// <summary>
        /// 變更安全密碼
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public DataResult<ChangeLoginPwdResult> ChangeLoginPwd(long mid, ChangeLoginPwdRequest request, long realIP, long proxyIP)
        {
            var result = new DataResult<ChangeLoginPwdResult>();
            result.SetError();

            string ErrorMsg = string.Empty;
            string Email = string.Empty;

            string args = "登入密碼";

            // 將密碼進行加密動作 
            string encryNewLoginPWD = _configCyptRepository.Hash_UserPwd(request.NewLoginPwd);
            string encryOriLoginPWD = _configCyptRepository.Hash_UserPwd(request.OriginalLoginPwd);
            
            // 取得目前登入密碼
            var oriLoginPWD = _memberInfoService.GetOriLoginPWD(mid);

            // 修改密碼流程
            if (mid > 0)
            {
                //### 檢查原密碼是否正確
                if(request.ChangeType == 1)
                {
                var checkLoginPWD = _memberInfoService.CheckLoginPWD(oriLoginPWD, encryOriLoginPWD);
                if (!checkLoginPWD)
                {
                    result.SetCode(200001); // 請輸入正確的原登入密碼
                    return result;
                }
                }

                //### 密碼規則
                string userCode = _libMemberInfoService.GetMemberData(mid).basic.Account;
                string encryUserCode = _configCyptRepository.Encrypt_UserCode(userCode);
                var verifyPasswordResult = _commonService.VerifyPassword(userCode, request.NewLoginPwd, mid);
                if (verifyPasswordResult != null && verifyPasswordResult.RtnCode != 1)
                {
                    result.SetCode(verifyPasswordResult.RtnCode); // 密碼不可與帳號相同 請輸入6-20個半形英數混合之密碼
                    return result;
                }

                // 檢查是否跟原登入密碼相同
                var checkOldLoginPasswordSameResult = _memberInfoService.CheckOldLoginPasswordSame(encryNewLoginPWD, mid);
                if (checkOldLoginPasswordSameResult != null && checkOldLoginPasswordSameResult.RtnCode != 1)
                {                    
                    result.SetCode(200003, args); // 您輸入的新登入密碼與原登入密碼相同
                    return result;
                }

                // 確認輸入密碼是否一致
                var validConfirmLoginPwdResult = _memberInfoService.ValidConfirmLoginPwd(request.NewLoginPwd, request.ConfirmLoginPwd);
                if (!validConfirmLoginPwdResult)
                {
                    result.SetCode(200004, args); // 兩次登入密碼輸入不同，請重新輸入
                    return result;
                }              

            }
            else
            {                
                var verifyModel = _commonService.VerifyPasswordFormat(request.NewLoginPwd);
                if (verifyModel != null && verifyModel.RtnCode != 1)
                {
                    result.SetCode(200002);
                    return result;
                }
            }


            //### 更新密碼  
            LoginPassword Editmodel = new LoginPassword
            {
                OriLoginPassword = oriLoginPWD,
                NewLoginPassword = encryNewLoginPWD
            };

            var updateResult = _memberInfoService.UpdateLoginPassword(Editmodel, mid, realIP, proxyIP, ref Email);
            if (!updateResult.IsSuccess)
            {
                result.SetError(updateResult);
                return result;
            }

            //寄Email


            result.SetSuccess(new ChangeLoginPwdResult());
            return result;
        }
        #endregion

        #region M0012 變更安全密碼
        /// <summary>
        /// 變更安全密碼
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public DataResult<ChangeSecurityPwdResult> ChangeSecurityPwd(long mid, ChangeSecurityPwdRequest request, long realIP, long proxyIP)
        {
            var result = new DataResult<ChangeSecurityPwdResult>();
            result.SetError();

            string ErrorMsg = string.Empty;
            string Email = string.Empty;

            string args = "安全密碼";

            //安全密碼加密
            HashCryptoHelper hashCryptoHelper = new HashCryptoHelper();
            string encrySecPWD = hashCryptoHelper.HashSha256(request.NewSecPwd);

            //密碼規則
            var authSecPasswordProcessResult = _memberInfoService.AuthSecPasswordProcess(request.NewSecPwd, ref ErrorMsg);
            if (!authSecPasswordProcessResult.IsSuccess)
            {
                result.SetCode(authSecPasswordProcessResult.RtnCode); // 請輸入6位數字安全密碼 或 安全密碼不可使用相同或連續數字，請重新輸入6碼數字之密碼
                return result;
            }

            //### 檢查是否跟原安全密碼相同
            var checkOldPayPasswordSameResult = _memberInfoService.CheckOldSecPasswordSame(encrySecPWD, mid);
            if (!checkOldPayPasswordSameResult)
            {
                result.SetCode(200003, args); //您輸入的新安全密碼與原安全密碼相同
                return result;
            }

            //### 確認輸入安全密碼是否一致
            var validConfirmSecPwdResult = _memberInfoService.ValidConfirmSecPwd(request.NewSecPwd, request.ConfirmSecPwd);
            if (!validConfirmSecPwdResult)
            {
                result.SetCode(200004, args); //兩次新安全密碼輸入不同，請重新輸入
                return result;
            }            

            // 更新密碼
            var oriSecPWD = _memberInfoService.GetOriSecPWD(mid);                       
            SecPassword Editmodel = new SecPassword
            {
                OriSecPassword = oriSecPWD,
                NewSecPassword = encrySecPWD
            };         
            if (!_memberInfoService.UpdateSecPassword(Editmodel, mid, realIP, proxyIP, ref Email))
            {
                result.SetCode(0); //設定安全密碼失敗
                return result;
            }

            //寄Email


            result.SetSuccess(new ChangeSecurityPwdResult());
            return result;


        }
        #endregion

        #region M0013 檢查安全密碼
        public DataResult<CheckPayPwdResult> CheckSecurityPwd(long mid, CheckPayPwdRequest request, long realIP, long proxyIP)
        {
            var result = new DataResult<CheckPayPwdResult>();
            result.SetError();

            if (!request.IsValid())
            {
                string msg = request.GetFirstErrorMessage();
                result.SetFormatError(msg);
                return result;
            }
            
            string ErrorMessage = string.Empty;

            //安全密碼加密
            HashCryptoHelper hashCryptoHelper = new HashCryptoHelper();
            string encryPayPWD = hashCryptoHelper.HashSha256(request.SecPwd);


            //### 檢查Model
            var RtnModel = _memberInfoService.Check_Member_Security_CheckSecPwd(mid, encryPayPWD, realIP, proxyIP);
            if (RtnModel != null && RtnModel.RtnCode == 1)
            {
                result.SetSuccess(new CheckPayPwdResult());
                return result;
            }
            else
            {
                result.SetError(RtnModel);
                return result;
            }
        }
        #endregion

        #region M0014 變更圖形密碼       
        /// <summary>
        /// 檢查圖形密碼
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public DataResult<ChangeGraphicLockResult> ChangeGraphicLock(long mid, ChangeGraphicLockRequest request, long realIP, long proxyIP)
        {
            var result = new DataResult<ChangeGraphicLockResult>();
            result.SetError();

            if (!request.IsValid())
            {
                string msg = request.GetFirstErrorMessage();
                result.SetFormatError(msg);
                return result;
            }

            // 取得原圖型密碼
            var memberGraphicLock = _memberGraphicLockService.GetMemberGraphicLock(mid);
            string oriGraphicPwd = (memberGraphicLock != null) ? memberGraphicLock.Password : null;

            //圖形鎖加密
            AesCryptoHelper Aes = new AesCryptoHelper
            {
                Key = GlobalConfigUtil.FingerPrintPasswordHashKey,
                Iv = GlobalConfigUtil.FingerPrintPasswordHashIV
            };
            string encryNewGraphicPwd = Aes.Encrypt(Aes.Key + request.NewGraphicPwd + Aes.Iv);

            // 檢查兩次輸入密碼是否一致
            var validGraphicPwdConfirm = _memberGraphicLockService.GraphicPwdConfirm(request);
            if (!validGraphicPwdConfirm.IsSuccess)
            {
                result.SetError(validGraphicPwdConfirm); 
                return result;
            }
            
            // 新圖型密碼是否與舊圖型密碼是否相同
            var validGraphicPwdRepeatResult = _memberGraphicLockService.ValidGraphicPwdRepeat(mid, encryNewGraphicPwd, oriGraphicPwd);
            if (!validGraphicPwdRepeatResult.IsSuccess)
            {
                result.SetError(validGraphicPwdRepeatResult); 
                return result;
            }

            // 更新圖型密碼            
            GraphicLockRerquest graphicLockRerquest = new GraphicLockRerquest {
                OriPassword = oriGraphicPwd,
                NewPassword = encryNewGraphicPwd
            };
            var updateGraphicPasswordResult = _memberGraphicLockService.UpdateGraphicPassword(mid, graphicLockRerquest, realIP, proxyIP, request.DeviceID);
            if (updateGraphicPasswordResult.RtnCode != 1)
            {
                result.SetError(updateGraphicPasswordResult);
                return result;
            }

            result.SetSuccess(new ChangeGraphicLockResult());
            return result;

        }
        #endregion

        #region M0015 檢查圖形密碼    
        /// <summary>
        /// 檢查圖形密碼
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public DataResult<CheckGraphicLockResult> CheckGraphicLock(long mid, CheckGraphicLockRequest request, long realIP, long proxyIP)
        {
            var result = new DataResult<CheckGraphicLockResult>();
            result.SetError();

            if (!request.IsValid())
            {
                string msg = request.GetFirstErrorMessage();
                result.SetFormatError(msg);
                return result;
            }

            //圖形鎖加密
            AesCryptoHelper Aes = new AesCryptoHelper
            {
                Key = GlobalConfigUtil.FingerPrintPasswordHashKey,
                Iv = GlobalConfigUtil.FingerPrintPasswordHashIV
            };

            string encryGraphicPwd = Aes.Encrypt(Aes.Key + request.GraphicPwd+ Aes.Iv);

            var checkGraphicLockResult = _memberGraphicLockService.CheckGraphicLock(mid, encryGraphicPwd, realIP, proxyIP);
            if (checkGraphicLockResult != null && checkGraphicLockResult.RtnCode == 1 )
            {
                result.SetSuccess(new CheckGraphicLockResult());
                return result;
            }
            else
            {
                result.SetError(checkGraphicLockResult);
                return result;
            }

        }
        #endregion

        #region M0016 設定指紋辨識及FaceID開關
        public DataResult<FingerPrintPwdStatusResult> UpdateFingerPrintPwdStatus(long mid, FingerPrintPwdStatusRequest request, long realIP, long proxyIP)
        {
            var result = new DataResult<FingerPrintPwdStatusResult>();
            result.SetError();

            string fingerPrintPassword = _memberInfoService.UpdateFingerPrintPassword(mid, request.DeviceID, request.Status);
            bool status = _memberInfoService.GetFingerPrintPasswordStatus(mid);

            var data = new FingerPrintPwdStatusResult {
                FingerPrintPwd = fingerPrintPassword
            };

            // Status=True 回傳 FingerPrintPwd
            //if (!string.IsNullOrWhiteSpace(fingerPrintPassword) && status)
            //{                
            //    data.FingerPrintPwd  = fingerPrintPassword;                
            //}

            //指紋開關和圖形密碼 開關 互斥 不能同時開啟
            if (request.Status)
            {
                _memberGraphicLockService.UpdateGraphicLockStatus(mid, request.DeviceID, false, realIP, proxyIP);
            }


            result.SetSuccess(data);
            return result;
        }
        #endregion

        #region M0017 驗證指紋辨識及FaceID
        public DataResult<CheckFingerPrintPwdResult> CheckFingerPrintPwd(long mid, CheckFingerPrintPwdRequest request, long realIP, long proxyIP)
        {
            var result = new DataResult<CheckFingerPrintPwdResult>();
            result.SetError();

            if (!request.IsValid())
            {
                string msg = request.GetFirstErrorMessage();
                result.SetFormatError(msg);
                return result;
            }

            if (!string.IsNullOrWhiteSpace(request.FingerPrintPwd))
            {
                var rtnModel = _memberInfoService.CheckFingerPrintPassword(mid, request.FingerPrintPwd, request.DeviceID, realIP, proxyIP);
                if (rtnModel.RtnCode == 1)
                {
                    result.SetSuccess(new CheckFingerPrintPwdResult());
                    return result;
                }
                else
                {
                    result.SetError(rtnModel);
                    return result;
                }                
            }

            result.SetSuccess(new CheckFingerPrintPwdResult());
            return result;
        }


        #endregion

        #region M0018 設定圖形密碼開關
        public DataResult<GraphicLockStatusResult> UpdateGraphicLockStatus(long mid, GraphicLockStatusRequest request, long RealIP, long ProxyIP)
        {
            var result = new DataResult<GraphicLockStatusResult>();
            result.SetError();

            // 變更圖形密碼開關
            var updateGraphicLockStatusResult = _memberGraphicLockService.UpdateGraphicLockStatus(mid, request.DeviceID, request.Status, RealIP, ProxyIP);
            if (!updateGraphicLockStatusResult.IsSuccess)
            {
                result.SetError(updateGraphicLockStatusResult);
                return result;
            }

            // 取得圖型鎖密碼
            string graphicPassword = (_memberGraphicLockService.GetMemberGraphicLock(mid) != null) ? _memberGraphicLockService.GetMemberGraphicLock(mid).Password : "";
            
            // 取得圖型鎖狀態
            bool status = _memberGraphicLockService.GetGraphicLockStatus(mid);

            //指紋開關和圖形密碼 開關 互斥 不能同時開啟
            if (request.Status)
            {
                _memberInfoService.UpdateFingerPrintPassword(mid, request.DeviceID, false); ;
            }

            var data = new GraphicLockStatusResult
            {
                GraphicPwd = graphicPassword
            };

            if (status)
            {
                result.SetSuccess(data);
            }
            else
            {
                result.SetSuccess();
            }

            if (status)
            {
                result.SetSuccess(data);
            }
            else
            {
                result.SetSuccess();
            }
            
            return result;
        }
        #endregion

        #region M0019 取得密碼設定開關狀態
        /// <summary>
        /// 取得密碼設定開關狀態
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public DataResult<GetPasswordStatusResult> GetPasswordStatus(long mid, GetPasswordStatusRequest request)
        {
            var result = new DataResult<GetPasswordStatusResult>();
            result.SetError();

            var rtnModel = _memberInfoService.GetPasswordStatus(mid);
            
            var data = new GetPasswordStatusResult {
                GraphicLockStatus = rtnModel.GraphicLockStatus,
                FingerPrintPwdStatus = rtnModel.FingerPrintPwdStatus,
                IsExistsSetGraphicLock = rtnModel.IsExistsSetGraphicLock
            };

            result.SetSuccess(data);
            return result;
        }
        #endregion

        #region M0024 重設登入密碼
        public DataResult<ResetLoginPwdResult> ResetLoginPwd(long AppTokenID, ResetLoginPwdRequest request, long realIP, long proxyIP)
        {
            var result = new DataResult<ResetLoginPwdResult>();
            result.SetError();

            string ErrorMsg = string.Empty;
            string Email = string.Empty;

            //取得 appToken
            var appTokenResult = _memberInfoService.GetMemberAppToken(AppTokenID);
            if (!appTokenResult.IsSuccess)
            {
                result.SetError(appTokenResult);
                return result;
            }
            var appToken = appTokenResult.RtnData;

            //取得 綁定 MID
            long checkMID = appToken.MID;

            //檢查 綁定 MID
            var checkMIDTypeResult = _memberInfoService.CheckMID(checkMID, CheckMIDType.Register);
            if (!checkMIDTypeResult.IsSuccess)
            {
                result.SetError(new BaseResult { RtnMsg = "尚未註冊完成" });
                return result;
            };
            
            
            // 將密碼進行加密動作 
            string encryNewLoginPWD = _configCyptRepository.Hash_UserPwd(request.NewLoginPwd);

            // 取得目前登入密碼
            var oriLoginPWD = _memberInfoService.GetOriLoginPWD(checkMID);

            // 修改密碼流程
            //if (checkMID > 0)
            //{   
                //### 密碼規則
                string userCode = _libMemberInfoService.GetMemberData(checkMID).basic.Account;
                string encryUserCode = _configCyptRepository.Encrypt_UserCode(userCode);
                var verifyPasswordResult = _commonService.VerifyPassword(userCode, request.NewLoginPwd, checkMID);
                if (verifyPasswordResult != null && verifyPasswordResult.RtnCode != 1)
                {
                    result.SetCode(verifyPasswordResult.RtnCode); // 密碼不可與帳號相同、請輸入6-20個半形英數混合之密碼
                    return result;
                }

                // 檢查是否跟原登入密碼相同
                var checkOldLoginPasswordSameResult = _memberInfoService.CheckOldLoginPasswordSame(encryNewLoginPWD, checkMID);
                if (checkOldLoginPasswordSameResult != null && checkOldLoginPasswordSameResult.RtnCode != 1)
                {
                    result.SetError(checkOldLoginPasswordSameResult); // 您輸入的新登入密碼與原登入密碼相同
                    return result;
                }

                // 確認輸入密碼是否一致
                var validConfirmLoginPwdResult = _memberInfoService.ValidConfirmLoginPwd(request.NewLoginPwd, request.ConfirmLoginPwd);
                if (!validConfirmLoginPwdResult)
                {
                    result.RtnMsg = "兩次登入密碼輸入不同，請重新輸入";
                    return result;
                }

            
            else
            {
                var verifyModel = _commonService.VerifyPasswordFormat(request.NewLoginPwd);
                if (!verifyModel.IsSuccess)
                {
                    result.SetError(verifyModel);
                    return result;
                }
            }


            //### 更新密碼  
            LoginPassword Editmodel = new LoginPassword
            {
                OriLoginPassword = oriLoginPWD,
                NewLoginPassword = encryNewLoginPWD
            };

            var updateResult = _memberInfoService.UpdateLoginPassword(Editmodel, checkMID, realIP, proxyIP, ref Email);
            if (!updateResult.IsSuccess)
            {
                result.SetError(updateResult);
                return result;
            }

            //to do: 寄Email


            result.SetSuccess(new ResetLoginPwdResult());
            return result;
        }
        #endregion

        #region M0023 修改Email
        public DataResult<UpdateEmailAddressResult> UpdateEmailAddress(long mid, UpdateEmailAddressRequest request)
        {
            var result = new DataResult<UpdateEmailAddressResult>();
            result.SetError();

            if (!request.IsValid())
            {
                string msg = request.GetFirstErrorMessage();
                result.SetFormatError(msg);
                return result;
            }

            //修改Email
            var UpdateEmailAddressResult = _memberInfoService.UpdateEmailAddress(mid, request.Email);
            if (UpdateEmailAddressResult.RtnCode != 1)
            {
                result.SetError(UpdateEmailAddressResult);
                return result;
            }
            
            result.SetSuccess(new UpdateEmailAddressResult());
            return result;
        }
        #endregion

        #region M0022 修改暱稱
        public DataResult<UpdateNickNameResult> UpdateNickName(long mid, UpdateNickNameRequest request)
        {
            var result = new DataResult<UpdateNickNameResult>();
            result.SetError();

            if (!request.IsValid())
            {
                string msg = request.GetFirstErrorMessage();
                result.SetFormatError(msg);
                return result;
            }

            //檢查暱稱格式
            var vaildNickNameResult = _memberInfoService.VaildNickName(request.NickName);
            if (!vaildNickNameResult.IsSuccess)
            {
                result.SetError(vaildNickNameResult);
                return result;
            }

            //修改暱稱
            var UpdateEmailAddressResult = _memberInfoService.UpdateNickName(mid, request.NickName);
            if (UpdateEmailAddressResult.RtnCode != 1)
            {
                result.SetError(UpdateEmailAddressResult);
                return result;
            }

            result.SetSuccess(new UpdateNickNameResult());
            return result;
        }
        #endregion



        #region _記錄略過修改安全密碼
        public DataResult<BaseResult> SetPayPwdIgnoreDate(long mid)
        {
            var result = new DataResult<BaseResult>();
            result.SetError();

            var IgnoreModifyPayPwdDate = _memberInfoService.UpdateIgnoreModifyPayPwdDate(mid);
            if (IgnoreModifyPayPwdDate.RtnCode != 1)
            {
                result.SetCode(0);
                return result;
            }

            result.SetSuccess(IgnoreModifyPayPwdDate);
            return result;
        }
        #endregion

        #region M0040 記錄略過修改圖形密碼
        public DataResult<BaseResult> SetGraphicLockIgnorDate(long mid, long RealIP, long ProxyIP)
        {
            var result = new DataResult<BaseResult>();
            result.SetError();

            var IgnoreModifyPayPwdDate = _memberGraphicLockService.UpdateGraphicPwdIgnorDate(mid, RealIP, ProxyIP);
            if (IgnoreModifyPayPwdDate.RtnCode != 1)
            {
                result.SetCode(0);
                return result;
            }

            result.SetSuccess(IgnoreModifyPayPwdDate);
            return result;
        }
        #endregion

        #region M0038 記錄略過修改登入密碼
        public DataResult<BaseResult> SetLoginPwdIgnorDate(long mid)
        {
            var result = new DataResult<BaseResult>();
            result.SetError();

            var IgnoreModifyPwdDate = _memberInfoService.UpdatePwdIgnorDate(mid);
            if (IgnoreModifyPwdDate.RtnCode != 1)
            {
                result.SetCode(0);
                return result;
            }

            result.SetSuccess(IgnoreModifyPwdDate);
            return result;
        }
        #endregion
                

        public DataResult<BaseResult> UpdateMemberPreferences(long MID, UpdateMemberPreferencesRequest request, long RealIP = 0, long ProxyIP = 0)
        {
            var result = new DataResult<BaseResult>();
            BaseResult baseResult =new BaseResult();
            result.SetError();
            byte OptionType = 0;//設定類型(0:APP, 1:PC官網, 2:廠商後台, 3:其它)

            var updateResult = _libMemberInfoService.UpdateMemberPreferences(MID, OptionType, request.Options, RealIP, ProxyIP);
            if (!updateResult.IsSuccess)
            {
                result.SetError(updateResult);
                return result;
            }

            result.SetSuccess(updateResult);
            return result;
        }
        

        public DataResult<GetMemberPreferencesResult> GetMemberPreferences(long MID, GetMemberPreferencesRequest request)
        {
            var result = new DataResult<GetMemberPreferencesResult>();
            result.SetError();

            byte OptionType = 0;//設定類型(0:APP, 1:PC官網, 2:廠商後台, 3:其它)

            var rtnData = new GetMemberPreferencesResult();//回傳資料

            //指定查詢單一設定
            if (!string.IsNullOrWhiteSpace(request.OptionName))
            {
                var options = new List<MemberPreferenceModel>();
                var option = _libMemberInfoService.GetMemberPreference(MID, OptionType, request.OptionName);
                options.Add(option);
                rtnData.Options = options;
            }
            else
            {
                rtnData.Options = _libMemberInfoService.ListMemberPreferences(MID, OptionType);
            }

            result.SetSuccess(rtnData);
            return result;
        }
               
        public DataResult<BaseResult> SetMemberAgree(long MID, SetMemberAgreeRequestItems request, long RealIP = 0, long ProxyIP = 0)
        {
            var result = new DataResult<BaseResult>();
            BaseResult baseResult =new BaseResult();
            baseResult.SetError();
            result.SetError();

            foreach (var memberAgreeRequest in request.AgreeItems)
            {
                baseResult = _libMemberInfoService.UpdateMemberAgreeResult(MID, memberAgreeRequest.AgreeType, memberAgreeRequest.AgreeStatus, RealIP, ProxyIP);
                if (!baseResult.IsSuccess)
                {
                    result.SetError(baseResult);
                    return result;

                }
            }

            result.SetSuccess(baseResult);
            return result;
        }

        public DataResult<ListNotifyMessageResult> ListNotifyMessage(long MID)
        {
            var result = new DataResult<ListNotifyMessageResult>();
            result.SetError();

            var listResult = _libtMemberNotifyMessageService.ListNotifyMessage(MID, 1, 10000);

            var rtnData = new ListNotifyMessageResult();
            rtnData.List = _memberInfoService.NotifyMessageToItem(listResult.Items);

            result.SetSuccess(rtnData);
            return result;
        }

        public DataResult<GetRangeNotifyMessageListResult> GetRangeNotifyMessageList(long MID, GetRangeNotifyMessageListRequest request)
        {
            var result = new DataResult<GetRangeNotifyMessageListResult>();
            result.SetError();

            var list = _libtMemberNotifyMessageService.ListNotifyMessageByID(MID, request.MsgID, request.Type ?? 0, request.Count);

            var rtnData = new GetRangeNotifyMessageListResult();
            rtnData.List = _memberInfoService.NotifyMessageToItem(list);

            result.SetSuccess(rtnData);
            return result;
        }

        public DataResult<ListSynNotifyMessageResult> ListSynNotifyMessage(long MID, ListSynNotifyMessageRequest request)
        {
            var result = new DataResult<ListSynNotifyMessageResult>();
            result.SetError();

            var list = _libtMemberNotifyMessageService.ListSynNotifyMessageByID(MID, request.ModifyDate);

            var rtnData = new ListSynNotifyMessageResult();
            rtnData.List = _memberInfoService.NotifyMessageToItem(list);

            result.SetSuccess(rtnData);
            return result;
        }

        public DataResult<BaseResult> SetReadMessage(long MID, SetReadMessageRequest request)
        {
            var result = new DataResult<BaseResult>();
            result.SetError();

            BaseResult updateResult;

            if (!request.ReadAll)
            {
                var ArrayMsgID = (request.Params ?? new List<SetReadMessageRequest.Param>()).Select(t => t.MsgID).ToList();
                updateResult = _libtMemberNotifyMessageService.ReadNotifyMessage(MID, ArrayMsgID);
            }
            else
            {
                updateResult = _libtMemberNotifyMessageService.ReadAllNotifyMessage(MID);
            }

            if (!updateResult.IsSuccess)
            {
                result.SetError(updateResult);
                return result;
            }

            result.SetSuccess(updateResult);
            return result;
        }

        public DataResult<BaseResult> DeleteNotifyMessage(long MID, DeleteNotifyMessageRequest request)
        {
            var result = new DataResult<BaseResult>();
            result.SetError();

            BaseResult updateResult;

            if (!request.DeleteAll)
            {
                var ArrayMsgID = (request.Params ?? new List<DeleteNotifyMessageRequest.Param>()).Select(t => t.MsgID).ToList();
                updateResult = _libtMemberNotifyMessageService.DeleteNotifyMessage(MID, ArrayMsgID);
            }
            else
            {
                updateResult = _libtMemberNotifyMessageService.DeleteAllNotifyMessage(MID);
            }

            if (!updateResult.IsSuccess)
            {
                result.SetError(updateResult);
                return result;
            }

            result.SetSuccess(updateResult);
            return result;
        }

        public DataResult<GetAppNotifyCountsResult> GetAppNotifyCounts(long MID)
        {
            var result = new DataResult<GetAppNotifyCountsResult>();
            result.SetError();

            var model = _libtMemberNotifyMessageService.GetNotifyMessageUnReadCount(MID);
            var rtnData = new GetAppNotifyCountsResult();
            rtnData.Counts = model.NotifyMessageUnread;
            rtnData.TotalUnread = model.TotalCount;

            result.SetSuccess(rtnData);
            return result;
        }

        public DataResult<GetNotifyMessageResult> GetNotifyMessage(long MID, GetNotifyMessageRequest request)
        {
            var result = new DataResult<GetNotifyMessageResult>();
            result.SetError();

            var getResult = _libtMemberNotifyMessageService.GetNotifyMessage(request.MsgID, MID);
            if (!getResult.IsSuccess)
            {
                return result;
            }
            
            var rtnData = _memberInfoService.GetNotifyMessageResult(getResult.RtnData);

            result.SetSuccess(rtnData);
            return result;
        }
        /// <summary>
        /// 取得銀行清單
        /// </summary>
        /// <returns></returns>
        public DataResult<GetListBankInfoResult> GetListBankInfo()
        {
            var result = new DataResult<GetListBankInfoResult>();

            var banks = _memberInfoService.GetListBankInfo();

            result.SetSuccess(banks);
            return result;
        }

        /// <summary>
        /// 取得提領資訊
        /// </summary>
        /// <param name="MID"></param>
        /// <returns></returns>
        public DataResult<GetWithdrawBalanceInfoResult> GetWithdrawBalanceInfo(long MID)
        {
            var result = new DataResult<GetWithdrawBalanceInfoResult>();
            result.SetError();

            var bankTransferInfo = _memberInfoService.GetBankTransferInfo(MID);

            var userCoinsBalance = _libMemberBankService.GetUserCoinsBalance(MID);

            var rtnData = new GetWithdrawBalanceInfoResult();
            if (bankTransferInfo == null)
            {
                rtnData.AvailableWithdrawCash = Convert.ToInt32(userCoinsBalance.TotalBalance);
            }
            else
            {
                rtnData.AccountLast5No = _memberInfoService.ConcealBankAccount(bankTransferInfo.BankAccount);

                rtnData.AccountID = bankTransferInfo.AccountID;
                rtnData.BankCode = bankTransferInfo.BankCode;
                rtnData.BankBranchCode = bankTransferInfo.BankBranchCode;
                rtnData.AccountLast5No = _memberInfoService.ConcealBankAccount(bankTransferInfo.BankAccount);
                rtnData.BankName = bankTransferInfo.BankName;
                rtnData.BankBranchName = bankTransferInfo.BankBranchName;
                rtnData.HandlingCharge = bankTransferInfo.Category == 0 ? 15 : 0;
                rtnData.AvailableWithdrawCash = Convert.ToInt32(userCoinsBalance.TotalBalance);
            }

            result.SetSuccess(rtnData);
            return result;
        }

        /// <summary>
        /// 取得綁定銀行帳號
        /// </summary>
        /// <param name="MID"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public DataResult<GetBindAccountListResult> GetBindAccountList(long MID, GetBindAccountListRequest request)
        {
            var result = new DataResult<GetBindAccountListResult>();

            var list = new List<AccountList>();

            byte? category = null;
            if (request.AccountType == 1)
            {
                category = 1;
            }
            else if (request.AccountType == 2)
            {
                category = 0;
            }
            else
            {
                result.RtnMsg = "綁定帳戶類別 格式錯誤";
                return result;
            }

            var memberBankInfo = _libMemberBankService.ListMemberBankInfo(MID, category);

            memberBankInfo.ForEach(t => list.Add(new AccountList
            {
                BankCode = t.BankCode,
                AccountID = t.AccountID,
                AccountLastNo = _memberInfoService.ConcealBankAccount(t.BankAccount),
                BankName = t.BankName,
                BankShortName = t.BankShortName,
                CreateDate = t.CreateDate
            }));

            result.SetSuccess(new GetBindAccountListResult
            {
                AccountList = list
            });
            return result;
        }

        /// <summary>
        /// 新增提領轉入帳戶
        /// </summary>
        /// <returns></returns>
        public DataResult<BaseResult> AddWithdrawAccountBind(long MID, AddWithdrawAccountBindRequest request)
        {
            var result = new DataResult<BaseResult>();

            var memberBankAccount = new MemberBankAccount
            {
                MID = MID,
                Category = 0,
                AuthCategory = 0,
                BankCode = request.BankCode,
                BankAccount = request.BankAccount,
                BankBranchCode = request.BranchCode
            };

            var addResult = _libMemberBankService.AddMemberBankAccount(memberBankAccount);
            if (addResult.RtnCode != 1)
            {
                result.SetError(addResult);
                return result;
            }

            result.SetSuccess();
            return result;
        }

        public DataResult<BaseResult> AuthIDNO(long MID, AuthIDNORequest request, long RealIP = 0, long ProxyIP = 0)
        {
            var result = new DataResult<BaseResult>();

            string appName = "member";

            #region 檢查資料格式及資料轉型
            if (!request.IsValid())
            {
                result.RtnMsg = request.GetFirstErrorMessage();
                return result;
            }

            #region 檢查身分證/居留證是否重複
            var checkIdnoResult = _libMemberAuthService.CheckIdnoRepeat(request.Idno, MID, IsOversea: false);
            if (checkIdnoResult.Repeat)
            {
                result.SetError(checkIdnoResult);
                return result;
            }
            #endregion

            #region 檢查身分證/居留證是否可使用            
            if (!checkIdnoResult.CanUse)
            {
                result.SetError(checkIdnoResult);
                return result;
            }
            #endregion

            #region 檢查信箱是否重複
            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                var checkEmailResult = _memberInfoService.CheckEmail(request.Email, MID);
                if (!checkEmailResult.IsSuccess)
                {
                    result.SetError(checkEmailResult);
                    return result;
                }
            }
            #endregion

            var model = new AuthIDNOModel();
            var detailModel = new MemberDetailModel();
            var verifyResult = VerifyAuthIDNORequest(MID, RealIP, ProxyIP, request, ref model, ref detailModel);
            if (!verifyResult.IsSuccess)
            {
                result.SetError(verifyResult);
                return result;
            }
            #endregion

            #region 新增AuthIDNO
            var addAuthIdnoResult = _libMemberAuthService.AddAuthIDNO(model, RealIP, ProxyIP);
            if (!addAuthIdnoResult.IsSuccess)
            {
                result.SetError(addAuthIdnoResult);
                return result;
            }
            #endregion
            
            #region 更新會員資料
            var updateMemberResult = _memberInfoService.UpdateMemberDetail(MID, request.CName, detailModel);
            if (!updateMemberResult.IsSuccess)
            {
                result.SetError(updateMemberResult);
                return result;
            }
            #endregion

            if (model.IsTeenager)
            {
                //SendAuthTeenagers(MID, request.CName);
                result.SetSuccess();
                return result;
            }
            else
            {
                #region 查功能開關
                var appFunctionSwitchs = _libAdminService.ListAppFunctionSwitch(appName);
                var query = appFunctionSwitchs.Where(t => (t.FunctionID == 1 || t.FunctionID == 2) && t.Status == 0).ToList();
                if (query.Any())
                {
                    result.RtnCode = 0;
                    result.RtnMsg = "聯徵功能尚未開啟，請聯絡客服";
                    return result;
                }
                #endregion

                #region P33 驗證
                var p33Auth = new P33Auth
                {
                    MID = MID,
                    IDNO = request.Idno,
                    Source = 1,
                    RealIP = RealIP,
                    ProxyIP = ProxyIP
                };
                var p33Result = _libPersonalAuthService.AddAuthP33(p33Auth);

                if (!p33Result.IsSuccess)
                {
                    result.SetError(p33Result);
                    return result;
                }
                #endregion

                #region P11 驗證
                var p11Auth = new P11Auth
                {
                    MID = MID,
                    IDNO = request.Idno,
                    IssueDate = model.IssueDate,
                    ObtainType = model.IssueType,
                    IsPicture = 1,
                    BirthDay = detailModel.Birthday,
                    IssueLocationID = request.IssueLoc,
                    Source = 1,
                    RealIP = RealIP,
                    ProxyIP = ProxyIP
                };
                var p11Result = _libPersonalAuthService.AddAuthP11(p11Auth);

                if (!p11Result.IsSuccess)
                {
                    result.SetError(p11Result);
                    return result;
                }
                #endregion
            }

            result.SetSuccess();
            return result;
        }

        /// <summary>
        /// 驗證AuthIDNORequest 資料
        /// </summary>
        /// <param name="MID"></param>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult VerifyAuthIDNORequest(
            long MID,
            long RealIP,
            long ProxyIP,
            AuthIDNORequest request,
            ref AuthIDNOModel model,
            ref MemberDetailModel detailModel
            )
        {
            var result = new BaseResult();

            DateTime issueDate;
            if (!DateTime.TryParse(request.IssueDate, out issueDate))
            {
                result.RtnMsg = "發證日期 格式錯誤";
                return result;
            }

            DateTime birthday;
            if (!DateTime.TryParse(request.BirthDay, out birthday))
            {
                result.RtnMsg = "生日 格式錯誤";
                return result;
            }

            if (request.Files == null || request.Files.Count() != 2 || request.Files[0] == null || request.Files[1] == null)
            {
                result.RtnMsg = "檔案數量有誤";
                return result;
            }

            var issueType = Convert.ToByte(request.IssueType);

            var nationalityID = Convert.ToInt64(request.NationalityID);

            var cNameVerifyResult = _libMemberAuthService.IsValidateCName(request.CName);
            if (!cNameVerifyResult.IsSuccess)
            {
                result.SetError(cNameVerifyResult);
                return result;
            }

            #region 上傳身分證/居留證正反面
            List<string> realFilePathes = new List<string>();
            var uploadFilesResult = _memberInfoService.UploadAuthIDNOFiles(request.Files, ref realFilePathes);
            if (!uploadFilesResult.IsSuccess)
            {
                result.SetError(uploadFilesResult);
                return result;
            }
            #endregion

            bool isTeenager = false;
            int age = _memberInfoService.CalculateAge(birthday);
            if (age < 14)
            {
                result.RtnMsg = "未滿14歲無法註冊";
                return result;
            }
            else if (age < 20 && age >= 14)
            {
                if (request.LegalRepData == null || !request.LegalRepData.Any())
                {
                    result.RtnMsg = "未成年需法定代理人同意";
                    return result;
                }

                var addResult = AddTeenagersLegalDetail(MID, request.LegalRepData, RealIP, ProxyIP);
                if (!addResult.IsSuccess)
                {
                    return addResult;
                }

                isTeenager = true;
            }

            model.MID = MID;
            model.IDNO = request.Idno;
            model.IssueDate = issueDate;
            model.ObtainType = issueType;
            model.FilePath1 = realFilePathes[0];
            model.FilePath2 = realFilePathes[1];
            model.IsTeenager = isTeenager;
            model.IssueLocationID = _memberInfoService.GetLocationByIssueDate(model.IssueLocationID, issueDate);
            model.IsPicture = 1;

            detailModel.Birthday = birthday;
            detailModel.IDNO = request.Idno;
            detailModel.NationalityID = nationalityID;
            detailModel.AreaID = request.AreaID;
            detailModel.Address = request.Address;
            detailModel.Email = request.Email;

            result.SetSuccess();
            return result;
        }

        /// <summary>
        /// 驗證法定代理人資料及寫DB
        /// </summary>
        /// <param name="MID"></param>
        /// <param name="legalRepDatas"></param>
        /// <returns></returns>
        public BaseResult AddTeenagersLegalDetail(long MID, List<LegalRepData> legalRepDatas, long RealIP, long ProxyIP)
        {
            var result = new BaseResult();

            foreach (var legalRepData in legalRepDatas)
            {
                var cNameVerifyResult = _libMemberAuthService.IsValidateCName(legalRepData.LegalRepName);
                if (!cNameVerifyResult.IsSuccess)
                {
                    result.SetError(cNameVerifyResult);
                    return result;
                }

                if (string.IsNullOrWhiteSpace(legalRepData.LegalRepIcpMID))
                {
                    result.RtnMsg = "法定代理人電支帳號未填";
                    return result;
                }

                result = _memberInfoService.AddAuthTeenagersLegalDetail(MID, legalRepData, RealIP, ProxyIP);
                if (!result.IsSuccess)
                {
                    return result;
                }
            }
            result = _memberInfoService.AddAuthTeenagers(MID, RealIP, ProxyIP);
            if (!result.IsSuccess)
            {
                return result;
            }

            result.SetSuccess();
            return result;
        }

        /// <summary>
        /// 發訊息中心給法代
        /// </summary>
        /// <param name="MID"></param>
        /// <param name="CName"></param>
        /// <param name="ExpireDate"></param>
        public void SendAuthTeenagers(long MID, string CName)
        {
            string msgKey = "auth_teenagers_confirm";

            var memberData = _libMemberInfoService.GetMemberData(MID);

            var genArgs = new
            {
                CName,
                ExpireDate = memberData.basic.ExpireDate.ToString("yyyy-MM-dd HH:mm:ss")
            };

            var content = _notifyManageService.Generate(msgKey, genArgs);

            var details = _memberInfoService.ListTeenagersLegalDetail(MID);
            details.ForEach(t =>
            {
                _libtMemberNotifyMessageService.AddNotifyMessage(t.MID, content.Title, content.Body, push: 1);
            });
        }

        /// <summary>
        /// Socket Server使用者驗證
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public DataResult<PrivateUserAuthResult> PrivateUserAuth(long mid)
        {
            var result = new DataResult<PrivateUserAuthResult>();
            result.SetError();
            //取得會員資料
            var memberData = _libMemberInfoService.GetMemberData(mid);
            //取得登入回傳資訊
            var rtnData = _memberInfoService.PrivateUserAuthResult(memberData);
            rtnData.TimeStamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            result.SetSuccess(rtnData);
            return result;
        }

        /// <summary>
        /// 取得電子發票載具頁面所需資訊
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public DataResult<GetEInvoiceCarrierInfoResult> GetEInvoiceCarrierInfo(long mid)
        {
            var result = new DataResult<GetEInvoiceCarrierInfoResult>();
            result.SetError();
            
            var rtnData = _einvoiceAppService.GetEInvoiceCarrierInfo(mid);

            rtnData.TimeStamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            result.SetSuccess(rtnData);
            return result;
        }

        /// <summary>
        /// 電子發票驗證手機條碼
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public DataResult<BaseResult> AuthCellPhoneCarrier(GetEInvoiceCarrierInfoRequest request, long mid)
        {
            var result = new DataResult<BaseResult>();
            result.SetError();
            result.RtnMsg = "失敗";

            var rtnData = _einvoiceAppService.AuthCellPhoneCarrier(mid, request.CarrierNumber, request.VerificationCode);
            if (rtnData.IsSuccess)
            {
                result.SetSuccess();
            }

            return result;
        }

        /// <summary>
        /// 取得發票清單
        /// </summary>
        /// <param name="request"></param>
        /// <param name="mid"></param>
        /// <returns></returns>
        public DataResult<QueryEinvoiceListResult> QueryEinvoiceList(long mid, QueryEinvoiceListRequest request)
        {
            var result = new DataResult<QueryEinvoiceListResult>();
            result.SetError();

            //判斷發票當期規格
            if (!(!string.IsNullOrWhiteSpace(request.EinvoicePeriod) &&
                Regex.IsMatch(request.EinvoicePeriod, RegexConst.InvPeriod)))
            {
                return result;
            }

            var rtnData = _einvoiceAppService.QueryEinvoiceList(mid, request.EinvoicePeriod);
            rtnData.TimeStamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            result.SetSuccess(rtnData);
            return result;

        }

        /// <summary>
        /// 取得發票明細
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DataResult<QueryEinvoiceDetailResult> QueryEinvoiceDetail(long mid, QueryEinvoiceDetailRequest request)
        {
            var result = new DataResult<QueryEinvoiceDetailResult>();
            
            result.SetError();

            //判斷發票當期規格不符就吐回
            if (!(!string.IsNullOrWhiteSpace(request.EinvoicePeriod) &&
                Regex.IsMatch(request.EinvoicePeriod, RegexConst.InvPeriod)))
            {
                return result;
            }

            //判斷發票號碼規格不符就吐回
            if (!(!string.IsNullOrWhiteSpace(request.EinvoiceNum) && Regex.IsMatch(request.EinvoiceNum, RegexConst.InvNum)))
            {
                return result;
            }

            request.EinvoiceNum = request.EinvoiceNum.ToUpper();
            var carrierInvDetail = _einvoiceAppService.GetCarrierInvDetail(mid, request.EinvoicePeriod, request.EinvoiceNum);
            
            #region 檢查DB是否已有發票明細，若無則跟財政部請求更新
            if (carrierInvDetail != null && !carrierInvDetail.ModifyDate.HasValue)
            {
                var dlResult = _einvoiceAppService.DownloadInvDetail(request.EinvoicePeriod,
                                                                    request.EinvoiceNum,
                                                                    carrierInvDetail.EinvoiceCreateDate.GetValueOrDefault(),
                                                                    carrierInvDetail.CarrierNum,
                                                                    carrierInvDetail.VerificationCode);
                if (dlResult.RtnCode != 1)
                {
                    return result;
                }
            }
            #endregion

            var invDetail = _einvoiceAppService.QueryInvDetail(mid,request.EinvoicePeriod,request.EinvoiceNum);
            if (invDetail == null)
            {
                return result;
            }
            QueryEinvoiceDetailResult queryEinvoiceDetail=new QueryEinvoiceDetailResult
            {
                EinvoiceCreateDate = invDetail.EinvoiceCreateDate,
                EinvoicePeriod = invDetail.EinvoicePeriod,
                EinvoiceNum=invDetail.EinvoiceNum,
                EinvoiceRandomNumber =invDetail.EinvoiceRandomNumber,
                EinvoiceSaleAmount=invDetail.EinvoiceSaleAmount,
                EinvoiceStoreName=invDetail.EinvoiceStoreName,
                EinvoiceStoreAddress=invDetail.EinvoiceStoreAddress,
                EinvoiceItemDetail=invDetail.EinvoiceItemDetail.IsNullOrEmpty()?null:_einvoiceAppService.deserializeEinvoiceItem(invDetail.EinvoiceItemDetail),
                CarrierType = 1,//TODO 載具類別待測試後確認值
                CarrierNumber =request.EinvoiceNum
            };
            queryEinvoiceDetail.TimeStamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            result.SetSuccess(queryEinvoiceDetail);

            return result;
        }
        #region M0037 首次登入設定帳密
        public DataResult<FirstLoginSetAccountResult> FirstLoginSetAccount(long mid, FirstLoginSetAccountRequest request, long RealIP, long ProxyIP)
        {
            string args = "登入密碼";
            string Email = string.Empty;

            var result = new DataResult<FirstLoginSetAccountResult>();
            result.SetError();

            //檢查資料格式
            if (!request.IsValid())
            {
                result.RtnMsg = request.GetFirstErrorMessage();
                return result;
    }

            //判斷登入帳號是否註冊過
            var checkUserCodeUniqueResult = _libMemberInfoService.CheckUserCodeUnique(request.UserCode);
            if (!checkUserCodeUniqueResult.IsSuccess)
            {
                result.SetError(checkUserCodeUniqueResult);
                return result;
}

            //### 密碼規則
            //string userCode = _libMemberInfoService.GetMemberData(mid).basic.Account;
            //string encryUserCode = _memberConfigCyptRepository.Encrypt_UserCode(userCode);
            var verifyPasswordResult = _commonService.VerifyPassword(request.UserCode, request.LoginPwd, mid);
            if (verifyPasswordResult != null && verifyPasswordResult.RtnCode != 1)
            {
                result.SetError(verifyPasswordResult); // 密碼不可與帳號相同 請輸入6-20個半形英數混合之密碼
                return result;
            }

            string oriLoginPWD = _memberInfoService.GetOriLoginPWD(mid);
            // 將密碼進行加密動作 
            string encryNewLoginPWD = _configCyptRepository.Hash_UserPwd(request.LoginPwd);  
            // 檢查是否跟原登入密碼相同
            var checkOldLoginPasswordSameResult = _memberInfoService.CheckOldLoginPasswordSame(encryNewLoginPWD, mid);
            if (checkOldLoginPasswordSameResult != null && checkOldLoginPasswordSameResult.RtnCode != 1)
            {
                result.SetCode(200003, args); // 您輸入的新登入密碼與原登入密碼相同
                return result;
            }

            // 確認輸入密碼是否一致
            var validConfirmLoginPwdResult = _memberInfoService.ValidConfirmLoginPwd(request.LoginPwd, request.ConfirmLoginPwd);
            if (!validConfirmLoginPwdResult)
            {
                result.SetCode(200004, args); // 兩次登入密碼輸入不同，請重新輸入
                return result;
            }

            // 變更登入帳號
            var updateAccountResult = _memberInfoService.UpdateAccountResult(mid, request.UserCode, mid.ToString(), RealIP, ProxyIP);
            if (!updateAccountResult.IsSuccess)
            {
                result.SetError(updateAccountResult);
                return result;
            }
                        
            //### 更新密碼  
            LoginPassword Editmodel = new LoginPassword
            {
                OriLoginPassword = oriLoginPWD,
                NewLoginPassword = encryNewLoginPWD
            };

            var updateResult = _memberInfoService.UpdateLoginPassword(Editmodel, mid, RealIP, ProxyIP, ref Email);
            if (!updateResult.IsSuccess)
            {
                result.SetCode(0);
                return result;
            }

            //確認是否要寄mail或訊息

            result.SetSuccess(new FirstLoginSetAccountResult());
            return result;
        }
        #endregion

        #region M0058 取得系統維護開關
        /// <summary>
        /// 取得系統維護開關
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DataResult<GetMaintainStatusResult> GetMaintainStatus(GetMaintainStatusRequest request)
        {
            var result = new DataResult<GetMaintainStatusResult>();
            result.SetError();

            var maintainStatusList = new List<GetMaintainStatusResult>();

            string appName = "icp";

            byte? functionID = null;
            if (!string.IsNullOrWhiteSpace(request.ItemID))
            {
                functionID = Convert.ToByte(request.ItemID);
            }

            var appFunctions = _libAdminService.ListAppFunctionSwitch(appName, functionID);
            appFunctions.ForEach(t => maintainStatusList.Add(new GetMaintainStatusResult
            {
                ItemID = t.FunctionID.ToString(),
                ItemName = t.FunctionName,
                Status = t.Status == 0 ? 2 : 1,
                Description = t.Status == 0 ? "系統目前正在維護中" : string.Empty
            }));

            result.SetSuccess();
            return result;
        }
        #endregion

        #region M0075 取得分行代碼清單
        /// <summary>
        /// M0075 取得分行代碼清單
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DataResult<ListBankBranchResult> ListBankBranch(ListBankBranchRequest request)
        {
            var result = new DataResult<ListBankBranchResult>();
            result.SetError();

            if (!request.IsValid())
            {
                result.RtnMsg = request.GetFirstErrorMessage();
                return result;
            }

            var items = new List<ListBankBranchItem>();

            var list = _libMemberBankService.ListBankBranchCode(request.BankCode);
            list.ForEach(t => items.Add(new ListBankBranchItem
            {
                BranchCode = t.BankBranchCode,
                BranchName = t.BankName
            }));

            var rtnData = new ListBankBranchResult
            {
                BranchList = items
            };

            result.SetSuccess(rtnData);
            return result;
        }
        #endregion
    }
}