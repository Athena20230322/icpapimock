using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.OpenWalletApi.ClientApi
{
    public class QueryMemberMIDRequest: BaseClientApiRequest
    {
        public string access_token { get; set; }
    }
}
