using ICP.Library.Models.AuthorizationApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.Certificate
{
    public class GenerateAesResult : BaseAuthorizationApiResult
    {
        public long EncKeyID { get; set; }

        public string AES_Key { get; set; }

        public string AES_IV { get; set; }

        public string ExpireDate { get; set; }
    }
}
