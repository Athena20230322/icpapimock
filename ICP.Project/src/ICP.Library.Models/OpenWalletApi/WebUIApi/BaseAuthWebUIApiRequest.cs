using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.OpenWalletApi.WebUIApi
{
    public class BaseAuthWebUIApiRequest: BaseWebUIApiRequest
    {
        /// <summary>
        /// 登入Token
        /// </summary>
        public string Token { get; set; }
    }
}
