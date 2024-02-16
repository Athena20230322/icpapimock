using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Services.MemberApi
{
    using Infrastructure.Core.Models;
    using Infrastructure.Core.Extensions;
    using Models.MemberModels;
    using Repositories.MemberApi;
    using Repositories.MemberRepositories;

    public class OPMemberApiService
    {
        OPMemberApiRepository _oPMemberApiRepository;
        MemberInfoRepository _memberInfoRepository;

        public OPMemberApiService(
            OPMemberApiRepository oPMemberApiRepository,
            MemberInfoRepository memberInfoRepository
            )
        {
            _oPMemberApiRepository = oPMemberApiRepository;
            _memberInfoRepository = memberInfoRepository;
        }

        //public DataResult<string> GetOPMIDByLoginToken(string LoginTokenID)
        //{
        //    if (string.IsNullOrWhiteSpace(LoginTokenID))
        //    {
        //        var errorResult = new DataResult<string>();
        //        errorResult.SetError(new BaseResult { RtnMsg = "LoginTokenID Required" });
        //        return errorResult;
        //    }

        //    return _oPMemberApiRepository.GetOPMIDByLoginToken(LoginTokenID);
        //}

        public DataResult<MemberAppToken> GetAppTokenIDByLoginToken(string LoginTokenID)
        {
            var result = new DataResult<MemberAppToken>();
            result.SetError();

            if (string.IsNullOrWhiteSpace(LoginTokenID))
            {
                return result;
            }

            var rtnData = _oPMemberApiRepository.GetAppTokenByLoginToken(LoginTokenID);
            if (rtnData == null)
            {
                return result;
            }

            result.SetSuccess(rtnData);
            return result;
        }

        public DataResult<MemberAppToken> CheckAppToken(string LoginTokenID, long MID, bool allowOPAnonymous)
        {
            var result = new DataResult<MemberAppToken>();
            result.SetError();

            // 取得 AppTokenID
            MemberAppToken appToken_byToken = null;
            if (!string.IsNullOrWhiteSpace(LoginTokenID))
            {
                var getAppTokenResult = GetAppTokenIDByLoginToken(LoginTokenID);
                if (!getAppTokenResult.IsSuccess)
                {
                    result.SetError(getAppTokenResult);
                    return result;
                }

                appToken_byToken = getAppTokenResult.RtnData;
            }

            // 金鑰 已綁定 MID
            if (MID > 0)
            {
                // 取得 appToken
                var appToken_byMID = _memberInfoRepository.GetMemberAppTokenByMID(MID);
                if (appToken_byMID == null)
                {
                    result.SetError(new BaseResult { RtnMsg = "MID 查無 appToken" });
                    return result;
                }

                // 檢核 LoginToken 取得的 AppTokenID 跟 MID 對應的 AppTokenID 是否一至
                if (appToken_byToken != null && appToken_byToken.AppTokenID != appToken_byMID.AppTokenID)
                {
                    result.SetError(new BaseResult { RtnMsg = "資料異常，請登出" });
                    return result;
                }

                //取得 appToken.OPMID
                appToken_byToken = appToken_byMID;
            }

            if (!allowOPAnonymous && appToken_byToken == null)
            {
                result.SetError(new BaseResult { RtnMsg = "取得 App Token 失敗" });
                return result;
            }

            result.SetSuccess(appToken_byToken);
            return result;
        }
    }
}
