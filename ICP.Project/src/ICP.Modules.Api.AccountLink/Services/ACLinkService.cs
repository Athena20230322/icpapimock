using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Helpers;
using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Utils;
using ICP.Library.Models.AccountLinkApi.Enums;
using ICP.Library.Services.AccountLinkApi;
using ICP.Modules.Api.AccountLink.Enums;
using ICP.Modules.Api.AccountLink.Models;
using ICP.Modules.Api.AccountLink.Models.Cathay;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace ICP.Modules.Api.AccountLink.Services
{
    /// <summary>
    /// AccountLink 共用方法
    /// </summary>
    class ACLinkService : ACLinkConfigService
    {
        protected ILogger _logger = null;

        /// <summary>
        /// 取Api功能名稱
        /// </summary>
        /// <param name="apiType"></param>
        /// <returns></returns>
        public string GetApiName(ApiType apiType)
        {
            string result = string.Empty;

            switch (apiType)
            {
                case ApiType.ACLinkBind://綁定
                    result = "綁定";
                    break;
                //case ApiType.ACLinkResult://綁定結果通知(銀行發動)
                //    sMsgid = "綁定結果通知";
                //    break;
                case ApiType.ACLinkCancel://取消綁定
                    result = "取消綁定";
                    break;
                case ApiType.ACLinkPay://付款
                    result = "付款";
                    break;
                case ApiType.ACLinkDeposit://儲值
                    result = "儲值";
                    break;
                case ApiType.ACLinkRefund://退款
                    result = "退款";
                    break;
                case ApiType.ACLinkWithdrawal://提領
                    result = "提領";
                    break;
                case ApiType.ACLinkBindQuery://綁定查詢
                    result = "綁定查詢";
                    break;
                case ApiType.ACLinkPayQuery://交易查詢
                    result = "交易查詢";
                    break;
                default:
                    break;
            }

            return result;
        }

        /// <summary>
        /// 取傳送路徑
        /// </summary>
        /// <param name="apiType"></param>
        /// <returns></returns>
        public string GetPostUrl(ApiType apiType)
        {
            string sUrl = string.Empty;

            switch (apiType)
            {
                case ApiType.ACLinkBind://綁定
                    sUrl = ACLinkBindAddr;
                    break;
                case ApiType.ACLinkCancel://取消綁定
                    sUrl = ACLinkCancelAddr;
                    break;
                case ApiType.ACLinkPay://付款
                    sUrl = ACLinkPayAddr;
                    break;
                case ApiType.ACLinkDeposit://儲值
                    sUrl = ACLinkDepositAddr;
                    break;
                case ApiType.ACLinkRefund://退款
                    sUrl = ACLinkRefundAddr;
                    break;
                case ApiType.ACLinkWithdrawal://提領
                    sUrl = ACLinkWithdrawalAddr;
                    break;
                case ApiType.ACLinkBindQuery://綁定查詢
                    sUrl = ACLinkBindQryAddr;
                    break;
                case ApiType.ACLinkPayQuery://交易查詢
                    sUrl = ACLinkPayQryAddr;
                    break;
                case ApiType.ACLinkBindApply://綁定申請
                    sUrl = ACLinkBindApplyAddr;
                    break;
                case ApiType.BindApiResult://綁定結果通知(後端)
                    sUrl = BindResultApiUrl;
                    break;
                case ApiType.BindWebResult://綁定結束導回(前端)
                    sUrl = BindResultWebUrl;
                    break;
                default:
                    break;
            }

            return sUrl;
        }

        /// <summary>
        /// 取得訊息序號(業者交易序號)
        /// </summary>
        /// <param name="bankType"></param>
        /// <returns></returns>
        public string GetMsgNo(BankType bankType)
        {
            return _acLinkRepository.AddAccountLinkMsgNo(1, ((int)bankType).ToString("000"));
        }

        /// <summary>
        /// 取得帳號識別序號
        /// </summary>
        /// <param name="apiType"></param>
        /// <returns></returns>
        public string GetINDTAccount(BankType bankType)
        {
            return _acLinkRepository.AddAccountLinkMsgNo(2, ((int)bankType).ToString("000"));
        }

        /// <summary>
        /// 檢查是否逾時
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public BaseResult CheckTimeout(string timestamp)
        {
            _logger.Trace($"檢查api是否逾時-Before: {timestamp}");

            var result = new BaseResult();

            if (!DateTime.TryParse(timestamp, out DateTime dt))
            {
                result.SetCode(7499);//系統非預期錯誤
            }

            double subSec = DateTime.Now.Subtract(dt).TotalSeconds;
            if (subSec > CommonConfigService.TimeoutSec)
            {
                result.SetCode(7401);//連線逾時
            }
            else
            {
                result.SetSuccess();
            }

            _logger.Trace($"檢查api是否逾時-After: {JsonConvert.SerializeObject(result)}");

            return result;
        }

        /// <summary>
        /// 新增帳戶綁定資訊
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult AddACLinkInfo(ACLinkAddModel model)
        {
            _logger.Trace($"新增帳戶綁定資訊-Before: {JsonConvert.SerializeObject(model)}");

            var result = new BaseResult();

            var reqDbModel = AutoMapper.Mapper.Map<ACLinkAddDbReq>(model);

            result = _acLinkRepository.AddAccountLink(reqDbModel);

            _logger.Trace($"新增帳戶綁定資訊-After: {JsonConvert.SerializeObject(result)}");

            return result;
        }

        /// <summary>
        /// 取得帳戶綁定資訊
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<ACLinkInfoModel> GetACLinkInfo(ACLinkInfoQryModel model)
        {
            _logger.Trace($"取帳戶綁定資訊-Before: {JsonConvert.SerializeObject(model)}");

            var result = new DataResult<ACLinkInfoModel>();

            var reqDbModel = new ACLinkInfoDbReq
            {
                MID = model.MID,
                BankCode = ((int)model.BankType).ToString("000"),
                INDTAccount = model.INDTAccount
            };

            ACLinkInfoDbRes resDbModel = _acLinkRepository.GetAccountLinkInfo(reqDbModel);

            var resModel = AutoMapper.Mapper.Map<ACLinkInfoModel>(resDbModel);

            if (resModel == null)
            {
                result.SetCode(7404);//查無綁定帳戶
            }
            else
            {
                result.SetSuccess(resModel);
            }

            _logger.Trace($"取帳戶綁定資訊-After: {JsonConvert.SerializeObject(result)}");

            return result;
        }

        /// <summary>
        /// Json轉Model
        /// </summary>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        public DataResult<T> ParseToModel<T>(string jsonData)
        {
            _logger.Trace($"反序列化-Before: {nameof(jsonData)}，長度 = {jsonData?.Length}");

            var result = new DataResult<T>();

            T data = jsonData.TryParseJsonToObj(out T obj) ? obj : default(T);

            if (data != null)
            {
                result.SetSuccess(data);
            }
            else
            {
                result.SetCode(7400);//資料轉換失敗
            }

            _logger.Trace($"反序列化-After: {JsonConvert.SerializeObject(result)}");

            return result;
        }

        /// <summary>
        /// 驗證欄位
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">傳入需驗證的物件</param>
        /// <returns></returns>
        public BaseResult ValidateField<T>(T model)
        {
            _logger.Trace($"驗證欄位-Before: {JsonConvert.SerializeObject(model)}");

            var result = new BaseResult();

            StringBuilder errorMsg = new StringBuilder();
            List<string> errList = new List<string>();

            errList.AddRange(ServerValidator.Validate(model));

            foreach (var item in errList)
            {
                errorMsg.Append(item.ToString() + " ");
            }

            if (string.IsNullOrWhiteSpace(errorMsg.ToString()))
            {
                result.SetSuccess();
            }
            else
            {
                result.SetCode(7403, errorMsg);//xxx欄位驗證失敗
            }

            _logger.Trace($"驗證欄位-After: {JsonConvert.SerializeObject(result)}");

            return result;
        }

        /// <summary>
        /// 驗證Hash
        /// </summary>
        /// <param name="digestHash"></param>
        /// <param name="rawData"></param>
        /// <returns></returns>
        public BaseResult ValidateHash(string digestHash, string rawData)
        {
            _logger.Trace($"驗證Hash-Before: {rawData}");

            var result = new BaseResult();

            string sha = new HashCryptoHelper().HashSha256(rawData).ToUpper();

            if (sha.Equals(digestHash.ToUpper()))
            {
                result.SetSuccess();
            }
            else
            {
                result.SetCode(7405);//資料驗證失敗
            }

            _logger.Trace($"驗證Hash-After: {JsonConvert.SerializeObject(result)}");

            return result;
        }

        /// <summary>
        /// 驗證簽章
        /// </summary>
        /// <param name="signDataHex">16進位的簽章字串</param>
        /// <returns></returns>
        public BaseResult ValidateSign(string signDataHex)
        {
            _logger.Trace($"驗證簽章-Before: {signDataHex}");

            var result = new BaseResult();

            KeyApiServiceReference.KeyApiSoapClient ws = new KeyApiServiceReference.KeyApiSoapClient();

            string bankCode = $"{(int)this._bankType:000}";

            bool verifyResult = ws.VerifySign(bankCode, signDataHex, HSMKeyLabel, HSMCertId);

            if (verifyResult)
            {
                result.SetSuccess();
            }
            else
            {
                result.SetCode(7410);//簽章驗證失敗
            }

            _logger.Trace($"驗證簽章-After: {JsonConvert.SerializeObject(result)}");

            return result;
        }

        /// <summary>
        /// 簽章
        /// </summary>
        /// <param name="rawData">欲簽章的字串</param>
        /// <returns></returns>
        public DataResult<string> Sign(string rawData)
        {
            _logger.Trace($"簽章-Before: {rawData}");

            var result = new DataResult<string>();

            KeyApiServiceReference.KeyApiSoapClient ws = new KeyApiServiceReference.KeyApiSoapClient();

            string bankCode = $"{(int)this._bankType:000}";

            string signed = ws.Sign(bankCode, rawData, HSMKeyLabel, HSMCertId);

            if (!string.IsNullOrEmpty(signed))
            {
                result.SetSuccess(signed);
            }
            else
            {
                result.SetCode(7409);//簽章失敗
            }

            _logger.Trace($"簽章-After: {JsonConvert.SerializeObject(result)}");

            return result;
        }

        /// <summary>
        /// 3DES加密
        /// </summary>
        /// <param name="rawData">欲加密的字串</param>
        /// <returns></returns>
        public string TripleDESEncrypt(string rawData)
        {
            KeyApiServiceReference.KeyApiSoapClient ws = new KeyApiServiceReference.KeyApiSoapClient();

            string result = ws.TripleDESEncrypt(rawData, HSM3DESKeyLabel);

            return result;
        }

        /// <summary>
        /// 3DES加密
        /// </summary>
        /// <param name="rawData">欲加密的字串</param>
        /// <param name="keyHex">16進位的key</param>
        /// <returns></returns>
        public string TripleDESEncryptByKey(string rawData, string keyHex)
        {
            KeyApiServiceReference.KeyApiSoapClient ws = new KeyApiServiceReference.KeyApiSoapClient();

            string result = ws.TripleDESEncryptByKey(rawData, keyHex);

            return result;
        }

        /// <summary>
        /// 3DES解密
        /// </summary>
        /// <param name="encryptedHex">欲解密的16進位字串</param>
        /// <returns></returns>
        public string TripleDESDecrypt(string encryptedHex)
        {
            KeyApiServiceReference.KeyApiSoapClient ws = new KeyApiServiceReference.KeyApiSoapClient();

            string result = ws.TripleDESDecrypt(encryptedHex, HSM3DESKeyLabel);

            return result;
        }

        /// <summary>
        /// 3DES解密
        /// </summary>
        /// <param name="encryptedHex">欲解密的16進位字串</param>
        /// <param name="keyHex">16進位的key</param>
        /// <returns></returns>
        public string TripleDESDecryptByKey(string encryptedHex, string keyHex)
        {
            KeyApiServiceReference.KeyApiSoapClient ws = new KeyApiServiceReference.KeyApiSoapClient();

            string result = ws.TripleDESDecryptByKey(encryptedHex, keyHex);

            return result;
        }


        /// <summary>
        /// 傳送至銀行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiType"></param>
        /// <param name="model"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public DataResult<string> PostToBankWithJson<T>(ApiType apiType, T model, Dictionary<string, string> headers)
        {
            _logger.Trace($"傳送至銀行({apiType.ToString()})-Before: {JsonConvert.SerializeObject(model)}");

            var result = new DataResult<string>();

            string postUrl = GetPostUrl(apiType);
            string json = JsonConvert.SerializeObject(model);

            string postResult = new NetworkHelper().DoRequestWithJson(postUrl, json, 0, null, headers);

            if (!string.IsNullOrWhiteSpace(postResult))
            {
                result.SetSuccess(postResult);
            }
            else
            {
                result.SetCode(7402);//連線失敗
            }

            _logger.Trace($"傳送至銀行({apiType.ToString()})-After: {JsonConvert.SerializeObject(result)}");

            return result;
        }

        /// <summary>
        /// 傳送至銀行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiType"></param>
        /// <param name="model"></param>
        /// <param name="codepage"></param>
        /// <returns></returns>
        public DataResult<string> PostToBank<T>(ApiType apiType, T model, int codepage = 65001)
        {
            _logger.Trace($"傳送至銀行({apiType.ToString()})-Before: {JsonConvert.SerializeObject(model)}");

            var result = new DataResult<string>();

            string postUrl = GetPostUrl(apiType);

            if (!isMockBank())
            {
                var queryString = HttpUtility.ParseQueryString(new CommonService().Obj2QueryString(model));
                var dict = NameValueCollectionExtension.ToDictionary(queryString);

                try
                {
                    string postResult = new NetworkHelper().DoRequestWithUrlEncode(postUrl, dict, codepage);

                    if (!string.IsNullOrWhiteSpace(postResult))
                    {
                        result.SetSuccess(postResult);
                    }
                    else
                    {
                        result.SetCode(7402);//連線失敗
                    }
                }
                catch(Exception ex)
                {
                    //result.SetCode(7402);//連線失敗
                    result.SetError();
                    result.RtnMsg = ex.ToString();
                }
            }
            else
            {
                result.SetSuccess();
            }

            _logger.Trace($"傳送至銀行({apiType.ToString()})-After: {JsonConvert.SerializeObject(result)}");

            return result;
        }

        /// <summary>
        /// 傳送
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="postUrl"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<string> Post<T>(string postUrl, T model)
        {
            _logger.Trace($"傳送-Before: {JsonConvert.SerializeObject(model)}");

            var result = new DataResult<string>();

            string json = JsonConvert.SerializeObject(model);

            string postResult = new NetworkHelper().DoRequestWithJson(postUrl, json);

            if (!string.IsNullOrWhiteSpace(postResult))
            {
                result.SetSuccess(postResult);
            }
            else
            {
                result.SetCode(7402);//連線失敗
            }

            _logger.Trace($"傳送-After: {JsonConvert.SerializeObject(result)}");

            return result;
        }

        /// <summary>
        /// 是否模擬銀行
        /// </summary>
        /// <returns></returns>
        public bool isMockBank()
        {
            return MockBank;
        }

        /// <summary>
        ///16進位字串轉換
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GetHexadecimal(string data)
        {
            byte[] byteSource = Encoding.Default.GetBytes(data);
            StringBuilder result = new StringBuilder();
            foreach (var item in byteSource)
            {
                result.Append(item.ToString("X2"));
            }
            return result.ToString();
        }

    }
}
