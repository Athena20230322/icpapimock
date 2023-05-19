using ICP.Library.Models.AuthorizationApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Authorization.Models
{
    public class ICPRequestContext : BaseAuthorizationApiRequest
    {
        public string Timestamp { get; set; }

        public string EncryptData { get; set; }

        public string DecryptData { get; set; }

        public AuthorizationApiKeyContext KeyContext { get; set; }

        public string LoginTokenID { get; set; }

        public string OPMID { get; set; }

        public long AppTokenID { get; set; }

        public long MID { get; set; }
    }
}