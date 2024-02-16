using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace ICP.Library.Repositories.OpenWalletApi
{
    using Infrastructure.Core.Models;
    using Infrastructure.Core.Extensions;
    using Models.OpenWalletApi.ClientApi;
    using Library.Repositories.MemberRepositories;
    using ICP.Library.Repositories.SystemRepositories;
    using ICP.Infrastructure.Abstractions.Logging;

    /// <summary>
    /// OP既有API
    /// </summary>
    public class OPClientApiRepository
    {
        MemberConfigCyptRepository _configCyptRepository;
        ConfigKeyValueRepository _configKeyValueRepository;
        ILogger<OPClientApiRepository> _logger;

        public OPClientApiRepository(
            MemberConfigCyptRepository configCyptRepository,
            ConfigKeyValueRepository configKeyValueRepository,
            ILogger<OPClientApiRepository> logger 
            )
        {
            _configCyptRepository = configCyptRepository;
            _configKeyValueRepository = configKeyValueRepository;
            _logger = logger;
        }

        /// <summary>
        /// 發動 api
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="url"></param>
        /// <param name="obj"></param>
        /// <param name="customApiMethodType"></param>
        /// <returns></returns>
        private DataResult<TResult> CallApi<TResult>(string url, object obj, GeneratePropertiesObject gen) where TResult : BaseClientApiResult, new()
        {
            string address = _configKeyValueRepository.op_client_domain;

            //所有的INPUT欄位依序組合
            string valueString = gen.ToValueString();

            //由字串{Md5加密前綴} + 所有的INPUT欄位依序組合 + {Md5加密後綴}轉成MD5
            string Mask = _configCyptRepository.MD5_CustomOpenWalletMask(valueString);

            if (Mask != null) Mask = Mask.ToLower();

            //INPUT 欄位
            JObject jb = JObject.FromObject(obj);

            //增加 Mask
            jb.Add("mask", Mask);

            //產生 json
            string json = jb.ToString(Formatting.None);

            //aes 加密
            string encData = _configCyptRepository.Encrypt_CustomOpenWalletEncData(json);

            //產生 body
            var formData = new Dictionary<string, string>();
            formData.Add("client_id", _configKeyValueRepository.op_client_id);
            formData.Add("v", encData);

            var content = new FormUrlEncodedContent(formData);

            string stringResult = null;
            TResult model = null;

            try
            {
                using (var handler = new WebRequestHandler())
                {
                    handler.ServerCertificateValidationCallback = delegate { return true; };

                    using (var _httpClient = new HttpClient(handler) { BaseAddress = new Uri(address) })
                    {
                        var postResult = _httpClient.PostAsync(url, content).Result;
                        stringResult = postResult.Content.ReadAsStringAsync().Result;
                    }
                }

                string resJson = _configCyptRepository.Decrypt_CustomOpenWalletEncData(stringResult);
                model = JsonConvert.DeserializeObject<TResult>(resJson);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, url);
                model = new TResult();
                model.errorCode = "ex";
                model.errorMessage = ex.Message;
            }

            return modelToResult(model);
        }

        /// <summary>
        /// api 結果轉成 DataResult
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        private DataResult<T> modelToResult<T>(T model) where T : BaseClientApiResult
        {
            var result = new DataResult<T>();
            result.SetSuccess(model);

            if (!string.IsNullOrEmpty(model.errorCode) && model.errorCode != "00")
            {
                result.SetCode(200025);
            }

            return result;
        }

        private T GetOPClientApiRequest<T>() where T: BaseClientApiRequest, new()
        {
            var request = new T();
            request.client_id = _configKeyValueRepository.op_client_id;
            request.client_mima = _configKeyValueRepository.op_client_mima;
            return request;
        }

        public DataResult<AccessTokenResult> GetAccessToken(string code, bool mock = false)
        {
            string url = "/SETMemberAuth_sit/AccessToken.html";

            var request = GetOPClientApiRequest<AccessTokenRequest>();
            request.code = code;

            var gen =
                new GeneratePropertiesObject<AccessTokenRequest>(request)
                .Add(t => t.client_id)
                .Add(t => t.client_mima)
                .Add(t => t.request_id)
                .Add(t => t.device_id)
                .Add(t => t.code)
                .Add(t => t.request_time);

            if (mock)
            {
                //假資料
                var model = new AccessTokenResult
                {
                    access_token = Guid.NewGuid().ToString().Replace("-", string.Empty),
                    expires = DateTime.Now.AddDays(1).ToString("yyyyMMddhhmmss")
                };
                var result = new DataResult<AccessTokenResult>();
                result.SetSuccess(model);
                return result;
            }

            if (!_configCyptRepository.ClientOpenWalletSwitch)
            {
                var result = new DataResult<AccessTokenResult>();
                result.SetError();
                return result;
            }

            return CallApi<AccessTokenResult>(url, request, gen);
        }

        public DataResult<QueryMemberMIDResult> QueryMemberMID(string AccessToken, bool mock = false)
        {
            string url = "/SETMemberAuth_sit/QueryMemberMID.html";

            var request = GetOPClientApiRequest<QueryMemberMIDRequest>();
            request.access_token = AccessToken;

            var gen =
                new GeneratePropertiesObject<QueryMemberMIDRequest>(request)
                .Add(t => t.client_id)
                .Add(t => t.client_mima)
                .Add(t => t.request_id)
                .Add(t => t.device_id)
                .Add(t => t.access_token)
                .Add(t => t.request_time);

            if (mock)
            {
                //假資料
                var model = new QueryMemberMIDResult
                {
                    mid = Guid.NewGuid().ToString().Replace("-", string.Empty)
                };
                var result = new DataResult<QueryMemberMIDResult>();
                result.SetSuccess(model);
                return result;
            }

            if (!_configCyptRepository.ClientOpenWalletSwitch)
            {
                var result = new DataResult<QueryMemberMIDResult>();
                result.SetError();
                return result;
            }

            return CallApi<QueryMemberMIDResult>(url, request, gen);
        }

        public DataResult<QueryMemberInfoResult> QueryMemberInfo(string AccessToken, string mid, bool mock = false)
        {
            string url = "/SETMemberAuth_sit/QueryMemberInfo.html";

            var request = GetOPClientApiRequest<RefreshAccessTokenRequest>();
            request.access_token = AccessToken;
            request.mid = mid;

            var gen =
                new GeneratePropertiesObject<RefreshAccessTokenRequest>(request)
                .Add(t => t.client_id)
                .Add(t => t.client_mima)
                .Add(t => t.request_id)
                .Add(t => t.device_id)
                .Add(t => t.access_token)
                .Add(t => t.mid)
                .Add(t => t.request_time);

            if (mock)
            {
                //假資料
                var model = new QueryMemberInfoResult
                {
                    phone = "09" + DateTime.Now.ToString("ddHHmmss")
                };
                var result = new DataResult<QueryMemberInfoResult>();
                result.SetSuccess(model);
                return result;
            }

            if (!_configCyptRepository.ClientOpenWalletSwitch)
            {
                var result = new DataResult<QueryMemberInfoResult>();
                result.SetError();
                return result;
            }

            return CallApi<QueryMemberInfoResult>(url, request, gen);
        }

        public DataResult<QueryMobileBarCodeResult> QueryMobileBarCode(string AccessToken, string mid, bool mock = false)
        {
            string url = "/SETMemberAPP_sit/QueryMobileBarcode.html";

            var request = GetOPClientApiRequest<RefreshAccessTokenRequest>();
            request.access_token = AccessToken;
            request.mid = mid;

            var gen =
                new GeneratePropertiesObject<RefreshAccessTokenRequest>(request)
                .Add(t => t.client_id)
                .Add(t => t.client_mima)
                .Add(t => t.request_id)
                .Add(t => t.device_id)
                .Add(t => t.access_token)
                .Add(t => t.mid)
                .Add(t => t.request_time);

            if (mock)
            {
                //假資料
                var model = new QueryMobileBarCodeResult
                {
                    mobile_barcode = string.Empty
                };
                var result = new DataResult<QueryMobileBarCodeResult>();
                result.SetSuccess(model);
                return result;
            }

            if (!_configCyptRepository.ClientOpenWalletSwitch)
            {
                var result = new DataResult<QueryMobileBarCodeResult>();
                result.SetError();
                return result;
            }

            return CallApi<QueryMobileBarCodeResult>(url, request, gen);
        }

        public DataResult<AccessTokenResult> RefreshAccessToken(string AccessToken, string mid, bool mock = false)
        {
            string url = "/SETMemberAuth_sit/RefreshAccessToken.html";

            var request = GetOPClientApiRequest<RefreshAccessTokenRequest>();
            request.access_token = AccessToken;
            request.mid = mid;

            var gen =
                new GeneratePropertiesObject<RefreshAccessTokenRequest>(request)
                .Add(t => t.client_id)
                .Add(t => t.client_mima)
                .Add(t => t.request_id)
                .Add(t => t.device_id)
                .Add(t => t.access_token)
                .Add(t => t.mid)
                .Add(t => t.request_time);

            if (mock)
            {
                //假資料
                var model = new AccessTokenResult
                {
                    access_token = request.access_token,
                    expires = DateTime.Now.AddDays(1).ToString("yyyyMMddHHmmss")
                };
                var result = new DataResult<AccessTokenResult>();
                result.SetSuccess(model);
                return result;
            }

            if (!_configCyptRepository.ClientOpenWalletSwitch)
            {
                var result = new DataResult<AccessTokenResult>();
                result.SetError();
                return result;
            }

            return CallApi<AccessTokenResult>(url, request, gen);
        }
    }
}