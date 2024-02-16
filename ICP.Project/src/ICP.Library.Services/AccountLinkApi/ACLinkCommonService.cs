using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Helpers;
using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Utils;
using ICP.Library.Models.AccountLinkApi;
using Newtonsoft.Json;
using System;

namespace ICP.Library.Services.AccountLinkApi
{
    public class ACLinkCommonService
    {
        private readonly ILogger _logger = null;

        public ACLinkCommonService(ILogger<ACLinkCommonService> logger)
        {
            _logger = logger;
        }

        #region 加解密處理
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
                var tmpData = JsonConvert.DeserializeObject(encData);
                encData = tmpData as string;

                _logger.Trace($"準備解密 {nameof(encData)}，長度 = {encData?.Length}");
                string json = aesCryptoHelper.Decrypt(encData);

                T data = json.TryParseJsonToObj(out T obj) ? obj : default(T);
                //data.Json = json;

                result.SetSuccess(data);
                _logger.Trace($"解密 {nameof(encData)} 成功");
            }
            catch (Exception ex)
            {
                result.SetCode(7408);
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
                result.SetCode(7407);
                _logger.Warning(ex, result.RtnMsg);
            }

            return result;
        }

        /// <summary>
        /// 取得AccountLink Key IV
        /// </summary>
        /// <returns></returns>
        public DataResult<ACLinkSettingModel> GetACLinkKey()
        {
            var result = new DataResult<ACLinkSettingModel>();
            result.SetError();

            // 建立基本物件
            string aclinkKey = string.Empty;
            string aclinkIV = string.Empty;

            _logger.Trace("準備取得 ACLink Key IV");
            aclinkKey = GlobalConfigUtil.ACLink_HashKey;
            aclinkIV = GlobalConfigUtil.ACLink_HashIV;

            if (string.IsNullOrWhiteSpace(aclinkKey))
            {
                result.SetCode(7499);
                _logger.Warning(result.RtnMsg);
                return result;
            }
            if (string.IsNullOrWhiteSpace(aclinkIV))
            {
                result.SetCode(7499);
                _logger.Warning(result.RtnMsg);
                return result;
            }
            _logger.Trace("取得 ACLink Key IV 成功");

            result.SetSuccess(new ACLinkSettingModel
            {
                ACKey = aclinkKey,
                ACIV = aclinkIV
            });

            return result;
        }
        #endregion

        #region ACLink相關設定
        /// <summary>
        /// 取得遮蔽後的銀行帳號 (只顯示後5碼)
        /// </summary>
        /// <param name="bankAccount"></param>
        /// <returns></returns>
        public string GetBankAccountMask(string bankAccount)
        {
            string result = string.Empty;

            string temp = "********************";
            int accountLen = bankAccount.Length;
            int showLen = 5;

            if (accountLen <= showLen)
            {
                return result;
            }

            result = string.Format("{0},{1}",
                temp.Substring(accountLen - showLen),
                bankAccount.Substring(accountLen - showLen, showLen));

            return result;
        }

        /// <summary>
        /// 依銀行代碼取得AccountLink銀行相關資訊
        /// </summary>
        /// <param name="bankCode"></param>
        /// <returns></returns>
        public dynamic GetACLinkBankInfo(string bankCode)
        {
            string shortCName = string.Empty;
            string cName = string.Empty;
            string fullName = string.Empty;
            string eName = string.Empty;
            int paymentSubTypeID = 0;

            switch (bankCode)
            {
                case "007":
                    shortCName = "一銀";
                    cName = "第一銀行";
                    fullName = "第一商業銀行";
                    eName = "FIRST";
                    paymentSubTypeID = 1;
                    break;
                case "822":
                    shortCName = "中信";
                    cName = "中國信託";
                    fullName = "中國信託商業銀行";
                    eName = "CTBC";
                    paymentSubTypeID = 2;
                    break;
                case "013":
                    shortCName = "國泰";
                    cName = "國泰世華";
                    fullName = "國泰世華商業銀行";
                    eName = "CATHAY";
                    paymentSubTypeID = 3;
                    break;
            }

            var result = new
            {
                BankName = cName,
                BankShortName = shortCName,
                BankFullName = fullName,
                BankEName = eName,
                PaymentSubTypeID = paymentSubTypeID
            };

            return result;
        }
        #endregion
    }
}
