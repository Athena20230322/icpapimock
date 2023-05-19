using ICP.Host.ApiTest.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICP.Host.ApiTest.Commands
{
    using ICP.Library.Models.OpenWalletApi.WebUIApi;
    using Library.Models.OpenWalletApi.Enums;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class MockOPWebUICommand
    {
        MockOPWebUIService _mockOPWebUIService;

        public MockOPWebUICommand(
            MockOPWebUIService mockOPWebUIService
            )
        {
            _mockOPWebUIService = mockOPWebUIService;
        }

        private string CallApiResult(long delag, object result)
        {
            return JsonConvert.SerializeObject(new { delag, result });
        }

        private string CallApiResult(long delag, string json)
        {
            var rtnData = new JObject();
            rtnData.Add("delag", delag);
            rtnData.Add("result", JToken.Parse(json));
            return rtnData.ToString(Formatting.None);
        }

        public string CallApi(string domain, string url, string json)
        {
            object result;
            string resJson = null;
            long delag = 0;

            var url2MethodNameResult = _mockOPWebUIService.UrlToMethodType(url);
            if (!url2MethodNameResult.IsSuccess)
            {
                result = new { StatusCode = "xxxx", StatusMessage = url2MethodNameResult.RtnMsg };
                return CallApiResult(delag, result);
            }
            var customApiMethodType = url2MethodNameResult.RtnData;

            DateTime dtStart = DateTime.Now;
            DateTime dtEnd;

            try
            {
                object obj = null;
                switch (customApiMethodType)
                {
                    case WebUIApiMethodType.Login:
                        obj = JsonConvert.DeserializeObject<LoginWebUIRequest>(json);
                        break;

                    case WebUIApiMethodType.GetUserData:
                        obj = JsonConvert.DeserializeObject<GetUserDataWebUIRequest>(json);
                        break;

                    case WebUIApiMethodType.AgreeRegister:
                        obj = JsonConvert.DeserializeObject<AgreeRegisterWebUIRequest>(json);
                        break;

                    default:
                        throw new Exception($"{customApiMethodType} test tool switch case not defined");
                }

                resJson = _mockOPWebUIService.CallApi(domain, url, obj, customApiMethodType);
                dtEnd = DateTime.Now;
                delag = new TimeSpan(dtEnd.Ticks - dtStart.Ticks).Milliseconds;
                return CallApiResult(delag, resJson);
            }
            catch (Exception ex)
            {
                result = new { StatusCode = "ex", StatusMessage = ex.Message };
                dtEnd = DateTime.Now;
                delag = new TimeSpan(dtEnd.Ticks - dtStart.Ticks).Milliseconds;
                return CallApiResult(delag, result);
            }
        }
    }
}