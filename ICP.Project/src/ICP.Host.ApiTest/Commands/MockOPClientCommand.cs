using ICP.Host.ApiTest.Services;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICP.Host.ApiTest.Commands
{
    public class MockOPClientCommand
    {
        MockOPClientService _mockOPClientService;

        public MockOPClientCommand(
            MockOPClientService mockOPClientService
            )
        {
            _mockOPClientService = mockOPClientService;
        }

        public string CallApi(string url, string json)
        {
            var result = new BaseResult();
            result.SetError();

            var dtStart = DateTime.Now;

            try
            {
                switch (url)
                {
                    case "/SETMemberAuth_sit/AccessToken.html":
                        result = _mockOPClientService.GetAccessToken(json);
                        break;

                    case "/SETMemberAuth_sit/QueryMemberMID.html":
                        result = _mockOPClientService.QueryMemberMID(json);
                        break;

                    case "/SETMemberAuth_sit/QueryMemberInfo.html":
                        result = _mockOPClientService.QueryMemberInfo(json);
                        break;

                    case "/SETMemberAuth_sit/RefreshAccessToken.html":
                        result = _mockOPClientService.RefreshAccessToken(json);
                        break;

                    case "/SETMemberAPP_sit/QueryMobileBarcode.html":
                        result = _mockOPClientService.QueryMobileBarCode(json);
                        break;

                    default:
                        result.RtnMsg = "url not defined";
                        break;
                }
            }
            catch (Exception ex)
            {
                result.RtnMsg = ex.Message;
            }

            var dtEnd = DateTime.Now;

            var delag = new TimeSpan(dtEnd.Ticks - dtStart.Ticks).Milliseconds;

            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                delag,
                result
            });
        }
    }
}