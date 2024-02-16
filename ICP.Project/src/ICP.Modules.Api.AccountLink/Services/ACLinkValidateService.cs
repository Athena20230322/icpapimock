using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Helpers;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.AccountLinkApi;
using ICP.Modules.Api.AccountLink.Models;
using Newtonsoft.Json;
using System;

namespace ICP.Modules.Api.AccountLink.Services
{
    /// <summary>
    /// 驗證API來源所使用的方法
    /// </summary>
    public class ACLinkValidateService
    {
        private readonly ILogger _logger = null;

        public ACLinkValidateService(ILogger<ACLinkValidateService> logger)
        {
            _logger = logger;
        }

        public DataResult<ACLinkValidateModel> ProcessRequest(string token)
        {
            var result = new DataResult<ACLinkValidateModel>();
            result.SetError();

            // 驗證token
            var validResult = BaseValidateProcess(token);
            if (!validResult.IsSuccess)
            {
                result.SetError(validResult);
                return result;
            }

            // 取得 ACLink Key 及 IV
            string aclinkKey = CommonConfigService.ACLinkHashKey;
            string aclinkIV = CommonConfigService.ACLinkHashIV;

            // 解密密文
            var decryptResult = DecryptClientAesData<ACLinkDecryptModel>(aclinkKey, aclinkIV, token);
            if (!decryptResult.IsSuccess)
            {
                result.SetError(decryptResult);
                return result;
            }

            var rtnModel = new ACLinkDecryptModel
            {
                BankType = decryptResult.RtnData.BankType,
                Json = decryptResult.RtnData.Json
            };

            result.SetSuccess(new ACLinkValidateModel
            {
                ACKey = aclinkKey,
                ACIV = aclinkIV,
                ACModel = rtnModel
            });

            return result;
        }

        public DataResult<ACLinkValidateModel> ProcessResponse(DataResult dataResult)
        {
            var result = new DataResult<ACLinkValidateModel>();
            result.SetError();

            // 取得 ACLink Key 及 IV
            string aclinkKey = CommonConfigService.ACLinkHashKey;
            string aclinkIV = CommonConfigService.ACLinkHashIV;

            // 加密回傳資訊
            var encryptResult = EncryptClientAesData(aclinkKey, aclinkIV, dataResult.RtnData);
            if (!encryptResult.IsSuccess)
            {
                result.SetError(encryptResult);
                return result;
            }

            var rtnModel = new ACLinkDecryptModel
            {
                Json = encryptResult.RtnData
            };

            result.SetSuccess(new ACLinkValidateModel
            {
                ACModel = rtnModel
            });

            return result;
        }


        public DataResult<ACLinkValidateModel> BaseValidateProcess(string token = "")
        {
            var result = new DataResult<ACLinkValidateModel>();
            var rtnModel = new ACLinkValidateModel();
            result.SetSuccess(rtnModel);

            if (string.IsNullOrWhiteSpace(token))
            {
                result.SetCode(7411);
                _logger.Warning(result.RtnMsg);
                return result;
            }

            return result;
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DataResult<string> EncryptClientAesData(string key, string iv, object obj)
        {
            var result = new DataResult<string>();

            AesCryptoHelper aesCryptoHelper = new AesCryptoHelper
            {
                Key = key,
                Iv = iv
            };

            try
            {
                string json = JsonConvert.SerializeObject(obj);

                _logger.Trace($"準備加密 {nameof(json)}，長度 = {json?.Length}");

                string encData = aesCryptoHelper.Encrypt(json);

                result.SetSuccess(encData);

                _logger.Trace($"加密成功");
            }
            catch (Exception ex)
            {
                result.SetCode(7412);
                _logger.Warning(ex, result.RtnMsg);
            }

            return result;
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <param name="encData"></param>
        /// <returns></returns>
        public DataResult<T> DecryptClientAesData<T>(string key, string iv, string encData) where T : ACLinkDecryptModel
        {
            var result = new DataResult<T>();

            AesCryptoHelper aesCryptoHelper = new AesCryptoHelper
            {
                Key = key,
                Iv = iv
            };

            try
            {
                _logger.Trace($"準備解密 {nameof(encData)}，長度 = {encData?.Length}");
                string json = aesCryptoHelper.Decrypt(encData);

                T data = json.TryParseJsonToObj(out T obj) ? obj : default(T);

                result.SetSuccess(data);
                _logger.Trace($"解密 {nameof(encData)} 成功");
            }
            catch (Exception ex)
            {
                result.SetCode(7412);
                _logger.Warning(ex, result.RtnMsg);
            }

            return result;
        }

    }
}
