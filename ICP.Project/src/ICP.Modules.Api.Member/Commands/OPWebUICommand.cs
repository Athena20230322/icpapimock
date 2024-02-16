using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Helpers;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.MemberModels;
using ICP.Library.Models.OpenWalletApi.WebUIApi;
using ICP.Library.Repositories.MemberRepositories;
using ICP.Library.Services.MemberServices;
using ICP.Library.Services.OpenWalletApi;
using ICP.Modules.Api.Member.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Commands
{
    /// <summary>
    /// Web UI Api
    /// </summary>
    public class OPWebUICommand
    {
        OPWebUIApiService _oPWebUIApiService;
        OPWebUIService _oPWebUIService;
        LibMemberLoginService _libMemberLoginService;
        LibMemberTeenagersService  _memberTeenagersService;
        LibMemberInfoService _libMemberInfoService;

        public OPWebUICommand(
            OPWebUIApiService oPWebUIApiService,
            OPWebUIService oPWebUIService,
            LibMemberLoginService libMemberLoginService,
            LibMemberTeenagersService memberTeenagersService,
            LibMemberInfoService libMemberInfoService
            )
        {
            _oPWebUIApiService = oPWebUIApiService;
            _oPWebUIService = oPWebUIService;
            _libMemberLoginService = libMemberLoginService;
            _memberTeenagersService = memberTeenagersService;
            _libMemberInfoService = libMemberInfoService;
        }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="request">登入資訊</param>
        /// <param name="RealIP">RealIP</param>
        /// <param name="ProxyIP">ProxyIP</param>
        /// <returns></returns>
        public DataResult<LoginWebUIResult> Login(LoginWebUIRequest request, long RealIP, long ProxyIP)
        {
            var result = new DataResult<LoginWebUIResult>();
            result.SetError();

            // 檢查是否為未成年代理人
            var checkResult = _memberTeenagersService.CheckTeenagersLegalDetail(request.LoginID);
            if (!checkResult.IsSuccess)
            {
                result.SetError(checkResult);
                return result;
            }

            long CheckMID = 0;
            var getMIDResult = _libMemberInfoService.GetMID(Account: request.LoginID);
            if (getMIDResult.IsSuccess)
            {
                CheckMID = getMIDResult.RtnData;
            }

            // 實際登入
            var loginResult = _libMemberLoginService.UserCodeLogin(request.LoginID, request.PassWord, LoginType: 3, RealIP: RealIP, ProxyIP: ProxyIP, checkMID: CheckMID);
            if (!loginResult.IsSuccess)
            {
                result.SetError(loginResult);
                return result;
            }

            // 更新 Token
            long MID = loginResult.RtnData;
            var tokenResult = _oPWebUIApiService.UpdateOPWebTokenExpired(MID, RealIP, ProxyIP);
            if (!tokenResult.IsSuccess)
            {
                result.SetError(tokenResult);
                return result;
            }

            // 取得 Token
            var getTokenResult = _oPWebUIApiService.GetOPWebToken(MID);
            if (!getTokenResult.IsSuccess)
            {
                result.SetError(getTokenResult);
                return result;
            }

            // 回傳資料
            var tokenData = getTokenResult.RtnData;
            var rtnData = new LoginWebUIResult();
            rtnData.Token = tokenData.OPAccessToken;
            rtnData.TokenDate = tokenData.OPExpired.ToString("yyyy/MM/dd HH:mm:ss");

            result.SetSuccess(rtnData);
            return result;
        }

        /// <summary>
        /// 查詢未成年身分資料
        /// </summary>
        /// <param name="MID"></param>
        /// <returns></returns>
        public DataResult<GetUserDataWebUIResult> GetUserData(long MID)
        {
            var result = new DataResult<GetUserDataWebUIResult>();
            result.SetError();

            var listResult = _memberTeenagersService.ListTeenagersLegalDetail(MID);
            if (!listResult.IsSuccess)
            {
                result.SetError(listResult);
                return result;
            }

            var rtnData = new GetUserDataWebUIResult();
            rtnData.UserData = _oPWebUIService.TeenagersLegalDetail_To_UserData(listResult.RtnData);

            result.SetSuccess(rtnData);
            return result;
        }

        /// <summary>
        /// 同意未成年註冊
        /// </summary>
        /// <param name="MID">會員編號</param>
        /// <param name="request">同意資料</param>
        /// <param name="RealIP">RealIP</param>
        /// <param name="ProxyIP">ProxyIP</param>
        /// <returns></returns>
        public DataResult<BaseWebUIApiResult> AgreeRegister(long MID, AgreeRegisterWebUIRequest request, string urlDir, string saveDir, long RealIP, long ProxyIP)
        {
            var result = new DataResult<BaseWebUIApiResult>();
            result.SetError();

            // 取得未成年 MID
            var getMIDResult = _libMemberInfoService.GetMID(ICPMID: request.MID);
            if (!getMIDResult.IsSuccess)
            {
                result.SetError(getMIDResult);
                return result;
            }
            long TeenagersMID = getMIDResult.RtnData;

            // 取得待同意資料
            var listResult = _memberTeenagersService.ListTeenagersLegalDetail(MID);
            if (!listResult.IsSuccess)
            {
                result.SetError(listResult);
                return result;
            }

            // 檢查 未成年 MID 是否在待同意資料中
            var checkResult = _oPWebUIService.CheckTeenagersLegalDetail(TeenagersMID, listResult.RtnData);
            if (!checkResult.IsSuccess)
            {
                result.SetError(checkResult);
                return result;
            }

            // 儲存上傳檔案
            var detail = checkResult.RtnData;
            var saveImgResult = _oPWebUIService.SaveAgreeRegisterImages(ref detail, request, urlDir, saveDir);
            if (!saveImgResult.IsSuccess)
            {
                result.SetError(saveImgResult);
                return result;
            }

            // 更新同意
            var updateResult = _oPWebUIService.UpdateTeenagersLegalAgree(detail, RealIP, ProxyIP);
            if (!updateResult.IsSuccess)
            {
                result.SetError(updateResult);
                return result;
            }

            result.SetSuccess();
            return result;
        }
    }
}