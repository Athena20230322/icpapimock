using ICP.Infrastructure.Core.Helpers;
using ICP.Modules.Api.Member.Models.Certificate;
using ICP.Modules.Api.Member.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICP.Infrastructure.Core.Extensions;
using Newtonsoft.Json.Linq;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Services.MemberApi;
using ICP.Infrastructure.Abstractions.Logging;
using ICP.Library.Models.AuthorizationApi;
using ICP.Infrastructure.Core.Utils;
using ICP.Infrastructure.Core.Models.Consts;

namespace ICP.Modules.Api.Member.Commands
{
    public class CertificateCommand
    {
        private readonly CertificateService _certificateService = null;
        private readonly CertificateMemberApiService _certificateMemberApiService = null;

        public CertificateCommand(
            CertificateService certificateService,
            CertificateMemberApiService certificateMemberApiService)
        {
            _certificateService = certificateService;
            _certificateMemberApiService = certificateMemberApiService;
        }

        public DataResult<RsaCertDTO> GetDefaultPucCert()
        {
            var getDefaultRsaCertIdResult = _certificateService.GetDefaultRsaCertId();

            long defaultCertId = getDefaultRsaCertIdResult.RtnData;

            var getRsaCertResult = _certificateService.GetRsaCert(defaultCertId);

            return getRsaCertResult;
        }

        public DataResult<ExchangePucCertResult> ExchangePucCert(long pubCertID, string signature, string encData)
        {
            var result = new DataResult<ExchangePucCertResult>();
            result.SetError();

            var getCertResult = _certificateService.GetRsaCert(pubCertID);
            if (!getCertResult.IsSuccess)
            {
                result.SetError(getCertResult);
                return result;
            }

            var cert = getCertResult.RtnData;
            var decryptResult = _certificateService.DecryptRsaData<ExchangePucCertRequest>(encData, cert.PrivateCert);
            if (!decryptResult.IsSuccess)
            {
                result.SetError(decryptResult);
                return result;
            }

            var request = decryptResult.RtnData;
            var timeValidResult = _certificateMemberApiService.ValidTimestamp(request.Timestamp);
            if (!timeValidResult.IsSuccess)
            {
                result.SetError(timeValidResult);
                return result;
            }

            var addCertResult = _certificateService.AddClientRsaCert(pubCertID, request.ClientPubCert);
            if (!addCertResult.IsSuccess)
            {
                result.SetError(addCertResult);
                return result;
            }

            var getClientCertResult = _certificateMemberApiService.GetClientRsaCert(addCertResult.RtnData);
            if (!getClientCertResult.IsSuccess)
            {
                result.SetError(getClientCertResult);
                return result;
            }

            var clientCert = getClientCertResult.RtnData;
            result.SetSuccess(new ExchangePucCertResult
            {
                ServerPubCert = clientCert.PublicCert,
                ServerPubCertID = clientCert.ClientCertId,
                ExpireDate = clientCert.ExpireDT.ToString(FormatConst.DateTime)
            });
            return result;
        }

        public DataResult<GenerateAesResult> GenerateAes(long clientCertId, string signature, string encData)
        {
            var result = new DataResult<GenerateAesResult>();
            result.SetError();

            var getClientCertResult = _certificateMemberApiService.GetClientRsaCert(clientCertId);
            if (!getClientCertResult.IsSuccess)
            {
                result.SetError(getClientCertResult);
                return result;
            }

            var clientCert = getClientCertResult.RtnData;
            var decryptResult = _certificateService.DecryptRsaData<GenerateAesRequest>(encData, clientCert.PrivateCert);
            if (!decryptResult.IsSuccess)
            {
                result.SetError(decryptResult);
                return result;
            }

            var request = decryptResult.RtnData;
            var timeValidResult = _certificateMemberApiService.ValidTimestamp(request.Timestamp);
            if (!timeValidResult.IsSuccess)
            {
                result.SetError(timeValidResult);
                return result;
            }

            var signatureValidResult = _certificateMemberApiService.ValidRsaSignature(encData, clientCert.ClientPublicCert, signature);
            if (!signatureValidResult.IsSuccess)
            {
                result.SetError(signatureValidResult);
                return result;
            }

            var addCertResult = _certificateService.AddClientAesCert(clientCertId);
            if (!addCertResult.IsSuccess)
            {
                result.SetError(addCertResult);
                return result;
            }

            long aesCertId = addCertResult.RtnData;
            var getAesCertResult = _certificateMemberApiService.GetClientAesCert(aesCertId);
            if (!getAesCertResult.IsSuccess)
            {
                result.SetError(getAesCertResult);
                return result;
            }

            var aesCert = getAesCertResult.RtnData;
            result.SetSuccess(new GenerateAesResult
            {
                EncKeyID = aesCertId,
                AES_Key = aesCert.AES_Key,
                AES_IV = aesCert.AES_IV,
                ExpireDate = aesCert.ExpireDT.ToString("yyyy/MM/dd HH:mm:ss")
            });
            return result;
        }

