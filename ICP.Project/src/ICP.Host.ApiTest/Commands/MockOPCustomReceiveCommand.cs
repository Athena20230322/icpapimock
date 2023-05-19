using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;

namespace ICP.Host.ApiTest.Commands
{
    using Library.Models.OpenWalletApi.Enums;
    using Library.Models.OpenWalletApi.CustomReceiveApi;
    using Services;
    using ICP.Infrastructure.Core.Models;
    using ICP.Infrastructure.Core.Extensions;
    using ICP.Host.ApiTest.Models;

    public class MockOPCustomReceiveCommand
    {
        MockOPCustomReceiveService _mockOPCustomReceiveService;

        public MockOPCustomReceiveCommand(
            MockOPCustomReceiveService mockOPCustomReceiveService
            )
        {
            _mockOPCustomReceiveService = mockOPCustomReceiveService;
        }

        public object CallApi(string domain, string url, string json)
        {
            MockOPCustomReceiveResult result;
            long delag = 0;

            var url2MethodNameResult = _mockOPCustomReceiveService.UrlToMethodType(url);
            if (!url2MethodNameResult.IsSuccess)
            {
                result = new MockOPCustomReceiveResult { Code = "xx", Msg = url2MethodNameResult.RtnMsg };
                return new { delag, result };
            }
            var customApiMethodType = url2MethodNameResult.RtnData;

            var dtStart = DateTime.Now;

            try
            {
                object obj = null;
                switch (customApiMethodType)
                {
                    case CustomApiMethodType.NoticeMemberDelete:
                        obj = JsonConvert.DeserializeObject<NoticeMemberDeleteRequest>(json);
                        break;

                    case CustomApiMethodType.NoticeMobileBarcode:
                        obj=JsonConvert.DeserializeObject<NoticeMobileBarcodeRequest>(json);
                        break;

                    default:
                        throw new Exception($"{customApiMethodType} test tool switch case not defined");
                }

                result = _mockOPCustomReceiveService.CallApi(domain, url, obj, customApiMethodType);
            }
            catch (Exception ex)
            {
                result = new MockOPCustomReceiveResult { Code = "ex", Msg = ex.Message };
            }

            var dtEnd = DateTime.Now;

            delag = new TimeSpan(dtEnd.Ticks - dtStart.Ticks).Milliseconds;

            return new { delag, result };
        }
    }
}