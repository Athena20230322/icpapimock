using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ICP.Host.ApiTest.Services
{
    using Library.Models.OpenWalletApi.Enums;
    using Library.Models.OpenWalletApi.CustomReceiveApi;
    using Library.Repositories.OpenWalletApi;
    using Library.Repositories.MemberRepositories;
    using ICP.Infrastructure.Core.Models;
    using ICP.Infrastructure.Core.Extensions;
    using ICP.Host.ApiTest.Models;

    public class MockOPCustomReceiveService
    {
        OPCustomApiRepository _oPCustomApiRepository;
        MemberConfigCyptRepository _configCyptRepository;

        public MockOPCustomReceiveService(
            OPCustomApiRepository oPCustomApiRepository,
            MemberConfigCyptRepository configCyptRepository
            
            )
        {
            _oPCustomApiRepository = oPCustomApiRepository;
            _configCyptRepository = configCyptRepository;
        }

        public DataResult<CustomApiMethodType> UrlToMethodType(string url)
        {
            var result = new DataResult<CustomApiMethodType>();
            result.SetError();

            CustomApiMethodType rtnData;

            string methodName = url.Split('/').Last();

            if (!Enum.TryParse(methodName, out rtnData) || rtnData == CustomApiMethodType.None)
            {
                result.RtnMsg = $"{methodName} no declare MethodType";
            }

            result.SetSuccess(rtnData);
            return result;
        }

        public MockOPCustomReceiveResult CallApi(string domain, string url, object obj, CustomApiMethodType customApiMethodType)
        {
            var baseObj = obj as BaseCustomReceiveApiRequest;
            if (baseObj == null)
            {

                return new MockOPCustomReceiveResult { Code = "xx", Msg = $"{obj.GetType().Name} not inherit BaseCustomReceiveApiRequest" };
            }

            baseObj.request_time = DateTime.Now.ToString("yyyyMMddhhmmss");

            //由字串{Md5加密前綴} + 所有的INPUT欄位依序組合 + {Md5加密後綴}轉成MD5
            var genMaskResult = _oPCustomApiRepository.GenerateMask(customApiMethodType, obj);
            if (!genMaskResult.IsSuccess)
            {
                return new MockOPCustomReceiveResult { Code = "xx", Msg = genMaskResult.RtnMsg };
            }
            string Mask = genMaskResult.RtnData;

            //增加 Mask
            baseObj.mask = Mask;

            //產生 json
            string json = JsonConvert.SerializeObject(obj);

            //aes 加密
            string encData = _configCyptRepository.Encrypt_CustomOpenWalletEncData(json);

            //產生 body
            var formData = new Dictionary<string, string>();
            formData.Add("v", encData);
            var content = new FormUrlEncodedContent(formData);

            string stringResult = null;
            try
            {
                using (var _httpClient = new HttpClient { BaseAddress = new Uri(domain) })
                {
                    var postResult = _httpClient.PostAsync(url, content).Result;
                    stringResult = postResult.Content.ReadAsStringAsync().Result;
                }

                return JsonConvert.DeserializeObject<MockOPCustomReceiveResult>(stringResult);
            }
            catch (Exception ex)
            {
                return new MockOPCustomReceiveResult { Code = "ex", Msg = ex.Message };
            }
        }
    }
}