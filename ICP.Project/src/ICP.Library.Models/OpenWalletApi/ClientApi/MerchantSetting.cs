using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.OpenWalletApi.ClientApi
{
    public class MerchantSetting
    {
        /// <summary>
        /// 廠商代碼
        /// </summary>
        public string client_id { get; set; }

        /// <summary>
        /// 廠商密碼
        /// </summary>
        public string client_mima { get; set; }
    }
}
