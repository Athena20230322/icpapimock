using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICP.Host.ApiTest.Models
{
    using Library.Models.OpenWalletApi.CustomReceiveApi;

    public class MockOPCustomReceiveResult: BaseCustomReceiveApiResult
    {
        public string Msg { get; set; }
    }
}