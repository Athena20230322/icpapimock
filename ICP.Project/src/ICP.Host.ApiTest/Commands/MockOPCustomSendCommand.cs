using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace ICP.Host.ApiTest.Commands
{
    using ICP.Infrastructure.Core.Extensions;
    using ICP.Infrastructure.Core.Models;
    using ICP.Library.Models.OpenWalletApi.CustomSendApi;
    using ICP.Library.Models.OpenWalletApi.Enums;
    using Models;
    using Services;

    public class MockOPCustomSendCommand
    {
        MockOPCustomSendService _mockOPCustomSendService;

        public MockOPCustomSendCommand(
            MockOPCustomSendService mockOPCustomSendService
            )
        {
            _mockOPCustomSendService = mockOPCustomSendService;
        }

        private DataResult<CustomApiMethodType> MethodNameToType(string methodName)
        {
            var result = new DataResult<CustomApiMethodType>();
            result.SetError();

            CustomApiMethodType rtnData;

            if (!Enum.TryParse(methodName, out rtnData) || rtnData == CustomApiMethodType.None)
            {
                result.RtnMsg = $"{methodName} no declare MethodType";
            }

            result.SetSuccess(rtnData);
            return result;
        }

        public string CallApi(string MethodName, string json)
        {
            var result = new BaseResult();
            result.SetError();

            long delag = 0;

            var dtStart = DateTime.Now;

            // API 方法轉 Type
            var methodTypeResult = MethodNameToType(MethodName);
            if (!methodTypeResult.IsSuccess)
            {
                result.SetError(methodTypeResult);
                return JsonConvert.SerializeObject(new { delag, result });
            }
            var MethodType = methodTypeResult.RtnData;

            try
            {
                switch (MethodType)
                {
                    case CustomApiMethodType.BindicashAccount:
                        result = _mockOPCustomSendService.BindicashAccount(json);
                        break;

                    case CustomApiMethodType.UnBindicashAccount:
                        result = _mockOPCustomSendService.UnBindicashAccount(json);
                        break;

                    default:
                        result.RtnMsg = "MethodType not defined";
                        break;
                }
            }
            catch (Exception ex)
            {
                result.RtnMsg = ex.Message;
            }

            var dtEnd = DateTime.Now;

            delag = new TimeSpan(dtEnd.Ticks - dtStart.Ticks).Milliseconds;

            return JsonConvert.SerializeObject(new { delag, result });
        }
    }
}