using ICP.Library.Models.AuthorizationApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.Certificate
{
    public class ExchangePucCertResult : BaseAuthorizationApiResult
    {
        public string ServerPubCert { get; set; }

        public long ServerPubCertID { get; set; }

        public string ExpireDate { get; set; }
    }
}
