using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Helpers;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Api.Member.Models.Certificate;
using ICP.Modules.Api.Member.Repositories;
using Newtonsoft.Json;
using System;

namespace ICP.Modules.Api.Member.Services
{
    public class CertificateService
    {
        private readonly CertificateRepository _certificateRepository = null;
        private readonly ILogger _logger = null;

        public CertificateService(
            CertificateRepository certificateRepository,
            ILogger<CertificateService> logger)
        {
            _certificateRepository = certificateRepository;
            _logger = logger;
        }

        public DataResult<RsaCertDTO> GetRsaCert(long certId)
        {
            var result = new DataResult<RsaCertDTO>();

            _logger.Trace($"準備取得 Rsa 金鑰， {nameof(certId)} = {certId}");
            var cert = _certificateRepository.GetRsaCert(certId);
            if (cert == null)
            {
                result.SetCode(1021);
            }
            else if (cert.IsExpired)
            {
                result.SetCode(1022);
            }
            else
            {
                result.SetSuccess(cert);
            }

            if (result.IsSuccess)
            {
                _logger.Trace($"取得 Rsa 金鑰成功");
            }
            else
            {
                _logger.Warning($"取得 Rsa 金鑰失敗，原因 = {result.RtnMsg}");
            }

            return result;
        }

        public DataResult<T> DecryptRsaData<T>(string encData, string privateKey)
        {
            var result = new DataResult<T>();

            RsaCryptoHelper rsaCryptoHelper = new RsaCryptoHelper();

            try
            {
                _logger.Trace($"準備載入 PrivateKey，長度 = {privateKey?.Length}");
                rsaCryptoHelper.ImportPemPrivateKey(privateKey);

                _logger.Trace($"準備解密 {nameof(encData)}，長度 = {encData?.Length}");
                string json = rsaCryptoHelper.Decrypt(encData);
                T data = json.TryParseJsonToObj(out T obj) ? obj : default(T);

                result.SetSuccess(data);
                _logger.Trace($"解密 {nameof(encData)} 成功");
            }
            catch (Exception ex)
            {
                result.SetCode(1023);
                _logger.Warning(ex, result.RtnMsg);
            }

            return result;
        }

        public DataResult<long> GetDefaultRsaCertId()
        {
            _logger.Trace($"準備取得預設 Rsa 金鑰編號");

            var result = _certificateRepository.GetDefaultRsaCertId();
            if (result.IsSuccess)
            {
                _logger.Trace($"取得預設 Rsa 金鑰編號成功");
            }
            else
            {
                _logger.Warning($"取得預設 Rsa 金鑰編號失敗，原因 = {result.RtnMsg}");
            }

            return result;
        }

        public DataResult<long> AddClientRsaCert(long certId, string clientPubCert)
        {
            RsaCryptoHelper rsaCryptoHelper = new RsaCryptoHelper();

            var newRsaKey = rsaCryptoHelper.GeneratePemKey();

            var dto = new AddClientRsaCertDTO
            {
                CertId = certId,
                ClientPublicCert = clientPubCert,
                PublicCert = newRsaKey.PublicKey,
                PrivateCert = newRsaKey.PrivateKey
            };

            _logger.Trace($"準備新增 ClientRsaCert 至 {nameof(certId)} = {certId}");
            var addResult = _certificateRepository.AddClientRsaCert(dto);
            if (addResult.IsSuccess)
            {
                _logger.Trace($"新增 ClientRsaCert 成功");
            }
            else
            {
                _logger.Warning($"新增 ClientRsaCert 失敗，原因 = {addResult.RtnMsg}");
            }

            return addResult;
        }

        public DataResult<long> AddClientAesCert(long clientCertId)
        {
            AesCryptoHelper aesCryptoHelper = new AesCryptoHelper();

            var newAesKey = aesCryptoHelper.GenerateKey();

            var dto = new AddClientAesCertDTO
            {
                RSAKeyClientCertId = clientCertId,
                AES_Key = newAesKey.Key,
                AES_IV = newAesKey.Iv
            };

            _logger.Trace($"準備新增 ClientAesCert 至 {nameof(clientCertId)} = {clientCertId}");
            var addResult = _certificateRepository.AddClientAesCert(dto);
            if (addResult.IsSuccess)
            {
                _logger.Trace($"新增 ClientAesCert 成功");
            }
            else
            {
                _logger.Warning($"新增 ClientAesCert 失敗，原因 = {addResult.RtnMsg}");
            }

            return addResult;
        }

