using AutoMapper;
using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Utils;
using ICP.Library.Models.AccountLinkApi;
using ICP.Library.Services.AccountLinkApi;
using ICP.Modules.Api.Member.Models.ACLink;
using ICP.Modules.Api.Member.Services;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Web;

namespace ICP.Modules.Api.Member.Commands
{
    public class ACLinkApiCommand
    {
        private ACLinkService _acLinkService = null;
        private ACLinkCommonService _commonService = null;
        private readonly ILogger _logger = null;

        public ACLinkApiCommand(
            ACLinkService accountLinkService,
            ACLinkCommonService aCLinkCommonService,
            ILogger<ACLinkApiCommand> logger)
        {
            _acLinkService = accountLinkService;
            _commonService = aCLinkCommonService;
            _logger = logger;
        }

        #region 綁定AccountLink
        /// <summary>
        /// 綁定AccountLink帳號導頁至銀行端
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<ACLinkBindRes> ACLinkBind(ACLinkBindReq request)
        {
            string apiType = "ACLinkBind";
            _logger.Info($"[ACLinkBind][Input] {JsonConvert.SerializeObject(request)}");

            var result = new DataResult<ACLinkBindRes>();
            result.SetError();

            #region 處理傳入資料

            // 驗證參數
            var validateResult = _acLinkService.ValidateField(request);
            if (!validateResult.IsSuccess)
            {
                result.SetCode(validateResult.RtnCode, validateResult.RtnMsg);
                return result;
            }

            // 驗證欄位邏輯
            ACLinkBindModel acLinkBindModel = Mapper.Map<ACLinkBindModel>(request);
            var checkResult = _acLinkService.ValidateACLinkBind(acLinkBindModel);
            if (!checkResult.IsSuccess)
            {
                result.SetCode(checkResult.RtnCode, checkResult.RtnMsg);
                return result;
            }

            // 中國信託申請綁定，直接回傳申請綁定的網址
            if (request.BankCode == "822" && request.BindFlag != "Y")
            {
                return checkResult;
            }

            #endregion

            #region 送至AccountLink

            // 組成送出資料
            var getPostData = _acLinkService.ACLinkBindPostData(acLinkBindModel);
            if (!getPostData.IsSuccess)
            {
                result.SetCode(getPostData.RtnCode, getPostData.RtnMsg);
                return result;
            }

            // 取得 ACLink Key 及 IV
            var getACLinkKey = _commonService.GetACLinkKey();
            if (!getACLinkKey.IsSuccess)
            {
                result.SetCode(getACLinkKey.RtnCode, getACLinkKey.RtnMsg);
                return result;
            }

            // 加密資料
            var encryptResult = _commonService.EncryptClientAesData(getACLinkKey.RtnData.ACKey, getACLinkKey.RtnData.ACIV, getPostData.RtnData);
            if (!encryptResult.IsSuccess)
            {
                result.SetCode(encryptResult.RtnCode, encryptResult.RtnMsg);
                return result;
            }

            _logger.Info($"[送至AccountLink] postData:{JsonConvert.SerializeObject(getPostData.RtnData)}, encryptData:{JsonConvert.SerializeObject(encryptResult.RtnData)}");

            // 送至AccountLink
            var postResult = PostToACLink(apiType, encryptResult.RtnData);
            if (!postResult.IsSuccess)
            {
                result.SetCode(postResult.RtnCode, postResult.RtnMsg);
                return result;
            }

            _logger.Info($"[接收AccountLink回傳結果] {JsonConvert.SerializeObject(postResult)}");

            #endregion

            #region 處理回傳資料

            // 解密密文
            var decryptResult = _commonService.DecryptClientAesData<ACLinkDecryptModel>(getACLinkKey.RtnData.ACKey, getACLinkKey.RtnData.ACIV, postResult.RtnData);
            if (!decryptResult.IsSuccess)
            {
                result.SetCode(decryptResult.RtnCode, decryptResult.RtnMsg);
                return result;
            }

            // 取得回傳資料
            var resultData = _acLinkService.ACLinkBindReturnData(decryptResult.RtnData);
            result = resultData;

            _logger.Info($"[ACLinkBind][Output] {JsonConvert.SerializeObject(result)}");

            #endregion

            return result;
        }
        #endregion

