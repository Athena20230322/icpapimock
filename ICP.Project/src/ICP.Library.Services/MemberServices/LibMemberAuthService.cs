using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Services.MemberServices
{
    using Infrastructure.Core.Models;
    using Infrastructure.Core.Extensions;
    using Library.Repositories.MemberRepositories;
    using Library.Models.MemberModels;
    using Library.Repositories.OpenWalletApi;
    using Models.OpenWalletApi.CustomSendApi;
    using ICP.Infrastructure.Core.Models.Consts;
    using System.Text.RegularExpressions;

    public class LibMemberAuthService
    {
        MemberAuthRepository _memberAuthRepository;
        OPCustomApiRepository _oPCustomApiRepository;
        MemberInfoRepository _memberInfoRepository;

        public LibMemberAuthService(
            MemberAuthRepository memberAuthRepository,
            OPCustomApiRepository oPCustomApiRepository,
            MemberInfoRepository memberInfoRepository
            )
        {
            _memberAuthRepository = memberAuthRepository;
            _oPCustomApiRepository = oPCustomApiRepository;
            _memberInfoRepository = memberInfoRepository;
        }

        /// <summary>
        /// 新增簡訊驗證
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public SMSAuthResult AddAuthSMS(SMSAuth model)
        {
            if (model.AuthType == 1 || model.AuthType == 7)
            {
                return _memberAuthRepository.AddAuthCellPhone(model);
            }
            else
            {
                return _memberAuthRepository.AddAuthSMS(model);
            }
        }

        /// <summary>
        /// 簡訊驗證
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public SMSAuthVerifyResult UpdateAuthSMS(SMSAuthVerify model)
        {
            return _memberAuthRepository.UpdateAuthSMS(model);
        }

        /// <summary>
        /// 簡訊驗證
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public SMSAuthVerifyResult UpdateAuthCellPhoneStatus(SMSAuthVerify model)
        {
            return _memberAuthRepository.UpdateAuthCellPhoneStatus(model);
        }

        /// <summary>
        /// 檢查驗證次數
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult CheckAuthSMSCount(SMSAuth model)
        {
            return _memberAuthRepository.CheckAuthSMSCount(model);
        }

        /// <summary>
        /// 新增P33驗證主表
        /// </summary>
        /// <param name="MID"></param>
        /// <param name="IDNO"></param>
        /// <param name="AuthStatus"></param>
        /// <returns></returns>
        public BaseResult AddAuthP33(P33AuthResult model)
        {
            return _memberAuthRepository.AddAuthP33(model);
        }

        /// <summary>
        /// 新增/修改IDNO驗證主表
        /// </summary>
        /// <param name="model"></param>
        /// <param name="RealIP"></param>
        /// <param name="ProxyIP"></param>
        /// <returns></returns>
        public BaseResult AddAuthIDNO(AuthIDNO model, long RealIP, long ProxyIP)
        {
            return _memberAuthRepository.AddAuthIDNO(model, RealIP, ProxyIP);
        }

        /// <summary>
        /// 確認身分證/居留證字號是否重複
        /// </summary>
        /// <param name="IDNO"></param>
        /// <param name="IsOversea"></param>
        /// <param name="MID"></param>
        /// <returns></returns>
        public CheckIdnoRepeatModel CheckIdnoRepeat(string IDNO, long MID, bool IsOversea)
        {
            return _memberAuthRepository.CheckIdnoRepeat(IDNO, MID, IsOversea);
        }

        /// <summary>
        /// 檢查並綁定
        /// </summary>
        /// <param name="memberData">會員資料</param>
        /// <param name="appToken">appToken</param>
        /// <param name="Source">來源 : 0: App 1: 電支後台 2:排程</param>
        /// <param name="RealIP">RealIP</param>
        /// <param name="ProxyIP">ProxyIP</param>
        /// <returns></returns>
        public BaseResult CheckOPBind(MemberDataModel memberData, MemberAppToken appToken, byte Source, long RealIP = 0, long ProxyIP = 0)
        {
            var result = new BaseResult();
            result.SetError();

            //綁定類型 0: 綁定 1:解綁
            byte Type = 0;
            var basic = memberData.basic;

            if (appToken.MID <= 0)
            {
                var bindAppTokenResult = BindAppToken(basic.MID, appToken.AppTokenID, Type);
                if (!bindAppTokenResult.IsSuccess)
                {
                    result.SetError(bindAppTokenResult);
                    return result;
                }
            }

            if ((basic.AuthType & 2) == 2 && string.IsNullOrWhiteSpace(basic.OPMID))
            {
                var bindOPAccountModel = new BindOPAccountModel
                {
                    MID = basic.MID,
                    OPMID = appToken.OPMID,
                    Type = Type,
                    Source = Source
                };

                long NotifyRecord = 0;
                var bindOPAccountResult = BindOPAccount(bindOPAccountModel, ref NotifyRecord, RealIP, ProxyIP);
                if (!bindOPAccountResult.IsSuccess)
                {
                    result.SetError(bindOPAccountResult);
                    return result;
                }

                var bindOPAccountNotifyResult = BindOPAccountNotify(Type, basic.MID, appToken.OPMID, basic.ICPMID, NotifyRecord, RealIP, ProxyIP);
            }

            result.SetSuccess();
            return result;
        }


        /// <summary>
        /// 檢查並解綁
        /// </summary>
        /// <param name="memberData">會員資料</param>
        /// <param name="appToken">appToken</param>
        /// <param name="Source">來源 : 0: App 1: 電支後台 2:排程</param>
        /// <param name="RealIP">RealIP</param>
        /// <param name="ProxyIP">ProxyIP</param>
        /// <returns></returns>
        public BaseResult CheckOPUnBind(MemberDataModel memberData, MemberAppToken appToken, byte Source, long RealIP = 0, long ProxyIP = 0)
        {
            var result = new BaseResult();
            result.SetError();

            //綁定類型 0: 綁定 1:解綁
            byte Type = 1;
            var basic = memberData.basic;

            if (appToken.MID > 0)
            {
                var bindAppTokenResult = BindAppToken(basic.MID, appToken.AppTokenID, Type);
                if (!bindAppTokenResult.IsSuccess)
                {
                    result.SetError(bindAppTokenResult);
                    return result;
                }
            }

            if ((basic.AuthType & 4) == 4 && string.IsNullOrWhiteSpace(basic.OPMID))
            {
                var bindOPAccountModel = new BindOPAccountModel
                {
                    MID = basic.MID,
                    OPMID = appToken.OPMID,
                    Type = Type,
                    Source = Source
                };

                long NotifyRecord = 0;
                var bindOPAccountResult = BindOPAccount(bindOPAccountModel, ref NotifyRecord, RealIP, ProxyIP);
                if (!bindOPAccountResult.IsSuccess)
                {
                    result.SetError(bindOPAccountResult);
                    return result;
                }

                var bindOPAccountNotifyResult = BindOPAccountNotify(Type, basic.MID, appToken.OPMID, basic.ICPMID, NotifyRecord, RealIP, ProxyIP);
            }

            result.SetSuccess();
            return result;
        }

        /// <summary>
        /// 綁定/解綁 AppToken
        /// </summary>
        /// <param name="MID">會員編號</param>
        /// <param name="AppTokenID">綁定編號</param>
        /// <param name="Type">綁定類型 0: 綁定 1:解綁</param>
        /// <returns></returns>
        public virtual BaseResult BindAppToken(long MID, long AppTokenID, byte Type)
        {
            return _memberAuthRepository.BindAppToken(MID, AppTokenID, Type);
        }

        /// <summary>
        /// 綁定/解綁 OP 帳號
        /// </summary>
        /// <param name="model">資料</param>
        /// <param name="NotifyRecord">待通知記錄</param>
        /// <param name="RealIP">RealIP</param>
        /// <param name="ProxyIP">ProxyIP</param>
        /// <returns></returns>
        public BaseResult BindOPAccount(BindOPAccountModel model, ref long NotifyRecord, long RealIP = 0, long ProxyIP = 0)
        {
            var result = new BaseResult();
            result.SetError();

            long MID = model.MID;

            // 更新 DB 資料 & 新增綁定記錄, 待通知記錄
            var bindResult = _memberAuthRepository.BindOPAccount(model, RealIP, ProxyIP);
            if (!bindResult.IsSuccess)
            {
                return bindResult;
            }

            NotifyRecord = bindResult.RtnData;
            result.SetSuccess();
            return result;
        }

        /// <summary>
        /// 通知 OP 綁定/解綁
        /// </summary>
        /// <param name="Type">通知類型 0: 綁定 1:解綁</param>
        /// <param name="MID">會員編號</param>
        /// <param name="OpMemberID">OP帳號</param>
        /// <param name="IcashAccount">OP帳號</param>
        /// <param name="NotifyRecord">待通知記錄</param>
        /// <param name="RealIP">RealIP</param>
        /// <param name="ProxyIP">ProxyIP</param>
        /// <returns></returns>
        public BaseResult BindOPAccountNotify(byte Type, long MID, string OpMemberID, string IcashAccount, long NotifyRecord, long RealIP = 0, long ProxyIP = 0)
        {
            var result = new BaseResult();
            result.SetError();

            BaseResult notifyResult;

            //綁定通知
            if (Type == 0)
            {

                var bindNotifyRequest = new BindicashAccountRequest
                {
                    IcashAccount = IcashAccount,
                    OpMemberID = OpMemberID
                };

                notifyResult = _oPCustomApiRepository.BindicashAccount(bindNotifyRequest, MID, RealIP, ProxyIP);
            }
            //解綁通知
            else
            {
                var unBindNotifyRequest = new UnBindicashAccountRequest
                {
                    IcashAccount = IcashAccount,
                    OpMemberID = OpMemberID
                };

                notifyResult = _oPCustomApiRepository.UnBindicashAccount(unBindNotifyRequest, MID, RealIP, ProxyIP);
            }

            byte notifyStatus = (byte)(notifyResult.IsSuccess ? 1 : 0);

            //更新通知結果
            _memberInfoRepository.UpdateBindOPAccountNotifyRecord(NotifyRecord, Status: notifyStatus);

            result.SetSuccess();
            return result;
        }

        /// <summary>
        /// 取得身份證換補發縣市清單
        /// </summary>
        /// <returns></returns>
        public List<MemberAuthIssueLocation> ListIssueLocation()
        {
            return _memberAuthRepository.ListIssueLocation();
        }

        /// <summary>
        /// 判斷是否為正常中文姓名
        /// </summary>
        /// <param name="cName"></param>
        /// <returns></returns>
        public BaseResult IsValidateCName(string cName)
        {
            var result = new BaseResult();
            result.RtnMsg = "請輸入2-10個中文字，不可包含英數字及空格，符號僅可接受「•」，不接受三連重複中文字";

            string reg_chinese = RegexConst.ChineseCName;

            if (string.IsNullOrWhiteSpace(cName))
            {
                return result;
            }

            if (cName.Length < 2 || cName.Length > 20)
            {
                return result;
            }

            //符號僅可接受「•」
            if (!Regex.IsMatch(cName, reg_chinese))
            {
                return result;
            }

            //判斷是否為三個連續中文字
            Regex reg = new Regex(@"(.)(\1)+");
            MatchCollection matchs = reg.Matches(cName);
            if (matchs != null && matchs.Cast<Match>().Any(x => x.Value.Length > 2 && Regex.IsMatch(x.Value, reg_chinese)))
            {
                return result;
            }

            result.SetSuccess();
            result.RtnMsg = "";

            return result;
        }
    }
}