        public DataResult<string> EncryptClientRsaData(string clientPublicKey, object obj)
        {
            RsaCryptoHelper rsaCryptoHelper = new RsaCryptoHelper();

            var result = new DataResult<string>();
            result.SetError();

            try
            {
                _logger.Trace($"準備載入 ClientPublicKey，長度 = {clientPublicKey?.Length}");
                rsaCryptoHelper.ImportPemPublicKey(clientPublicKey);

                string json = JsonConvert.SerializeObject(obj);

                _logger.Trace($"準備加密 {nameof(json)}，長度 = {json?.Length}");
                string encData = rsaCryptoHelper.Encrypt(json);

                result.SetSuccess(encData);
                _logger.Trace($"加密 {nameof(json)} 成功");
            }
            catch (Exception ex)
            {
                result.SetCode(1025);
                _logger.Warning(ex, result.RtnMsg);
            }

            return result;
        }

        public DataResult<long> UpdateClientAesCertExpireDT(long clientCertId)
        {
            _logger.Trace($"準備更新 ClientAesCert 過期時間，{nameof(clientCertId)} = {clientCertId}");
            var dbResult = _certificateRepository.UpdateClientAesCertExpireDT(clientCertId);
            if (dbResult.IsSuccess)
            {
                _logger.Trace($"更新 ClientAesCert 過期時間成功");
            }
            else
            {
                _logger.Warning($"更新 ClientAesCert 過期時間失敗，原因 = {dbResult.RtnMsg}");
            }

            return dbResult;
        }

        public BaseResult UpdateClientCertFromMerchant(long clientCertId, long merchantID, string token)
        {
            _logger.Trace($"準備綁定 MerchantId，{nameof(clientCertId)} = {clientCertId}，{nameof(merchantID)} = {merchantID}");
            var dbResult = _certificateRepository.UpdateClientCertFromMerchant(clientCertId, merchantID, token);
            if (dbResult.IsSuccess)
            {
                _logger.Trace($"綁定 MerchantId 成功");
            }
            else
            {
                _logger.Warning($"綁定 MerchantId 失敗，原因 = {dbResult.RtnMsg}");
            }

            return dbResult;
        }

        public BaseResult UpdateClientCertFromMember(long ClientCertId, long MID)
        {
            _logger.Trace($"準備綁定 MID，{nameof(ClientCertId)} = {ClientCertId}，{nameof(MID)} = {MID}");
            var dbResult = _certificateRepository.UpdateClientCertFromMember(ClientCertId, MID);
            if (dbResult.IsSuccess)
            {
                _logger.Trace($"綁定 MID 成功");
            }
            else
            {
                _logger.Warning($"綁定 MID 失敗，原因 = {dbResult.RtnMsg}");
            }

            return dbResult;
        }

        public DataResult<string> DecryptClientAesData(string key, string iv, string encData)
        {
            var result = new DataResult<string>();

            AesCryptoHelper aesCryptoHelper = new AesCryptoHelper
            {
                Key = key,
                Iv = iv
            };

            try
            {
                _logger.Trace($"準備解密 {nameof(encData)}，長度 = {encData?.Length}");
                string json = aesCryptoHelper.Decrypt(encData);

                result.SetSuccess(json);
                _logger.Trace($"解密 {nameof(encData)} 成功");
            }
            catch (Exception ex)
            {
                result.SetCode(10026);
                _logger.Warning(ex, result.RtnMsg);
            }

            return result;
        }

        public DataResult<AppRedirectDeCryptoDTO> ParseAppRedirectDeCryptoDTO(string json)
        {
            var result = new DataResult<AppRedirectDeCryptoDTO>();

            try
            {
                _logger.Trace($"準備解密 {nameof(json)}，長度 = {json?.Length}");
                var dto = JsonConvert.DeserializeObject<AppRedirectDeCryptoDTO>(json);

                result.SetSuccess(dto);
                _logger.Trace($"解密 {nameof(json)} 成功");
            }
            catch (Exception ex)
            {
                result.SetCode(10027);
                _logger.Warning(ex, result.RtnMsg);
            }

            return result;
        }
    }
}
