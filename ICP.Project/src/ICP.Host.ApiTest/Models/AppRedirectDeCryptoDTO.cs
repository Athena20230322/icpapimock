using ICP.Library.Models.AuthorizationApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICP.Host.ApiTest.Models
{
    public class AppRedirectDeCryptoDTO : BaseAuthorizationApiRequest
    {
        public string Url { get; set; }
    }
}