        #region 申請綁定AccountLink
        /// <summary>
        /// 申請綁定AccountLink帳號
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<ACLinkApplyRes> ACLinkApply(ACLinkApplyReq request)
        {
            string apiType = "ACLinkApply";
            _logger.Info($"[ACLinkApply][Input] {JsonConvert.SerializeObject(request)}");

            var result = new DataResult<ACLinkApplyRes>();
            result.SetError();

            #region 處理傳入資料

            // 驗證參數
            var validateResult = _acLinkService.ValidateField(request);
            if (!validateResult.IsSuccess)
            {
                result.SetCode(validateResult.RtnCode, validateResult.RtnMsg);
                return result;
            }

            // 驗證欄位邏輯
            ACLinkBindModel acLinkBindModel = Mapper.Map<ACLinkBindModel>(request);
            var checkResult = _acLinkService.ValidateACLinkApply(acLinkBindModel);
            if (!checkResult.IsSuccess)
            {
                result.SetCode(checkResult.RtnCode, checkResult.RtnMsg);
                return result;
            }

            #endregion

            #region 送至AccountLink

            // 組成送出資料
            var getPostData = _acLinkService.ACLinkApplyPostData(acLinkBindModel);
            if (!getPostData.IsSuccess)
            {
                result.SetCode(getPostData.RtnCode, getPostData.RtnMsg);
                return result;
            }

            // 取得 ACLink Key 及 IV
            var getACLinkKey = _commonService.GetACLinkKey();
            if (!getACLinkKey.IsSuccess)
            {
                result.SetCode(getACLinkKey.RtnCode, getACLinkKey.RtnMsg);
                return result;
            }

            // 加密資料
            var encryptResult = _commonService.EncryptClientAesData(getACLinkKey.RtnData.ACKey, getACLinkKey.RtnData.ACIV, getPostData.RtnData);
            if (!encryptResult.IsSuccess)
            {
                result.SetCode(encryptResult.RtnCode, encryptResult.RtnMsg);
                return result;
            }

            _logger.Info($"[送至AccountLink] postData:{JsonConvert.SerializeObject(getPostData.RtnData)}, encryptData:{JsonConvert.SerializeObject(encryptResult.RtnData)}");

            // 送至AccountLink
            var postResult = PostToACLink(apiType, encryptResult.RtnData);
            if (!postResult.IsSuccess)
            {
                result.SetCode(postResult.RtnCode, postResult.RtnMsg);
                return result;
            }

            _logger.Info($"[接收AccountLink回傳結果] {JsonConvert.SerializeObject(postResult)}");

            #endregion

            #region 處理回傳資料

            // 解密密文
            var decryptResult = _commonService.DecryptClientAesData<ACLinkDecryptModel>(getACLinkKey.RtnData.ACKey, getACLinkKey.RtnData.ACIV, postResult.RtnData);
            if (!decryptResult.IsSuccess)
            {
                result.SetCode(decryptResult.RtnCode, decryptResult.RtnMsg);
                return result;
            }

            // 取得回傳資料
            var resultData = _acLinkService.ACLinkApplyReturnData(decryptResult.RtnData);
            result = resultData;

            _logger.Info($"[ACLinkApply][Output] {JsonConvert.SerializeObject(result)}");

            #endregion

            return result;
        }
        #endregion

