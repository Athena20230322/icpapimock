using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Helpers;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.MemberApi.Certificate;
using ICP.Library.Repositories.MemberApi;
using Newtonsoft.Json;
using System;

namespace ICP.Library.Services.MemberApi
{
    public class CertificateMemberApiService
    {
        private readonly CertificateMemberApiRepository _certificateMemberApiRepository = null;
        private readonly ILogger _logger = null;

        public CertificateMemberApiService(
            CertificateMemberApiRepository certificateMemberApiRepository,
            ILogger<CertificateMemberApiService> logger)
        {
            _certificateMemberApiRepository = certificateMemberApiRepository;
            _logger = logger;
        }

        public DataResult<ClientRsaCertDTO> GetClientRsaCert(long clientCertId)
        {
            var result = new DataResult<ClientRsaCertDTO>();

            _logger.Trace($"準備取得 ClientRsaCert，{nameof(clientCertId)} = {clientCertId}");
            var cert = _certificateMemberApiRepository.GetClientRsaCert(clientCertId);
            if (cert == null)
            {
                result.SetCode(1007);
            }
            else if (cert.IsExpired)
            {
                result.SetCode(1008);
            }
            else
            {
                result.SetSuccess(cert);
            }

            if (result.IsSuccess)
            {
                _logger.Trace($"驗證 ClientRsaCert 成功");
            }
            else
            {
                _logger.Warning($"驗證 ClientRsaCert 失敗，原因 = {result.RtnMsg}");
            }

            return result;
        }

        public DataResult<ClientAesCertDTO> GetClientAesCert(long clientCertId, long rsaKeyClientCertId = 0, bool checkExpired = true)
        {
            var result = new DataResult<ClientAesCertDTO>();

            _logger.Trace($"準備取得 ClientAesCert，{nameof(clientCertId)} = {clientCertId}, {nameof(rsaKeyClientCertId)} = {rsaKeyClientCertId}");
            var cert = _certificateMemberApiRepository.GetClientAesCert(clientCertId, rsaKeyClientCertId);
            if (cert == null)
            {
                result.SetCode(1009);
                _logger.Warning($"驗證 ClientAesCert 失敗，原因 = {result.RtnMsg}");
                return result;
            }
            else if (checkExpired)
            {
                var checkResult = CheckAesCertExpired(cert);
                if (!checkResult.IsSuccess)
                {
                    result.SetError(checkResult);
                    _logger.Warning($"驗證 ClientAesCert 失敗，原因 = {result.RtnMsg}");
                    return result;
                }
            }

            result.SetSuccess(cert);
            _logger.Trace($"驗證 ClientAesCert 成功");
            return result;
        }

        public BaseResult CheckAesCertExpired(ClientAesCertDTO cert)
        {
            var result = new BaseResult();
            result.SetError();

            if (cert.IsExpired)
            {
                result.SetCode(1010);
                return result;
            }

            result.SetSuccess();
            return result;
        }

        public BaseResult ValidRsaSignature(string content, string publicKey, string signature)
        {
            var result = new BaseResult();
            result.SetError();

            RsaCryptoHelper rsaCryptoHelper = new RsaCryptoHelper();

            try
            {
                _logger.Trace($"準備載入 PublicKey，長度 = {publicKey?.Length}");
                rsaCryptoHelper.ImportPemPublicKey(publicKey);

                _logger.Trace($"準備驗證 {nameof(signature)}，長度 = {signature?.Length}");
                bool isValid = rsaCryptoHelper.VerifySignDataWithSha256(content, signature);
                if (!isValid)
                {
                    throw new Exception("簽章驗證失敗");
                }

                result.SetSuccess();
                _logger.Trace($"驗證 {nameof(signature)} 成功");
            }
            catch (Exception ex)
            {
                result.SetCode(1011);
                _logger.Warning(ex, result.RtnMsg);
            }

            return result;
        }

        public BaseResult ValidTimestamp(string timestamp)
        {
            var result = new BaseResult();
            result.SetError();

            _logger.Trace($"準備驗證 Timestamp，{nameof(timestamp)} = {timestamp}");
            if (!DateTime.TryParse(timestamp, out DateTime dt))
            {
                result.SetCode(1012);
            }
            else if (DateTime.Now.Subtract(dt) > TimeSpan.FromMinutes(3))
            {
                result.SetCode(1013);
            }
            else
            {
                result.SetSuccess();
            }

            if (result.IsSuccess)
            {
                _logger.Trace($"驗證 Timestamp 成功");
            }
            else
            {
                _logger.Warning($"驗證 Timestamp 失敗，原因 = {result.RtnMsg}");
            }

            return result;
        }

        public DataResult<string> SignData(string privateKey, object obj)
        {
            var result = new DataResult<string>();

            RsaCryptoHelper rsaCryptoHelper = new RsaCryptoHelper();

            try
            {
                _logger.Trace($"準備載入 PrivateKey，長度 = {privateKey?.Length}");
                rsaCryptoHelper.ImportPemPrivateKey(privateKey);

                string json = JsonConvert.SerializeObject(obj);

                _logger.Trace($"準備簽章 {nameof(json)}，長度 = {json?.Length}");
                string encData = rsaCryptoHelper.SignDataWithSha256(json);

                result.SetSuccess(encData);
                _logger.Trace($"簽章 {nameof(json)} 成功");
            }
            catch (Exception ex)
            {
                result.SetCode(1014);
                _logger.Warning(ex, result.RtnMsg);
            }

            return result;
        }
        
        public BaseResult ValidClientCertMID(ClientAesCertDTO clientAesCert, ClientRsaCertDTO clientRsaCert)
        {
            var result = new BaseResult();

            _logger.Trace($"準備驗證金鑰是否綁訂 MID， {nameof(ClientAesCertDTO)}.MID = {clientAesCert.MID}，{nameof(ClientRsaCertDTO)}.MID = {clientRsaCert.MID}");
            if (clientAesCert.MID == 0 || clientRsaCert.MID == 0)
            {
                result.SetCode(10000);
            }
            else if (clientAesCert.MID != clientRsaCert.MID)
            {
                result.SetCode(10001);
            }
            else
            {
                result.SetSuccess();
            }

            _logger.Trace($"驗證金鑰是否綁訂 MID，{result.RtnMsg}");

            return result;
        }

        public BaseResult UpdateClientCertExpired(long ClientCertId, long MID)
        {
            return _certificateMemberApiRepository.UpdateClientCertExpired(ClientCertId, MID);
        }
    }
}
