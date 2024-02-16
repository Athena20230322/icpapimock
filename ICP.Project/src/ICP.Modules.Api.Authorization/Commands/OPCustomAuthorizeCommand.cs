using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Linq;

namespace ICP.Modules.Api.Authorization.Commands
{
    using Infrastructure.Core.Extensions;
    using Infrastructure.Core.Models;
    using Infrastructure.Core.Web.Extensions;
    using Library.Models.OpenWalletApi.Enums;
    using Library.Models.OpenWalletApi.CustomReceiveApi;
    using Library.Models.Enums;
    using Library.Repositories.MemberRepositories;
    using Models;
    using Services;

    public class OPCustomAuthorizeCommand
    {
        private readonly OPCustomAuthorizeService _opApiAuthorizeService = null;
        private readonly MemberConfigRepository _configRepository = null;
        private readonly MemberInfoRepository _memberInfoRepository = null;

        private string _AESKey;
        private string AESKey
        {
            get
            {
                if (_AESKey == null)
                {
                    _AESKey = string.Empty + _configRepository.CustomOpenWalletAESKey;
                }
                return _AESKey;
            }
        }

        private string _AESIV;
        private string AESIV
        {
            get
            {
                if (_AESIV == null)
                {
                    _AESIV = string.Empty + _configRepository.CustomOpenWalletAESIV;
                }
                return _AESIV;
            }
        }

        private long RealIP;
        private long ProxyIP;

        private long? MID = null;

        private CustomApiMethodType MethodType;

        public OPCustomAuthorizeCommand(
            OPCustomAuthorizeService opApiAuthorizeService,
            MemberConfigRepository configRepository,
            MemberInfoRepository memberInfoRepository
            )
        {
            _opApiAuthorizeService = opApiAuthorizeService;
            _configRepository = configRepository;
            _memberInfoRepository = memberInfoRepository;
            MethodType = CustomApiMethodType.None;
        }

        /// <summary>
        /// 驗證 API 請求
        /// </summary>
        /// <param name="httpRequest">Http 請求</param>
        /// <param name="MethodName">API Action Name</param>
        /// <param name="targetType">API Request Model Type</param>
        /// <returns></returns>
        public DataResult<BaseCustomReceiveApiRequest> ProcessRequest(HttpRequestBase httpRequest, string MethodName, Type targetType, ref string decryptData)
        {
            var result = new DataResult<BaseCustomReceiveApiRequest>();
            result.SetError();

            RealIP = httpRequest.RealIP();
            ProxyIP = httpRequest.ProxyIP();

            // 密文
            string encData = httpRequest.Params["v"];

            // API 方法轉 Type
            var methodTypeResult = _opApiAuthorizeService.MethodNameToType(MethodName);
            if (!methodTypeResult.IsSuccess)
            {
                result.SetError(methodTypeResult);
                return result;
            }

            MethodType = methodTypeResult.RtnData;

            // 解密密文
            var decryptResult = _opApiAuthorizeService.DecryptClientAesData<BaseCustomReceiveApiRequest>(AESKey, AESIV, encData, ref decryptData);
            if (!decryptResult.IsSuccess)
            {
                result.SetError(decryptResult);
                return result;
            }

            // 記錄 API 輸入內容
            _opApiAuthorizeService.AddCustomAPILog(TransType.Receive, MethodType, decryptData, MID: MID, RealIP: RealIP, ProxyIP: ProxyIP);

            if (targetType != null)
            {
                // 還原 model
                var parseResult = ParseRequestModel(targetType, decryptData);
                if (!parseResult.IsSuccess)
                {
                    result.SetError(parseResult);
                    return result;
                }

                // 取得雜湊碼
                var jOb = JObject.Parse(decryptData);
                string Mask = (string)jOb["mask"];

                // 驗證 Mask
                var validMaskResult = _opApiAuthorizeService.ValidMask(MethodType, parseResult.RtnData, Mask);
                if (!validMaskResult.IsSuccess)
                {
                    result.SetError(validMaskResult);
                    return result;
                }

                // 驗證 TimeSpan
                string TimeSpan = (string)jOb["TimeSpan"];
                _opApiAuthorizeService.ValidTimestamp(TimeSpan, minutes: 3);

                // 取得 OPMID
                string OPMID = (string)jOb["mid"];
                if (!string.IsNullOrEmpty(OPMID))
                {
                    // 取得 MID
                    var appToken = _memberInfoRepository.GetMemberAppToken(OPMID);
                    if (appToken != null && appToken.MID > 0)
                    {
                        MID = appToken.MID;
                    }
                }
            }

            var request = decryptResult.RtnData;
            result.SetSuccess(request);
            return result;
        }

        /// <summary>
        /// 還原 Model
        /// </summary>
        /// <param name="targetType">型別</param>
        /// <param name="decryptData">資料</param>
        /// <returns></returns>
        public DataResult<object> ParseRequestModel(Type targetType, string decryptData)
        {
            return _opApiAuthorizeService.ParseRequestModel(targetType, decryptData);
        }

        /// <summary>
        /// 記錄 API 輸出內容
        /// </summary>
        /// <param name="result"></param>
        public DataResult<string> ProcessResponse(BaseResult baseResult = null, BaseCustomReceiveApiResult rtnData = null)
        {
            var result = new DataResult<string>();

            if (rtnData == null)
            {
                rtnData = new BaseCustomReceiveApiResult();
            }

            if (string.IsNullOrWhiteSpace(rtnData.Code) && baseResult != null)
            {
                if (baseResult.IsSuccess)
                {
                    rtnData.Code = "00";
                }
                else
                {
                    rtnData.Code = baseResult.RtnCode.ToString().PadLeft(2, '0');
                }
            }

            //var encryResult = _opApiAuthorizeService.EncryptClientAesData(AESKey, AESIV, rtnData);
            //if (!encryResult.IsSuccess)
            //{
            //    result.SetError(encryResult);
            //    return result;
            //}
            //result.SetSuccess(encryResult.RtnData);

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(rtnData);
            _opApiAuthorizeService.AddCustomAPILog(TransType.Send, MethodType, json, MID: MID, StatusCode: rtnData.Code, RealIP: ProxyIP, ProxyIP: ProxyIP);

            result.SetSuccess(json);
            return result;
        }
    }
}
