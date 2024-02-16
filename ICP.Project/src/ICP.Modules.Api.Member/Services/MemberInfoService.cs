using AutoMapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace ICP.Modules.Api.Member.Services
{
    using Infrastructure.Core.Models.Consts;
    using Infrastructure.Core.Models;
    using Infrastructure.Core.Extensions;
    using Infrastructure.Core.Helpers;
    using Infrastructure.Core.Utils;

    using Library.Models.MemberModels;
    using Library.Models.OpenWalletApi.CustomReceiveApi;
    using Library.Repositories.MemberRepositories;
    using Library.Repositories.OpenWalletApi;

    using Models.MemberInfo;
    using Modules.Api.Member.Enums;
    using Modules.Api.Member.Models;
    using Repositories;

    public class MemberInfoService
    {
        Repositories.MemberInfoRepository _memberInfoRepository;
        MemberRegRepository _libMemberRegRepository;
        OPClientApiRepository _oPClientApiRepository;
        FingerPrintPasswordRepository _fingerPrintPasswordRepository;
        MemberConfigCyptRepository _configCyptRepository;
        MemberConfigRepository _configRepository;
        MemberBankRepository _memberBankRepository;

        public MemberInfoService(
            Repositories.MemberInfoRepository memberInfoRepository,
            FingerPrintPasswordRepository fingerPrintPasswordRepository,
            MemberRegRepository libMemberRegRepository,
            OPClientApiRepository oPClientApiRepository,
            MemberConfigCyptRepository configCyptRepository,
            MemberConfigRepository configRepository,
            MemberBankRepository memberBankRepository
            )
        {
            _memberInfoRepository = memberInfoRepository;
            _libMemberRegRepository = libMemberRegRepository;
            _oPClientApiRepository = oPClientApiRepository;
            _fingerPrintPasswordRepository = fingerPrintPasswordRepository;
            _configCyptRepository = configCyptRepository;
            _configRepository = configRepository;
            _memberBankRepository = memberBankRepository;
        }

        #region Member_AppToken

        public DataResult<MemberAppToken> GetMemberAppToken(string OPMID)
        {
            var result = new DataResult<MemberAppToken>();
            result.SetError();

            if (string.IsNullOrWhiteSpace(OPMID))
            {
                return result;
            }

            var rtnData = _memberInfoRepository.GetMemberAppToken(OPMID);
            if (rtnData == null)
            {
                return result;
            }

            result.SetSuccess(rtnData);
            return result;
        }

        /// <summary>
        /// 取得註冊狀態 
        /// 1：已完成手機號碼驗證 
        /// 2：手機號碼驗證中 
        /// 3：尚未驗證手機號碼
        /// </summary>
        /// <param name="MID"></param>
        /// <returns></returns>
        public int GetRegisterStatus(long MID)
        {
            if (MID > 0) return 1;
            if (MID == 0) return 3;

            var checkTempResult = _libMemberRegRepository.CheckMemberTemp(MID);
            if (!checkTempResult.IsSuccess) return 3;

            return 2;
        }

        public DataResult<MemberAppToken> GetMemberAppToken(long AppTokenID)
        {
            var result = new DataResult<MemberAppToken>();
            result.SetError();

            if (AppTokenID <= 0)
            {
                return result;
            }

            var rtnData = _memberInfoRepository.GetMemberAppToken(AppTokenID);
            if (rtnData == null)
            {
                return result;
            }

            result.SetSuccess(rtnData);
            return result;
        }

        public DataResult<MemberAppToken> GetMemberAppTokenByMID(long MID)
        {
            var result = new DataResult<MemberAppToken>();
            result.SetError();

            var rtnData = _memberInfoRepository.GetMemberAppTokenByMID(MID);
            if (rtnData == null)
            {
                return result;
            }

            result.SetSuccess(rtnData);
            return result;
        }

        public DataResult<string> GetMemberAuthVCode(string AuthV, ref bool mock)
        {
            var result = new DataResult<string>();
            result.SetError();

            string AuthVCode = null;
            
            if (AuthV.Length > 128)
            {
                try
                {
                    string json = _configCyptRepository.Decrypt_CustomOpenWalletEncData(AuthV);

                    var jObj = JObject.Parse(json);

                    AuthVCode = jObj.Value<string>("code");
                }
                catch { }
            }
            else if (_configRepository.AllowMock)
            {
                mock = true;
                AuthVCode = AuthV;
            }

            if (string.IsNullOrWhiteSpace(AuthVCode))
            {
                return result;
            }

            result.SetSuccess(AuthVCode);
            return result;
        }

        public DataResult<MemberAppToken> GetMemberAppTokenByAuthV(string AuthV)
        {
            var result = new DataResult<MemberAppToken>();
            result.SetError();

            var rtnData = _memberInfoRepository.GetMemberAppTokenByAuthV(AuthV);
            if (rtnData == null)
            {
                return result;
            }

            result.SetSuccess(rtnData);
            return result;
        }

        public DataResult<MemberAppToken> GetMemberAppTokenByAccount(string Account)
        {
            var result = new DataResult<MemberAppToken>();
            result.SetError();

            string enAccount = _configCyptRepository.Encrypt_UserCode(Account);
            var rtnData = _memberInfoRepository.GetMemberAppTokenByAccount(enAccount);
            if (rtnData == null)
            {
                return result;
            }

            result.SetSuccess(rtnData);
            return result;
        }

        public BaseResult UpdateMemberAppTokenExpired(long AppTokenID)
        {
            return _memberInfoRepository.UpdateMemberAppTokenExpired(AppTokenID);
        }

        public DataResult<BaseResult> AddMemberAppToken(string AuthV, bool mock = false)
        {
            var result = new DataResult<BaseResult>();
            result.SetError();

            var model = new AddMemberAppToken();
            model.AuthV = AuthV;

            #region 取得 AccessToken
            var refreshOPTokenResult = openWallet_getOPToken(ref model, mock);
            #endregion

            #region 取得 OPMID
            var getOPMIDResult = new DataResult<BaseResult>();
            getOPMIDResult.SetError();
            if (refreshOPTokenResult.IsSuccess)
            {
                getOPMIDResult = openWallet_getOPMID(ref model, mock);
            }
            #endregion

            #region 取得 OP DATA
            if (getOPMIDResult.IsSuccess)
            {
                openWallet_getOPData(ref model, mock);
            }
            #endregion

            var rtnData = _memberInfoRepository.UpdateMemberAppToken(model);
            if (!rtnData.IsSuccess)
            {
                return result;
            }

            if (string.IsNullOrEmpty(model.OPAccessToken) ||
                string.IsNullOrEmpty(model.OPMID) ||
                string.IsNullOrEmpty(model.OPCellPhone))
            {
                return result;
            }

            result.SetSuccess();
            return result;
        }

        private DataResult<BaseResult> openWallet_getOPToken(ref AddMemberAppToken model, bool mock = false)
        {
            var result = new DataResult<BaseResult>();
            result.SetError();

            var getAccessTokenResult = _oPClientApiRepository.GetAccessToken(model.AuthV, mock);
            var getAccessTokenData = getAccessTokenResult.RtnData;
            if (!getAccessTokenResult.IsSuccess)
            {
                model.OPErrorCode = getAccessTokenData.errorCode;
                model.OPErrorMessage = getAccessTokenData.errorMessage;
                result.SetError(getAccessTokenResult);
                return result;
            }

            model.OPAccessToken = getAccessTokenData.access_token;
            model.OPExpired = DateTime.ParseExact(getAccessTokenData.expires, "yyyyMMddhhmmss", CultureInfo.InvariantCulture);

            result.SetSuccess();
            return result;
        }

        private DataResult<BaseResult> openWallet_refreshOPToken(ref AddMemberAppToken model, bool mock = false)
        {
            var result = new DataResult<BaseResult>();
            result.SetError();

            var refreshResult = _oPClientApiRepository.RefreshAccessToken(model.OPAccessToken, model.OPMID, mock);
            var refreshData = refreshResult.RtnData;
            if (!refreshResult.IsSuccess)
            {
                model.OPErrorCode = refreshData.errorCode;
                model.OPErrorMessage = refreshData.errorMessage;
                result.SetError(refreshResult);
                return result;
            }

            model.OPAccessToken = refreshData.access_token;
            model.OPExpired = DateTime.ParseExact(refreshData.expires, "yyyyMMddhhmmss", CultureInfo.InvariantCulture);

            result.SetSuccess();
            return result;
        }

        private DataResult<BaseResult> openWallet_getOPMID(ref AddMemberAppToken model, bool mock = false)
        {
            var result = new DataResult<BaseResult>();
            result.SetError();

            var queryMemberMIDResult = _oPClientApiRepository.QueryMemberMID(model.OPAccessToken, mock);
            var memberMIDResult = queryMemberMIDResult.RtnData;
            if (!queryMemberMIDResult.IsSuccess)
            {
                model.OPErrorCode = memberMIDResult.errorCode;
                model.OPErrorMessage = memberMIDResult.errorMessage;
                result.SetError(queryMemberMIDResult);
                return result;
            }

            model.OPMID = memberMIDResult.mid;

            result.SetSuccess();
            return result;
        }

        private DataResult<BaseResult> openWallet_getOPData(ref AddMemberAppToken model, bool mock = false)
        {
            var result = new DataResult<BaseResult>();
            result.SetError();

            #region 取得 CellPhone
            var queryMemberInfoResult = _oPClientApiRepository.QueryMemberInfo(model.OPAccessToken, model.OPMID, mock);
            var memberInfoResult = queryMemberInfoResult.RtnData;
            if (!queryMemberInfoResult.IsSuccess)
            {
                model.OPErrorCode = memberInfoResult.errorCode;
                model.OPErrorMessage = memberInfoResult.errorMessage;
                result.SetError(queryMemberInfoResult);
                return result;
            }

            model.OPCellPhone = memberInfoResult.phone;
            #endregion

            #region 取得 MobileBarcode
            var queryMobileBarCodeResult = _oPClientApiRepository.QueryMobileBarCode(model.OPAccessToken, model.OPMID, mock);
            if (queryMobileBarCodeResult.IsSuccess)
            {
                var mobileBarCodeResult = queryMobileBarCodeResult.RtnData;
                model.OPMobileBarcode = mobileBarCodeResult.mobile_barcode;
            }
            #endregion

            result.SetSuccess();
            return result;
        }

        public DataResult<BaseResult> RefreshOPData(MemberAppToken data, bool mock = false)
        {
            var result = new DataResult<BaseResult>();
            result.SetError();

            var model = Mapper.Map<AddMemberAppToken>(data);

            if (data.OPExpired > DateTime.Now)
            {
                var refreshOPTokenResult = openWallet_refreshOPToken(ref model, mock);
                if (!refreshOPTokenResult.IsSuccess)
                {
                    return result;
                }
            }

            var getOPDataResult = openWallet_getOPData(ref model, mock);
            if (!getOPDataResult.IsSuccess)
            {
                return result;
            }

            var rtnData = _memberInfoRepository.UpdateMemberAppToken(model);
            if (!rtnData.IsSuccess)
            {
                result.SetError(rtnData);
                return result;
            }

            result.SetSuccess();
            return result;
        }
        #endregion

        public BaseResult CheckMID(long MID, CheckMIDType checkMIDType)
        {
            var result = new BaseResult();
            result.SetError();

            if ((checkMIDType == CheckMIDType.NoRegister && MID > 0) ||
                (checkMIDType == CheckMIDType.TempRegister && MID >= 0) ||
                (checkMIDType == CheckMIDType.Register && MID <= 0))
            {
                result.RtnMsg = "MID ERROR";
                return result;
            }

            result.SetSuccess();
            return result;
        }

        #region 檢查簡訊驗證纇別
        /// <summary>
        /// 檢查簡訊驗證纇別
        /// </summary>
        /// <param name="ApiSMSAuthType"></param>
        /// <param name="OPMID"></param>
        /// <param name="MID"></param>
        /// <returns></returns>
        public BaseResult CheckSMSAuthType(long ApiSMSAuthType, MemberAppToken appToken, long MID)
        {
            BaseResult result = null;

            switch (ApiSMSAuthType)
            {
                case 1://註冊時發送簡訊
                    if (appToken.MID >= 0 || MID > 0)
                    {
                        result = new BaseResult { RtnMsg = string.Format("SMSAuthType {0} use for register status, please register!", ApiSMSAuthType) };
                    }
                    break;
                case 2://忘記登入密碼發送簡訊
                case 3://忘記登入帳號發送簡訊
                case 4://忘記圖形密碼發送簡訊
                case 5://換機驗證發送簡訊
                    if (appToken.MID <= 0 || MID > 0)
                    {
                        result = new BaseResult { RtnMsg = string.Format("SMSAuthType {0} use for logout status, please logout!", ApiSMSAuthType) };
                    }
                    break;
                case 6://忘記原登入密碼發送簡訊(已登入)
                case 7://忘記原圖形密碼發送簡訊(已登入)
                    if (appToken.MID <= 0 || MID <= 0)
                    {
                        result = new BaseResult { RtnMsg = string.Format("SMSAuthType {0} use for login status, please login!", ApiSMSAuthType) };
                    }
                    break;
                default:
                    result = new BaseResult { RtnMsg = "SMSAuthType {0} Invlid!" };
                    break;
            }

            if (result != null) return result;

            result = new BaseResult();
            result.SetSuccess();
            return result;
        }
        #endregion

        #region API簡訊驗證類別 轉 DB簡訊驗證類別
        /// <summary>
        /// API簡訊驗證類別 轉 DB簡訊驗證類別
        /// </summary>
        /// <param name="ApiSMSAuthType"></param>
        /// <returns></returns>
        public byte ApiSMSAuthType_toDBAuthType(long ApiSMSAuthType)
        {
            byte result = 0;
            switch (ApiSMSAuthType)
            {
                case 1: //1：註冊時發送簡訊
                    result = 1;
                    break;

                case 2: //2：忘記登入密碼發送簡訊
                    result = 6;
                    break;

                case 3: //3：忘記登入帳號發送簡訊
                    result = 5;
                    break;

                case 4: //4：忘記圖形鎖發送簡訊
                    result = 8;
                    break;

                case 5: //5：換機驗證發送簡訊
                    result = 2;
                    break;

                case 6: //6：忘記原登入密碼發送簡訊(已登入)
                    result = 4;
                    break;

                case 7: //7：忘記原圖形密碼發送簡訊(已登入)
                    result = 8;
                    break;

                case 8: //8：修改手機號碼
                    result = 7;
                    break;

                default:
                    throw new ArgumentException("SMSAuthType Error");
            }

            return result;
        }
        #endregion

        #region 會員類別
        public string MemberType2Class(short MemberType)
        {
            if ((MemberType & 2) == 2) return "02";

            else if ((MemberType & 1) == 1) return "01";

            return string.Empty;
        }

        public bool MemberType2Minor(short MemberType)
        {
            if ((MemberType & 16) == 16)
                return true;
            else
                return false;
        }
        #endregion

        public BaseResult CheckValueWithAppToken(MemberAppToken model, Func<MemberAppToken, bool> check)
        {
            var result = new BaseResult();

            if (!check(model))
            {
                result.SetError();
                return result;
            }

            result.SetSuccess();
            return result;
        }

        public MemberVerifyStatus GetMemberVerifyStatus(long MID)
        {
            var result = _memberInfoRepository.GetMemberVerifyStatus(MID);
            AesCryptoHelper Aes = new AesCryptoHelper
            {
                Key = GlobalConfigUtil.FingerPrintPasswordHashKey,
                Iv = GlobalConfigUtil.FingerPrintPasswordHashIV
            };
            
            result.FingerPrintPwd = ( !string.IsNullOrEmpty(result.FingerPrintPwd) ) ? Aes.Decrypt(result.FingerPrintPwd) : result.FingerPrintPwd;

            return result;
        }

        public CheckVerifyStatusResult GetCheckVerifyStatusResultResult(MemberVerifyStatus verifyStatus)
        {
            var rtnData = new CheckVerifyStatusResult
            {
                IsPwdPass = verifyStatus.IsPwdPass,
                GraphicLock = verifyStatus.IsGraphicLock,
                IsIDNOPass = verifyStatus.IsIDNOPass,
                IsTeenagersPass = verifyStatus.IsTeenagersPass,
                LoginPwdStatus = verifyStatus.LoginPwdStatus,
                SecPwdStatus = verifyStatus.SecPwdStatus,
                GraphicLockPwdStatus = verifyStatus.GraphicLockStatus,
                SecPwdRtnMsg = "請重新設定安全密碼",
                IsDefaultAct = verifyStatus.IsDefaultAct
            };

            /*
             * 下一步:
             * 1：身分證驗證(頁面P5)
             * 7：首次登入預設帳密設定頁(員工預設帳號)
             * 2：設定安全密碼(頁面P6)
             * 3：未成年註冊驗證中
             * 4：登入密碼一年到期
             * 5：安全密碼一年到期
             * 6：圖形密碼一年到期
             * 100：驗證完成
             */
            if (!rtnData.IsIDNOPass && rtnData.IsTeenagersPass)
            {
                rtnData.NextStep = 1;
            }
            else if (rtnData.IsDefaultAct)
            {
                rtnData.NextStep = 7;
            }
            else if (!rtnData.IsPwdPass)
            {
                rtnData.NextStep = 2;
            }
            else if (!rtnData.IsTeenagersPass)
            {
                rtnData.NextStep = 3;
            }
            else if (rtnData.LoginPwdStatus != 0)
            {
                rtnData.NextStep = 4;
            }
            else if (rtnData.SecPwdStatus != 0)
            {
                rtnData.NextStep = 5;
            }
            else if (rtnData.GraphicLockPwdStatus != 0)
            {
                rtnData.NextStep = 6;
            }
            else
            {
                rtnData.NextStep = 100;
            }

            return rtnData;
        }

        /// <summary>
        /// Socket Server使用者驗證資訊取得
        /// </summary>
        /// <param name="memberData"></param>
        /// <returns></returns>
        public PrivateUserAuthResult PrivateUserAuthResult(MemberDataModel memberData)
        {
            var basic = memberData.basic;
            var rtnData = new PrivateUserAuthResult
            {
                OpwMID = basic.OPMID,
                IcpMID = basic.ICPMID
            };
            return rtnData;
        }

        /// <summary>
        /// 取得登入資訊
        /// </summary>
        /// <param name="memberData">會員資料</param>
        /// <param name="verifyStatus">驗證狀態</param>
        /// <param name="appToken">App Token</param>
        /// <param name="agreeItems">同意項目</param>
        /// <returns></returns>
        public GetLoginInfoResult GetLoginInfoResult(MemberDataModel memberData, MemberVerifyStatus verifyStatus, MemberAppToken appToken, List<MemberAgreeResult> agreeItems, GetPasswordStatusResult getPasswordStatusResult, string Barcode)
        {
            var basic = memberData.basic;
            var detail = memberData.detail;
            if (appToken == null) appToken = new MemberAppToken();

            var rtnData = new GetLoginInfoResult
            {
                MID = basic.MID,
                OpwMID = basic.OPMID,
                IcpMID = basic.ICPMID,
                LoginTokenID = appToken.LoginTokenID,
                TokenExpireTime = appToken.LoginTokenExpired.ToString(FormatConst.DateTime),
                LastLoginDate = basic.LastLoginDate == null ? null : basic.LastLoginDate.Value.ToString(FormatConst.DateTime),
                UserCode = basic.Account,
                CName = basic.CName,
                CellPhone = detail.CellPhone,
                EMail = detail.Email,
                Idno = detail.IDNO,
                isOverSea = verifyStatus.IsOverSea, //!string.IsNullOrEmpty(detail.UniformID),
                UniformID = detail.UniformID,
                CarrierNum = appToken.OPMobileBarcode,
                AreaID = detail.AreaID,
                Address = detail.Address,
                LevelID = basic.LevelID,
                MemberClass = MemberType2Class(basic.MemberType),
                IsMinor = MemberType2Minor(basic.MemberType),
                IsCellPhoneAuth = verifyStatus.IsCellPhoneAuth,
                IsEmailAuth = verifyStatus.IsEmailAuth,
                IDNOAuthStatus = verifyStatus.IDNOAuthStatus,
                IsP33Auth = verifyStatus.IsP33Auth ? 1 : 0,
                BankAuthStatus = verifyStatus.BankAuthStatus,
                IsExistsSetGraphicLock = verifyStatus.IsGraphicLock,
                GraphicLockStatus = getPasswordStatusResult.GraphicLockStatus,
                FingerPrintPwd = verifyStatus.FingerPrintPwd,
                IsNickName = string.IsNullOrEmpty(detail.NickName) ? false : true,
                NickName = detail.NickName,
                RCCode = detail.ReferrerCode,
                AgreeItems = agreeItems,
                IsDefaultAct = verifyStatus.IsDefaultAct,
                Barcode = Barcode
            };

            return rtnData;
        }

        public List<ListNotifyMessageItem> NotifyMessageToItem(List<MemberNotifyMessage> list)
        {
            return list.Select(t => new ListNotifyMessageItem
            {
                MsgID = t.NotifyMessageID,
                MsgType = t.CategoryName,
                Subject = t.Subject,
                CreateDate = t.CreateDate.ToString(FormatConst.DateMinute),
                isRead = t.isRead
            }).ToList();
        }

        public GetNotifyMessageResult GetNotifyMessageResult(MemberNotifyMessageDetail model)
        {
            return new GetNotifyMessageResult
            {
                Subject = model.Subject,
                Body = model.Body,
                CreateDate = model.CreateDate.ToString(FormatConst.DateTime)
            };
        }


        /// <summary>
        /// 取得指紋密碼狀態
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public bool GetFingerPrintPasswordStatus(long mid)
        {
            var model = _fingerPrintPasswordRepository.GetFingerPrintPasswordStatus(mid);
            return (model != null && model.RtnCode == 1);
        }

        /// <summary>
        /// 更新指紋密碼
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public string UpdateFingerPrintPassword(long mid, string deviceID, bool? status = null, long realIP = 0, long poxyIP = 0)
        {
            //更新設定狀態，並產生一組Hash密碼紀錄
            bool dbStatus = GetFingerPrintPasswordStatus(mid);

            AesCryptoHelper Aes = new AesCryptoHelper
            {
                Key = GlobalConfigUtil.FingerPrintPasswordHashKey,
                Iv = GlobalConfigUtil.FingerPrintPasswordHashIV
            };
            
            string guid = Guid.NewGuid().ToString();

            
            FingerPrintPasswordModel model = new FingerPrintPasswordModel
            {
                DeviceID = deviceID,
                MID = mid,
                Password = Aes.Encrypt(guid),
                RealIP = realIP,
                ProxyIP = poxyIP,
                Status = (status == null) ? dbStatus : status.GetValueOrDefault()
            };

            BaseResult baseModel = _fingerPrintPasswordRepository.UpdateFingerPrintPassword(model);
            return (status == true) ? guid : baseModel.RtnMsg ;
        }

        /// <summary>
        /// 比對指紋密碼是否正確
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public BaseResult CheckFingerPrintPassword(long mid, string password, string deviceID, long realIP, long poxyIP)
        {
            AesCryptoHelper Aes = new AesCryptoHelper
            {
                Key = GlobalConfigUtil.FingerPrintPasswordHashKey,
                Iv = GlobalConfigUtil.FingerPrintPasswordHashIV
            };

            FingerPrintPasswordModel model = new FingerPrintPasswordModel
            {
                DeviceID = deviceID,
                MID = mid,
                Password = Aes.Encrypt(password),
                RealIP = realIP,
                ProxyIP = poxyIP
            };

            return _fingerPrintPasswordRepository.CheckFingerPrintPassword(model);
        }

        /// <summary>
        /// 取得當前指紋密碼
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public string GetFingerPrintPassword(long mid)
        {
            var model = _fingerPrintPasswordRepository.GetFingerPrintPassword(mid);
            string password = null;

            if (model != null && model.RtnCode == 1 && !string.IsNullOrWhiteSpace(model.RtnMsg))
            {
                AesCryptoHelper Aes = new AesCryptoHelper {
                    Key = GlobalConfigUtil.FingerPrintPasswordHashKey,
                    Iv = GlobalConfigUtil.FingerPrintPasswordHashIV
                };

                password = Aes.Decrypt(model.RtnMsg);
            }
            return password;
        }

        

        #region M0018 取得密碼設定開關狀態
        /// <summary>
        /// 取得密碼設定開關狀態
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public GetPasswordStatusResult GetPasswordStatus(long mid)
        {
            return _memberInfoRepository.GetPasswordStatus(mid);
        }
        #endregion

        #region 驗證安全密碼
        /// <summary>
        /// 檢查安全密碼是否正確
        /// </summary>
        /// <param name="MID">會員代號</param>=
        /// <param name="PayPwd">安全密碼</param>
        /// <returns></returns>
        public BaseResult Check_Member_Security_CheckSecPwd(long MID, string PayPwd, long realIP, long proxyIP)
        {
            return _memberInfoRepository.Check_Member_Security_CheckSecPwd(MID, PayPwd, realIP, proxyIP);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oriLoginPWD"></param>
        /// <param name="OriginalLoginPwd"></param>
        /// <returns></returns>
        public bool CheckLoginPWD(string oriLoginPWD, string OriginalLoginPwd)
        {
            bool confirm = (oriLoginPWD != OriginalLoginPwd) ? false : true;
            return confirm;
        }

        /// <summary>
        /// 檢查密碼是否與前一次相同
        /// </summary>
        /// <param name="loginPassword"></param>
        /// <param name="mid"></param>
        /// <returns></returns>
        public BaseResult CheckOldLoginPasswordSame(string loginPassword, long mid)
        {
            return _memberInfoRepository.CheckOldLoginPasswordSame(loginPassword, mid);
        }

        #region 驗證安全密碼
        public BaseResult AuthSecPasswordProcess(string Password, ref string errorMessage)
        {
            BaseResult result = new BaseResult();
            result.SetError();            
            
            errorMessage = "安全密碼請輸入6碼數字";

            //## 檢查格式
            //## 安全密碼不可使用相同或連續數字

            Regex regExSecPassword = new Regex("^[0-9]{6}$");

            string FChar = Password[0].ToString();
            string tempChar = string.Empty;
            bool same = true;                      //判斷是否相同
            bool repeat = true;                    //判斷是否連續

            int tempCompare = 0;
            int nowCompare = 0;            

            if (!regExSecPassword.IsMatch(Password))
            {
                result.RtnCode = 200005; //安全密碼請輸入6碼數字                
                return result;
            }
            
            for (int i = 0; i < Password.Length; i++)
            {
                //### 若某一字元跟首字元不同,即為不相同文字
                if (Password[i].ToString() != FChar) { same = false; }

                //### 若為純數字或純英文 進入字元比對
                if (!string.IsNullOrWhiteSpace(tempChar))
                {
                    nowCompare = string.CompareOrdinal(Password[i].ToString(), tempChar);
                    if (nowCompare == 0 || (nowCompare != tempCompare && tempCompare != 0)) { repeat = false; }
                    tempCompare = nowCompare;
                }

                tempChar = Password[i].ToString();
            }

            //### 不可相同英數字
            if (same) {
                result.RtnCode = 200024;//安全密碼不可使用相同或連續數字，請重新輸入6碼數字之密碼                 
                return result;
            }
            if (repeat) {
                result.RtnCode = 200024;//安全密碼不可使用相同或連續數字，請重新輸入6碼數字之密碼               
                return result;
            }
                        
            result.SetSuccess();
            return result;
        }
        #endregion

        #region 確認輸入的安全密碼是否一致
        public bool ValidConfirmSecPwd(string NewSecPwd, string ConfirmSecPwd)
        {
            bool confirm = (NewSecPwd != ConfirmSecPwd) ? false : true;
            return confirm;
        }

        /// <summary>
        /// 檢查安全密碼是否正確
        /// </summary>
        /// <param name="tradePassword">安全密碼</param>
        /// <param name="mid">會員編號</param>
        /// <returns></returns>
        public bool CheckOldSecPasswordSame(string SecPassword, long mid)
        {
            return _memberInfoRepository.CheckOldPayPasswordSame(SecPassword, mid);
        }

        #endregion

        /// <summary>
        /// 取得會員目前安全密碼
        /// </summary>
        /// <param name="tradePassword">安全密碼</param>
        /// <param name="mid">會員編號</param>
        /// <returns></returns>
        public string GetOriSecPWD(long mid)
        {
            return _memberInfoRepository.GetOriSecPWD(mid);
        }

        /// <summary>
        /// 更新安全密碼
        /// </summary>
        /// <param name="model">TradePassword Model</param>
        /// <param name="mid">會員代碼</param>
        /// <param name="email">回傳的Email</param>
        /// <returns></returns>
        public bool UpdateSecPassword(SecPassword model, long mid, long realIP, long proxyIP, ref string email)
        {
            return _memberInfoRepository.UpdateSecPassword(model, mid, realIP, proxyIP, ref email);
        }
               

        #region 確認輸入的登入密碼是否一致  
        public bool ValidConfirmLoginPwd(string NewLoginPwd, string ConfirmLoginPwd)
        {
            bool confirm = (NewLoginPwd != ConfirmLoginPwd) ? false : true;
            return confirm;
        }
        #endregion



        /// <summary>
        /// 取得會員目前登入密碼
        /// </summary>
        /// <param name="mid">會員編號</param>
        /// <returns></returns>
        public string GetOriLoginPWD(long mid)
        {
            return _memberInfoRepository.GetOriLoginPWD(mid);
        }

        /// <summary>
        /// 更新登入密碼
        /// </summary>
        /// <param name="model">TradePassword Model</param>
        /// <param name="mid">會員代碼</param>
        /// <param name="email">回傳的Email</param>
        /// <returns></returns>
        public BaseResult UpdateLoginPassword(LoginPassword model, long mid, long realIP, long proxyIP, ref string email)
        {
            return _memberInfoRepository.UpdateLoginPassword(model, mid, realIP, proxyIP, ref email);
        }

        #region M0039 記錄略過修改安全密碼

        public BaseResult UpdateIgnoreModifyPayPwdDate(long mid)
        {
            return _memberInfoRepository.UpdateIgnoreModifyPayPwdDate(mid);
        }

        #endregion

        #region M0038 記錄略過修改登入密碼
        public BaseResult UpdatePwdIgnorDate(long mid)
        {
            return _memberInfoRepository.UpdatePwdIgnorDate(mid);
        }
        #endregion

        #region 驗證暱稱格式
        public BaseResult VaildNickName(string NickName)
        {
            var result = new BaseResult();

            if (NickName.Length < 2 || NickName.Length > 20)
            {
                result.SetCode(100007); // 只能輸入2~20字
                return result;
            }

            //待確認是否要判斷不雅文字


            if (NickName.IndexOf("  ") != -1 || NickName.StartsWith(" ") || NickName.EndsWith(" "))
            {
                result.SetCode(100007); // 符號僅接受半型「@」、「-」、「&」、「‧」及空格。（空格不可連續輸入，且頭尾不可為空格）
                return result;
            }

            result.SetSuccess();
            return result;
        }

        #endregion

        /// <summary>
        /// 取得銀行清單
        /// </summary>
        /// <returns></returns>
        public GetListBankInfoResult GetListBankInfo()
        {
            List<MemberBankDetail> bankDetails = _memberInfoRepository.ListBankDetail();

            var cooperateBanks = bankDetails.Where(t => t.isCooperate == true).ToList();

            var coopBanks = new List<CoopBank>();
            cooperateBanks.ForEach(t =>
            {
                coopBanks.Add(new CoopBank
                {
                    BankCode = t.BankCode,
                    BankName = t.BankName,
                    BankShortName = t.BankShortName,
                    FillBranchInfo = t.AppFillBranchInfo == true ? 1 : 0
                });
            });

            var nonCooperateBanks = bankDetails.Where(t => t.isCooperate == false).ToList();

            var nonCoopBanks = new List<NonCoopBank>();
            nonCooperateBanks.ForEach(t =>
            {
                nonCoopBanks.Add(new NonCoopBank
                {
                    BankCode = t.BankCode,
                    BankName = t.BankName,
                    BankShortName = t.BankShortName
                });
            });

            return new GetListBankInfoResult
            {
                CoopBank = coopBanks,
                NonCoopBank = nonCoopBanks
            };
        }

        /// <summary>
        /// 取得提領資訊
        /// </summary>
        /// <param name="MID"></param>
        /// <returns></returns>
        public WithdrawBalanceInfo GetBankTransferInfo(long MID)
        {
            return _memberInfoRepository.GetBankTransferInfo(MID);
        }

        /// <summary>
        /// 刪除綁定銀行帳戶
        /// </summary>
        /// <param name="MID"></param>
        /// <param name="Category"></param>
        /// <param name="BankCode"></param>
        /// <returns></returns>
        public BaseResult DeleteBankAccount(long MID, byte Category, string BankCode, long AccountID)
        {
            return _memberInfoRepository.DeleteMemberBankAccount(MID, Category, BankCode, AccountID);
        }

        /// <summary>
        /// 銀行帳號遮罩(僅顯示後五碼)
        /// </summary>
        /// <param name="bankAccount"></param>
        /// <returns></returns>
        public string ConcealBankAccount(string bankAccount)
        {
            if (string.IsNullOrWhiteSpace(bankAccount) || bankAccount.Length <= 5)
            {
                return bankAccount;
            }

            string result = bankAccount.Substring(bankAccount.Length - 5, 5);

            for (int i = 0; i < bankAccount.Length - 5; i++)
            {
                result = "*" + result;
            }

            return result;
        }

        #region 修改Email
        public BaseResult UpdateEmailAddress(long mid, string Email)
        {
            return _memberInfoRepository.UpdateEmailAddress(mid, Email);
        }
        #endregion
               
        #region 修改暱稱
        public BaseResult UpdateNickName(long MID, string NickName)
        {
            var result = new BaseResult();
             
            result = _memberInfoRepository.UpdateNickName(MID, NickName);

            if (result.RtnCode == 1)
            {
                result.SetSuccess();
                return result;
            }
            
            return result;
        }


        #endregion

        public BaseResult CheckEmail(string Email, long MID)
        {
            return _memberInfoRepository.CheckEmail(Email, MID);
        }

        public BaseResult UploadAuthIDNOFiles(System.Web.HttpPostedFileBase[] files, ref List<string> realFilePathes)
        {
            BaseResult result = new BaseResult();

            int fileSize = 5;

            string[] contentTypeArray = new string[] { "image/jpeg", "image/gif", "image/pjpeg", "image/png" };

            string IDNOPath = _configRepository.IDNOPath;

            string fileDirPath = System.Web.HttpContext.Current.Server.MapPath(IDNOPath) + string.Format("{0:yyyyMM}", DateTime.Now) + "/";

            foreach (var file in files)
            {
                string realFilePath = string.Empty;

                result = UploadFile(file, contentTypeArray, fileSize, fileDirPath, ref realFilePath);
                realFilePathes.Add(realFilePath);
            }

            return result;
        }

        private BaseResult UploadFile(System.Web.HttpPostedFileBase file, string[] contentTypeArray, int fileSize, string fileDirPath, ref string realFilePath)
        {
            fileSize = 1024 * 1024 * fileSize;

            if (!contentTypeArray.Contains(file.ContentType))
            {
                return new BaseResult
                {
                    RtnCode = 0,
                    RtnMsg = string.Format("無法上傳檔案，檔案格式不接受：{0}", file.FileName)
                };
            }

            if (file.ContentLength > fileSize)
            {
                return new BaseResult
                {
                    RtnCode = 0,
                    RtnMsg = string.Format("無法上傳檔案，檔案大小限制{0}MB以下", fileSize)
                };
            }

            string realFileName = Guid.NewGuid().ToString() + DateTime.Now.ToString("_yyyyMMddHHmmss") + Path.GetExtension(file.FileName);

            realFilePath = Path.Combine(fileDirPath, realFileName);

            if (!Directory.Exists(fileDirPath))
            {
                Directory.CreateDirectory(fileDirPath);
            }
            file.SaveAs(realFilePath);

            realFilePath = _configRepository.IDNOPath + string.Format("{0:yyyyMM}", DateTime.Now) + "/" + realFileName;

            return new BaseResult
            {
                RtnCode = 1,
                RtnMsg = "成功"
            };
        }

        /// <summary>
        /// 判斷是否為海外會員
        /// </summary>
        /// <param name="Nationality"></param>
        /// <returns></returns>
        public bool IsOverseas(long Nationality)
        {
            return Nationality != 1206;
        }

        /// <summary>
        /// 會員資料填寫
        /// </summary>
        /// <param name="MID"></param>
        /// <param name="request"></param>
        /// <param name="Birthday"></param>
        /// <param name="UniformID"></param>
        /// <returns></returns>
        public BaseResult UpdateMemberDetail(long MID, string CName, MemberDetailModel detailModel)
        {
            return _memberInfoRepository.UpdateMemberDetail(MID, CName, detailModel);
        }

        /// <summary>
        /// 新增法定代理人資料
        /// </summary>
        /// <param name="MID"></param>
        /// <param name="legalRepData"></param>
        /// <param name="RealIP"></param>
        /// <param name="ProxyIP"></param>
        /// <returns></returns>
        public BaseResult AddAuthTeenagersLegalDetail(long MID, LegalRepData legalRepData, long RealIP, long ProxyIP)
        {
            return _memberInfoRepository.AddAuthTeenagersLegalDetail(MID, legalRepData, RealIP, ProxyIP);
        }

        /// <summary>
        /// 新增未成年待審資料
        /// </summary>

        /// <param name="MID"></param>
        /// <param name="RealIP"></param>
        /// <param name="ProxyIP"></param>
        /// <returns></returns>
        public BaseResult AddAuthTeenagers(long MID, long RealIP, long ProxyIP)
        {
            return _memberInfoRepository.AddAuthTeenagers(MID, RealIP, ProxyIP);
        }

        /// <summary>
        /// 計算年齡
        /// </summary>
        /// <param name="birthDay"></param>
        /// <returns></returns>
        public int CalculateAge(DateTime birthDay)
        {
            int years = DateTime.Now.Year - birthDay.Year;

            if ((birthDay.Month > DateTime.Now.Month) || (birthDay.Month == DateTime.Now.Month && birthDay.Day > DateTime.Now.Day))
                years--;

            return years;
        }

        /// <summary>
        /// 取得發證地點代號(依發證日期)
        /// </summary>
        /// <param name="IssueLocationID">發證地點代號</param>
        /// <param name="IssueDate">發證日期</param>
        /// <returns></returns>
        public string GetLocationByIssueDate(string IssueLocationID, DateTime IssueDate)
        {
            //五都合併前
            if (IssueDate < new DateTime(2010, 12, 27))
            {
                switch (IssueLocationID)
                {
                    case "10001"://北縣
                    case "65000"://新北市
                        return "10001";
                    case "68000"://桃市
                    case "10003"://桃縣
                        return "10003";
                    case "66000"://中市
                    case "10006"://中縣
                        return "10006";
                    case "67000"://南市
                    case "10011"://南縣
                        return "10011";
                    case "64000"://高市
                    case "10012"://高縣
                        return "10012";
                    default:
                        return IssueLocationID;
                }
            }
            //五都合併後
            else
            {
                switch (IssueLocationID)
                {
                    case "10001"://北縣
                    case "65000"://新北市
                        return "65000";
                    case "68000"://桃市
                    case "10003"://桃縣
                        return "68000";
                    case "66000"://中市
                    case "10006"://中縣
                        return "66000";
                    case "67000"://南市
                    case "10011"://南縣
                        return "67000";
                    case "64000"://高市
                    case "10012"://高縣
                        return "64000";
                    default:
                        return IssueLocationID;
                }
            }
        }
               
        /// <summary>
        /// 變更登入帳號
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="UserCode"></param>
        /// <param name="CreateUser"></param>
        /// <param name="realIP"></param>
        /// <param name="proxyIP"></param>
        /// <returns></returns>
        public BaseResult UpdateAccountResult(long mid, string UserCode, string CreateUser, long realIP, long proxyIP)
        {
            //帳號加密
            string enAccount = _configCyptRepository.Encrypt_UserCode(UserCode);

            return _memberInfoRepository.UpdateAccountResult(mid, enAccount, CreateUser, realIP, proxyIP);
        }

        
        /// <summary>
        /// OP會員手機條碼異動
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResult openWallet_NoticeMobileBarcode(NoticeMobileBarcodeRequest request)
        {
            BaseResult result =new BaseResult();
            result.SetError();


            var getCarrierModel=_memberInfoRepository.GetCarrierDetail(request.mid);
            getCarrierModel.CarrierNum = request.mobile_barcode;

            if (getCarrierModel.RtnCode == 1)
            {
                result = _memberInfoRepository.AddCarrierDetail(getCarrierModel);
            }
            else 
            {
                result.RtnCode = getCarrierModel.RtnCode;
                result.RtnMsg = getCarrierModel.RtnMsg;
            }

            return result;
        }

        /// <summary>
        /// 取得會員餘額
        /// </summary>
        /// <param name="MID"></param>
        /// <returns></returns>
        public UserCoins GetUserCoins(long MID)
        {
            return _memberInfoRepository.GetUserCoins(MID);
        }

        /// <summary>
        /// 確認是否有未完成的提領資料
        /// </summary>
        /// <param name="MID"></param>
        /// <returns></returns>
        public bool CheckBankTransfer(long MID)
        {
            return _memberInfoRepository.CheckBankTransfer(MID);
        }

        /// <summary>
        /// 會員結清
        /// </summary>
        /// <param name="MID"></param>
        /// <param name="Modifier"></param>
        /// <param name="RealIP"></param>
        /// <param name="ProxyIP"></param>
        /// <returns></returns>
        public BaseResult CloseMemberAccount(long MID, byte Source, string Modifier, long RealIP, long ProxyIP)
        {
            return _memberInfoRepository.CloseMemberAccount(MID, Source, Modifier, RealIP, ProxyIP);
        }

        /// <summary>
        /// 確認會員是否有待撥款項
        /// </summary>
        /// <param name="MID"></param>
        /// <returns></returns>
        public bool CheckUnAllocateTrade(long MID)
        {
            return _memberInfoRepository.CheckUnAllocateTrade(MID);
        }

        /// <summary>
        /// 取得法定代理人資料
        /// </summary>
        /// <param name="MID"></param>
        /// <returns></returns>
        public List<TeenagersLegalDetail> ListTeenagersLegalDetail(long MID)
        {
            return _memberInfoRepository.ListTeenagersLegalDetail(MID);
        }
    }
}