        #region 取消綁定AccountLink
        /// <summary>
        /// 取消綁定AccountLink
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult ACLinkCancel(ACLinkCancelBindReq request)
        {
            string apiType = "ACLinkCancel";
            _logger.Info($"[ACLinkCancel][Input] {JsonConvert.SerializeObject(request)}");

            var result = new BaseResult();
            result.SetError();

            #region 處理傳入資料

            // 驗證參數
            var validateResult = _acLinkService.ValidateField(request);
            if (!validateResult.IsSuccess)
            {
                result.SetCode(validateResult.RtnCode, validateResult.RtnMsg);
                return result;
            }

            #endregion

            #region 送至AccountLink

            // 組成送出資料
            ACLinkBindModel acLinkBindModel = Mapper.Map<ACLinkBindModel>(request);
            var getPostData = _acLinkService.ACLinkCancelPostData(acLinkBindModel);
            if (!getPostData.IsSuccess)
            {
                result.SetCode(getPostData.RtnCode, getPostData.RtnMsg);
                return result;
            }

            // 取得 ACLink Key 及 IV
            var getACLinkKey = _commonService.GetACLinkKey();
            if (!getACLinkKey.IsSuccess)
            {
                result.SetCode(getACLinkKey.RtnCode, getACLinkKey.RtnMsg);
                return result;
            }

            // 加密資料
            var encryptResult = _commonService.EncryptClientAesData(getACLinkKey.RtnData.ACKey, getACLinkKey.RtnData.ACIV, getPostData.RtnData);
            if (!encryptResult.IsSuccess)
            {
                result.SetCode(encryptResult.RtnCode, encryptResult.RtnMsg);
                return result;
            }

            _logger.Info($"[送至AccountLink] postData:{JsonConvert.SerializeObject(getPostData.RtnData)}, encryptData:{JsonConvert.SerializeObject(encryptResult.RtnData)}");

            // 送至AccountLink
            var postResult = PostToACLink(apiType, encryptResult.RtnData);
            if (!postResult.IsSuccess)
            {
                result.SetCode(postResult.RtnCode, postResult.RtnMsg);
                return result;
            }

            _logger.Info($"[接收AccountLink回傳結果] {JsonConvert.SerializeObject(postResult)}");

            #endregion

            #region 處理回傳資料

            // 解密密文
            var decryptResult = _commonService.DecryptClientAesData<ACLinkDecryptModel>(getACLinkKey.RtnData.ACKey, getACLinkKey.RtnData.ACIV, postResult.RtnData);
            if (!decryptResult.IsSuccess)
            {
                result.SetCode(decryptResult.RtnCode, decryptResult.RtnMsg);
                return result;
            }

            // 取得回傳資料
            var resultData = _acLinkService.ACLinkCancelReturnData(decryptResult.RtnData);
            result = resultData;

            _logger.Info($"[ACLinkCancel][Output] {JsonConvert.SerializeObject(result)}");

            #endregion

            return result;
        }
        #endregion

        #region 提領電支帳戶金額至約定銀行帳戶
        /// <summary>
        /// 提領電支帳戶金額至約定銀行帳戶
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult ACLinkWithdrawal(ACLinkWithdrawalReq request)
        {
            string apiType = "ACLinkWithdrawal";
            _logger.Info($"[ACLinkWithdrawal][Input] {JsonConvert.SerializeObject(request)}");

            var result = new BaseResult();
            result.SetError();

            #region 處理傳入資料

            // 驗證參數
            var validateResult = _acLinkService.ValidateField(request);
            if (!validateResult.IsSuccess)
            {
                result.SetCode(validateResult.RtnCode, validateResult.RtnMsg);
                return result;
            }

            #endregion

            #region 送至AccountLink

            // 組成送出資料
            var getPostData = _acLinkService.ACLinkWithdrawalPostData(request);
            if (!getPostData.IsSuccess)
            {
                result.SetCode(getPostData.RtnCode, getPostData.RtnMsg);
                return result;
            }

            // 取得 ACLink Key 及 IV
            var getACLinkKey = _commonService.GetACLinkKey();
            if (!getACLinkKey.IsSuccess)
            {
                result.SetCode(getACLinkKey.RtnCode, getACLinkKey.RtnMsg);
                return result;
            }

            // 加密資料
            var encryptResult = _commonService.EncryptClientAesData(getACLinkKey.RtnData.ACKey, getACLinkKey.RtnData.ACIV, getPostData.RtnData);
            if (!encryptResult.IsSuccess)
            {
                result.SetCode(encryptResult.RtnCode, encryptResult.RtnMsg);
                return result;
            }

            _logger.Info($"[送至AccountLink] postData:{JsonConvert.SerializeObject(getPostData.RtnData)}, encryptData:{JsonConvert.SerializeObject(encryptResult.RtnData)}");

            // 送至AccountLink
            var postResult = PostToACLink(apiType, encryptResult.RtnData);
            if (!postResult.IsSuccess)
            {
                result.SetCode(postResult.RtnCode, postResult.RtnMsg);
                return result;
            }

            _logger.Info($"[接收AccountLink回傳結果] {JsonConvert.SerializeObject(postResult)}");

            #endregion

            #region 處理回傳資料

            // 解密密文
            var decryptResult = _commonService.DecryptClientAesData<ACLinkDecryptModel>(getACLinkKey.RtnData.ACKey, getACLinkKey.RtnData.ACIV, postResult.RtnData);
            if (!decryptResult.IsSuccess)
            {
                result.SetCode(decryptResult.RtnCode, decryptResult.RtnMsg);
                return result;
            }

            // 取得回傳資料
            var resultData = _acLinkService.ACLinkWithdrawalReturnData(decryptResult.RtnData);
            result = resultData;

            _logger.Info($"[ACLinkWithdrawal][Output] {JsonConvert.SerializeObject(result)}");

            #endregion

            return result;
        }
        #endregion

