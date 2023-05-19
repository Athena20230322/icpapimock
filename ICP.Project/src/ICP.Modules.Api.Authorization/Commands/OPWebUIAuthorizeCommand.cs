using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.OpenWalletApi.Enums;
using ICP.Library.Models.OpenWalletApi.WebUIApi;
using ICP.Library.Repositories.MemberRepositories;
using ICP.Library.Services.OpenWalletApi;
using ICP.Modules.Api.Authorization.Models;
using ICP.Modules.Api.Authorization.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ICP.Modules.Api.Authorization.Commands
{
    public class OPWebUIAuthorizeCommand
    {
        MemberConfigRepository _configRepository = null;
        OPWebUIAuthorizeService _oPWebUIAuthorizeService = null;
        OPWebUIApiService _oPWebUIApiService = null;

        private string _AESKey;
        private string AESKey
        {
            get
            {
                if (_AESKey == null)
                {
                    _AESKey = string.Empty + _configRepository.OPWebUIApiAESKey;
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
                    _AESIV = string.Empty + _configRepository.OPWebUIApiAESIV;
                }
                return _AESIV;
            }
        }

        public OPWebUIAuthorizeCommand(
            MemberConfigRepository configRepository,
            OPWebUIAuthorizeService oPWebUIAuthorizeService,
            OPWebUIApiService oPWebUIApiService
        )
        {
            _configRepository = configRepository;
            _oPWebUIAuthorizeService = oPWebUIAuthorizeService;
            _oPWebUIApiService = oPWebUIApiService;
        }

        public DataResult<BaseAuthWebUIApiRequest> ProcessRequest(HttpRequestBase httpRequest, bool allowAnonymous, string MethodName, Type targetType, ref string decryptData, ref long MID)
        {
            var result = new DataResult<BaseAuthWebUIApiRequest>();
            result.SetError();

            string encData = httpRequest.Params["EncData"];

            // 解密密文
            var decryptResult = _oPWebUIAuthorizeService.DecryptClientAesData<BaseAuthWebUIApiRequest>(AESKey, AESIV, encData, ref decryptData);
            if (!decryptResult.IsSuccess)
            {
                result.SetError(decryptResult);
                return result;
            }

            // 驗證 TimeSpan
            var request = decryptResult.RtnData;
            string TimeStamp = request.TimeStamp;
            var timeValidResult = _oPWebUIAuthorizeService.ValidTimestamp(TimeStamp, minutes: 3);
            if (!timeValidResult.IsSuccess)
            {
                result.SetError(timeValidResult);
                return result;
            }

            // API 方法轉 Type
            var methodTypeResult = _oPWebUIAuthorizeService.MethodNameToType(MethodName);
            if (!methodTypeResult.IsSuccess)
            {
                result.SetError(methodTypeResult);
                return result;
            }

            var MethodType = methodTypeResult.RtnData;

            // 序列化還原
            var parseResult = ParseRequestModel(targetType, decryptData);
            if (!parseResult.IsSuccess)
            {
                result.SetError(parseResult);
                return result;
            }

            // 驗證 Mask
            var jOb = JObject.Parse(decryptData);
            string Mask = (string)jOb["Mask"];
            var validMaskResult = _oPWebUIAuthorizeService.ValidMask(MethodType, parseResult.RtnData, Mask);
            if (!validMaskResult.IsSuccess)
            {
                result.SetError(validMaskResult);
                return result;
            }

            MID = 0;

            // 驗證登入Token
            if (!allowAnonymous)
            {
                string Token = (string)jOb["Token"];
                var checkTokenResult = _oPWebUIApiService.CheckOPWebToken(Token);
                if (!checkTokenResult.IsSuccess)
                {
                    result.SetError(checkTokenResult);
                    return result;
                }

                MID = checkTokenResult.RtnData;
            }

            result.SetSuccess(request);
            return result;
        }

        public DataResult<object> ParseRequestModel(Type targetType, string decryptData)
        {
            return _oPWebUIAuthorizeService.ParseRequestModel(targetType, decryptData);
        }

        public DataResult<object> ProcessResponse(BaseWebUIApiResult rtnData)
        {
            var result = new DataResult<object>();
            
            var encryResult = _oPWebUIAuthorizeService.EncryptClientAesData(AESKey, AESIV, rtnData);
            if (!encryResult.IsSuccess)
            {
                result.SetError(encryResult);
                return result;
            }

            result.SetSuccess(new
            {
                rtnData.StatusCode,
                rtnData.StatusMessage,
                EncData = encryResult.RtnData
            });
            return result;
        }
    }
}
