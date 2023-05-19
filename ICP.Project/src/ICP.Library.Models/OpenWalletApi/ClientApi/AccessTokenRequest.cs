using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.OpenWalletApi.ClientApi
{
    public class AccessTokenRequest: BaseClientApiRequest
    {
        /// <summary>
        /// 授權碼
        /// </summary>
        public string code { get; set; }
    }
}