        #region DoRequest
        /// <summary>
        /// 送至AccountLink站台
        /// </summary>
        /// <param name="api"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public DataResult<string> PostToACLink(string api, string postData)
        {
            var result = new DataResult<string>();
            result.SetError();

            var acLinkDomain = $"{GlobalConfigUtil.Host_Middleware_AccountLink_Domain}";
            var url = string.Format("{0}/AccountLink/{1}", acLinkDomain, api);
            var postStr = "token=" + HttpUtility.UrlEncode(postData);

            var postResult = this.DoRequestStrData(url, postStr);

            if (postResult == "連線錯誤" || string.IsNullOrWhiteSpace(postResult))
            {
                result.SetCode(7402);
            }

            result.SetSuccess(postResult);

            return result;
        }

        public bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public string DoRequestStrData(string requestUrl, string sPostData, string contentType = "application/x-www-form-urlencoded", string method = "POST")
        {
            var result = new DataResult<string>();

            HttpWebRequest httpWebRequest = null;

            if (requestUrl.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(AcceptAllCertifications);
                httpWebRequest = WebRequest.Create(requestUrl) as HttpWebRequest;
                httpWebRequest.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                httpWebRequest = WebRequest.Create(requestUrl) as HttpWebRequest;
            }

            httpWebRequest.Method = method;
            httpWebRequest.ContentType = contentType;

            string receiveData;
            try
            {
                if (method == "POST")
                {
                    // 取得request stream 並且寫入post data
                    using (StreamWriter sw = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        sw.Write(sPostData);
                        sw.Close();
                    }

                    // 取得server的reponse結果
                    HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
                    using (StreamReader sr = new StreamReader(httpWebResponse.GetResponseStream()))
                    {
                        receiveData = sr.ReadToEnd();
                    }
                }
                else
                {
                    using (WebResponse webResponse = httpWebRequest.GetResponse())
                    {
                        using (StreamReader streamReader = new StreamReader(webResponse.GetResponseStream()))
                        {
                            receiveData = streamReader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                string errorMsg = "錯誤訊息:" + exception.Message + "\r" + "連接網址:" + requestUrl;
                result.SetCode(10020);
                _logger.Warning(exception, errorMsg);

                receiveData = "連線錯誤";
            }

            return receiveData;
        }
        #endregion

        #region 共用
        /// <summary>
        /// 設定回傳RtnCode RtnMsg
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private string GetResult(BaseResult result)
        {
            string jsonStr = JsonConvert.SerializeObject(new BaseResult
            {
                RtnCode = result.RtnCode,
                RtnMsg = result.RtnMsg
            });

            return jsonStr;
        }
        #endregion
    }
}