        public DataResult<AuthorizationApiResultContext> EncryptApiResult(long clientCertId, BaseResult baseResult, object obj)
        {
            var result = new DataResult<AuthorizationApiResultContext>();
            result.SetError();

            var getClientCertResult = _certificateMemberApiService.GetClientRsaCert(clientCertId);
            if (!getClientCertResult.IsSuccess)
            {
                result.SetError(getClientCertResult);
                return result;
            }

            var clientCert = getClientCertResult.RtnData;
            var encryptDataResult = _certificateService.EncryptClientRsaData(clientCert.ClientPublicCert, obj);
            if (!encryptDataResult.IsSuccess)
            {
                result.SetError(encryptDataResult);
                return result;
            }

            var apiResult = new AuthorizationApiEncryptResult
            {
                RtnCode = baseResult.RtnCode,
                RtnMsg = baseResult.RtnMsg,
                EncData = encryptDataResult.RtnData
            };
            var signResult = _certificateMemberApiService.SignData(clientCert.PrivateCert, apiResult);
            if (!signResult.IsSuccess)
            {
                result.SetError(signResult);
                return result;
            }

            result.SetSuccess(new AuthorizationApiResultContext
            {
                Result = apiResult,
                Signature = signResult.RtnData
            });
            return result;
        }

        public DataResult<BindMerchantCertResult> BindMerchantCert(long clientCertId, BindMerchantCertRequest request)
        {
            var result = new DataResult<BindMerchantCertResult>();
            result.SetError();

            if (!request.IsValid())
            {
                string msg = request.GetFirstErrorMessage();
                result.SetFormatError(msg);
                return result;
            }

            var dbResult = _certificateService.UpdateClientCertFromMerchant(clientCertId, request.MerchantID, request.Token);
            if (!dbResult.IsSuccess)
            {
                result.SetError(dbResult);
                return result;
            }

            result.SetSuccess(new BindMerchantCertResult());
            return result;
        }

        public DataResult<AppRedirectResult> AppRedirect(AppRedirectRequest request)
        {
            var result = new DataResult<AppRedirectResult>();
            result.SetError();

            if (!request.IsValid())
            {
                string msg = request.GetFirstErrorMessage();
                result.SetFormatError(msg);
                return result;
            }

            // 取得 AES Key
            var getClientAesCertResult = _certificateMemberApiService.GetClientAesCert(request.EncKeyID);
            if (!getClientAesCertResult.IsSuccess)
            {
                result.SetError(getClientAesCertResult);
                return result;
            }

            // 解密
            string aesKey = getClientAesCertResult.RtnData.AES_Key;
            string aesIv = getClientAesCertResult.RtnData.AES_IV;
            var decryptClientAesData = _certificateService.DecryptClientAesData(aesKey, aesIv, request.EncData);
            if (!decryptClientAesData.IsSuccess)
            {
                result.SetError(decryptClientAesData);
                return result;
            }

            // 取得 RSA Key
            long reaClientCertId = getClientAesCertResult.RtnData.RSAKeyClientCertId;
            var getClientRsaCertResult = _certificateMemberApiService.GetClientRsaCert(reaClientCertId);
            if (!getClientRsaCertResult.IsSuccess)
            {
                result.SetError(getClientRsaCertResult);
                return result;
            }

            // 驗簽章
            string content = decryptClientAesData.RtnData;
            string rsaClientPublicKey = getClientRsaCertResult.RtnData.ClientPublicCert;
            var validRsaSignatureResult = _certificateMemberApiService.ValidRsaSignature(request.EncData, rsaClientPublicKey, request.Signature);
            if (!validRsaSignatureResult.IsSuccess)
            {
                result.SetError(validRsaSignatureResult);
                return result;
            }

            // 驗 MID
            var validClientCertMIDResult = _certificateMemberApiService.ValidClientCertMID(getClientAesCertResult.RtnData, getClientRsaCertResult.RtnData);
            if (!validClientCertMIDResult.IsSuccess)
            {
                result.SetError(validClientCertMIDResult);
                return result;
            }

            // 反序列化
            var parseAppRedirectDeCryptoDTOResult = _certificateService.ParseAppRedirectDeCryptoDTO(content);
            if (!parseAppRedirectDeCryptoDTOResult.IsSuccess)
            {
                result.SetError(parseAppRedirectDeCryptoDTOResult);
                return result;
            }

            // 驗時間
            var dto = parseAppRedirectDeCryptoDTOResult.RtnData;
            var validTimestampResult = _certificateMemberApiService.ValidTimestamp(dto.Timestamp);
            if (!validTimestampResult.IsSuccess)
            {
                result.SetError(validTimestampResult);
                return result;
            }

            result.SetSuccess(new AppRedirectResult
            {
                MID = getClientAesCertResult.RtnData.MID,
                Url = dto.Url
            });
            return result;
        }
    }
}
