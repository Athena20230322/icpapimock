using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.OpenWalletApi.Enums;
using ICP.Library.Models.OpenWalletApi.WebUIApi;
using ICP.Library.Repositories.MemberRepositories;
using ICP.Library.Repositories.OpenWalletApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace ICP.Host.ApiTest.Services
{
    public class MockOPWebUIService
    {
        OPWebUIApiRepository _oPWebUIApiRepository;
        MemberConfigCyptRepository _configCyptRepository;

        public MockOPWebUIService(
            OPWebUIApiRepository oPWebUIApiRepository,
            MemberConfigCyptRepository configCyptRepository
            )
        {
            _oPWebUIApiRepository = oPWebUIApiRepository;
            _configCyptRepository = configCyptRepository;
        }

        public DataResult<WebUIApiMethodType> UrlToMethodType(string url)
        {
            var result = new DataResult<WebUIApiMethodType>();
            result.SetError();

            WebUIApiMethodType rtnData;

            string methodName = url.Split('/').Last();

            if (!Enum.TryParse(methodName, out rtnData) || rtnData == WebUIApiMethodType.None)
            {
                result.RtnMsg = $"{methodName} no declare MethodType";
            }

            result.SetSuccess(rtnData);
            return result;
        }

        public string CallApi(string domain, string url, object obj, WebUIApiMethodType webUIApiMethodType)
        {
            var baseObj = obj as BaseWebUIApiRequest;
            if (baseObj == null)
            {

                return JsonConvert.SerializeObject(new { StatusCode = "xxxx", StatusMessage = $"{obj.GetType().Name} not inherit BaseWebUIApiRequest" });
            }

            baseObj.TimeStamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            //由字串{Md5加密前綴} + 所有的INPUT欄位依序組合 + {Md5加密後綴}轉成MD5
            var genMaskResult = _oPWebUIApiRepository.GenerateMask(webUIApiMethodType, obj);
            if (!genMaskResult.IsSuccess)
            {
                return JsonConvert.SerializeObject(new  { StatusCode = "xxxx", StatusMessage = genMaskResult.RtnMsg });
            }
            string Mask = genMaskResult.RtnData;

            //增加 Mask
            baseObj.Mask = Mask;

            //產生 json
            string json = JsonConvert.SerializeObject(obj);

            //aes 加密
            string encData = _configCyptRepository.Encrypt_OPWebUIEncData(json);

            //產生 body
            var formData = new Dictionary<string, string>();
            formData.Add("EncData", encData);
            var content = new FormUrlEncodedContent(formData);

            string stringResult = null;
            try
            {
                using (var _httpClient = new HttpClient { BaseAddress = new Uri(domain) })
                {
                    var postResult = _httpClient.PostAsync(url, content).Result;
                    stringResult = postResult.Content.ReadAsStringAsync().Result;
                }

                var jObj = JObject.Parse(stringResult);
                string resEncData = jObj.Value<string>("EncData");
                jObj.Remove("EncData");
                if (!string.IsNullOrWhiteSpace(resEncData))
                {
                    string resRtnData = _configCyptRepository.Decrypt_OPWebUIEncData(resEncData);
                    jObj.Add("RtnData", JToken.Parse(resRtnData));
                }
                return jObj.ToString(Formatting.None);
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { StatusCode = "ex", StatusMessage = ex.Message });
            }
        }
    }
}