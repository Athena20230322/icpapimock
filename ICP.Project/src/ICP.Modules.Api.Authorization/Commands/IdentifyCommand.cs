using System;
using System.Web;

namespace ICP.Modules.Api.Authorization.Commands
{
    using Infrastructure.Core.Extensions;
    using Infrastructure.Core.Models;
    using Library.Models.AuthorizationApi;
    using Library.Services.MemberApi;
    using Library.Services.MemberServices;
    using Models;
    using Services;

    public class IdentifyCommand
    {
        private readonly IdentifyService _identifyService = null;
        private readonly CertificateMemberApiService _certificateMemberApiService = null;
        private readonly OPMemberApiService _oPMemberApiService = null;
        private readonly LibMemberInfoService _libMemberInfoService = null;

        public IdentifyCommand(
            IdentifyService identifyService,
            CertificateMemberApiService certificateMemberApiService,
            OPMemberApiService oPMemberApiService,
            LibMemberInfoService libMemberInfoService
            )
        {
            _identifyService = identifyService;
            _certificateMemberApiService = certificateMemberApiService;
            _oPMemberApiService = oPMemberApiService;
            _libMemberInfoService = libMemberInfoService;
        }

        public DataResult<ICPRequestContext> ProcessRequest(HttpRequestBase httpRequest, bool allowAnonymous, bool allowOPAnonymous)
        {
            var result = new DataResult<ICPRequestContext>();
            result.SetError();

            // 驗證 Header
            var validHeaderResult = _identifyService.ValidHeaders(httpRequest.Headers);
            if (!validHeaderResult.IsSuccess)
            {
                result.SetError(validHeaderResult);
                return result;
            }

            // 取得 Aes 金鑰(不檢查期限)
            var baseCertificateHeader = validHeaderResult.RtnData;
            var getClientAesCertResult = _certificateMemberApiService.GetClientAesCert(baseCertificateHeader.EncKeyID, checkExpired: false);
            if (!getClientAesCertResult.IsSuccess)
            {
                result.SetError(getClientAesCertResult);
                return result;
            }

            // 取得 Rsa 金鑰(優先檢查 Rsa 期限)
            var clientAesCert = getClientAesCertResult.RtnData;
            var getClientCertResult = _certificateMemberApiService.GetClientRsaCert(clientAesCert.RSAKeyClientCertId);
            if (!getClientCertResult.IsSuccess)
            {
                result.SetError(getClientCertResult);
                return result;
            }

            //檢查 Aes 金鑰期限
            var checkAesCertResult = _certificateMemberApiService.CheckAesCertExpired(clientAesCert);
            if (!checkAesCertResult.IsSuccess)
            {
                result.SetError(checkAesCertResult);
                return result;
            }

            // 驗證金鑰是否已綁訂
            var clientRsaCert = getClientCertResult.RtnData;
            if (!allowAnonymous)
            {
                var validClientCertMIDResult = _certificateMemberApiService.ValidClientCertMID(clientAesCert, clientRsaCert);
                if (!validClientCertMIDResult.IsSuccess)
                {
                    result.SetError(validClientCertMIDResult);
                    return result;
                }

                var memberStatus = _libMemberInfoService.GetMemberStatus(clientAesCert.MID);
                if (memberStatus == 0)
                {
                    result.SetError(new BaseResult
                    {
                        RtnCode = 200026,
                        RtnMsg = "您已從其他裝置登入或帳號發生問題請洽客服"
                    });

                    return result;
                }
            }

            // 驗證簽章
            string encData = httpRequest.Params["EncData"];
            var signatureValidResult = _certificateMemberApiService.ValidRsaSignature(encData, clientRsaCert.ClientPublicCert, baseCertificateHeader.Signature);
            if (!signatureValidResult.IsSuccess)
            {
                result.SetError(signatureValidResult);
                return result;
            }

            // 解密密文
            string DecryptData = null;
            var decryptResult = _identifyService.DecryptClientAesData<ICPRequestContext>(clientAesCert.AES_Key, clientAesCert.AES_IV, encData, ref DecryptData);
            if (!decryptResult.IsSuccess)
            {
                result.SetError(decryptResult);
                return result;
            }

            if (!allowAnonymous)
            {
                var memberDeviceStatus=_libMemberInfoService.CheckMemberDeviceStatus(decryptResult.RtnData.DeviceID);
                if (memberDeviceStatus.IsSuccess)
                {
                    result.SetError(new BaseResult
                    {
                        RtnCode = 200026,
                        RtnMsg = "您已從其他裝置登入或帳號發生問題請洽客服"
                    });

                    return result;
                }
            }

            // 驗證密文 Timestamp
            var request = decryptResult.RtnData;
            var timeValidResult = _certificateMemberApiService.ValidTimestamp(request.Timestamp);
            if (!timeValidResult.IsSuccess)
            {
                result.SetError(timeValidResult);
                return result;
            }

            var checkAppTokenResult = _oPMemberApiService.CheckAppToken(request.LoginTokenID, clientAesCert.MID, allowOPAnonymous);
            if (!checkAppTokenResult.IsSuccess)
            {
                result.SetError(checkAppTokenResult);
                return result;
            }
            var appToken = checkAppTokenResult.RtnData;
            if (appToken != null)
            {
                request.OPMID = appToken.OPMID;
                request.AppTokenID = appToken.AppTokenID;
            }

            request.DecryptData = DecryptData;
            request.EncryptData = encData;
            request.KeyContext = new AuthorizationApiKeyContext
            {
                ClientRsaCert = clientRsaCert,
                ClientAesCert = clientAesCert
            };

            result.SetSuccess(request);
            return result;
        }

        public DataResult<object> ParseRequestModel(Type targetType, string decryptData)
        {
            return _identifyService.ParseRequestModel(targetType, decryptData);
        }

        public DataResult<AuthorizationApiResultContext> ProcessResponse(AuthorizationApiKeyContext keyContext, DataResult dataResult)
        {
            var result = new DataResult<AuthorizationApiResultContext>();
            result.SetError();

            var encryptResult = _identifyService.EncryptClientAesData(keyContext.ClientAesCert.AES_Key,
                                                                      keyContext.ClientAesCert.AES_IV,
                                                                      dataResult.RtnData);
            if (!encryptResult.IsSuccess)
            {
                result.SetError(encryptResult);
                return result;
            }

            dataResult.RtnData = encryptResult.RtnData;

            var signDataResult = _certificateMemberApiService.SignData(keyContext.ClientRsaCert.PrivateCert, dataResult);
            if (!signDataResult.IsSuccess)
            {
                result.SetError(signDataResult);
                return result;
            }

            result.SetSuccess(new AuthorizationApiResultContext
            {
                Result = dataResult,
                Signature = signDataResult.RtnData
            });
            return result;
        }
    }
}