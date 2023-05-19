using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ICP.Modules.Api.Authorization.Services
{
    using Infrastructure.Abstractions.Logging;
    using Infrastructure.Core.Models;
    using Infrastructure.Core.Extensions;
    using Infrastructure.Core.Helpers;
    using Models;

    public class BaseIdentifyService<T>
    {
        private readonly ILogger _logger = null;

        public BaseIdentifyService(ILogger<T> logger)
        {
            _logger = logger;
        }

        public DataResult<T> DecryptClientAesData<T>(string key, string iv, string encData)
        {
            string decryptData = null;

            return DecryptClientAesData<T>(key, iv, encData, ref decryptData);
        }

        public DataResult<T> DecryptClientAesData<T>(string key, string iv, string encData, ref string DecryptData)
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
                DecryptData = json;

                result.SetSuccess(data);
                _logger.Trace($"解密 {nameof(encData)} 成功");
            }
            catch (Exception ex)
            {
                result.SetCode(1018);
                _logger.Warning(ex, result.RtnMsg);
            }

            return result;
        }

        public DataResult<object> ParseRequestModel(Type targetType, string decryptData)
        {
            var result = new DataResult<object>();
            result.SetError();

            var listError = new List<string>();

            try
            {
                _logger.Trace($"準備反序列化 {nameof(decryptData)}，長度 = {decryptData?.Length}，{nameof(targetType)} = {targetType.FullName}");

                object request = JsonConvert.DeserializeObject(decryptData, targetType, new JsonSerializerSettings
                {
                    Error = delegate (object sender, Newtonsoft.Json.Serialization.ErrorEventArgs args)
                    {
                        listError.Add(args.ErrorContext.Path);
                    }
                });

                _logger.Trace($"反序列化成功");

                result.SetSuccess(request);
            }
            catch (Exception ex)
            {
                result.SetCode(1019, string.Join(", ", listError));
                _logger.Warning(ex, result.RtnMsg);
            }

            return result;
        }

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

                _logger.Trace($"加密成功");

                result.SetSuccess(encData);
            }
            catch (Exception ex)
            {
                result.SetCode(1020);
                _logger.Warning(ex, result.RtnMsg);
            }

            return result;
        }

        public BaseResult ValidTimestamp(string timestamp, int minutes = 1)
        {
            var result = new BaseResult();
            result.SetError();

            _logger.Trace($"準備驗證 Timestamp，{nameof(timestamp)} = {timestamp}");
            if (!DateTime.TryParse(timestamp, out DateTime dt))
            {
                result.SetCode(1012);
            }
            else if (DateTime.Now.Subtract(dt) > TimeSpan.FromMinutes(minutes))
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
    }
}
