using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.OpenWalletApi.WebUIApi
{
    public class BaseWebUIApiResult
    {
        [JsonIgnore]
        public string StatusCode { get; set; }

        [JsonIgnore]
        public string StatusMessage { get; set; }
    }
}
