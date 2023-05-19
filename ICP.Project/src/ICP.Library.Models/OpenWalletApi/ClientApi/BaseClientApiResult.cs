using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.OpenWalletApi.ClientApi
{
    public class BaseClientApiResult
    {
        /// <summary>
        /// 識別值
        /// </summary>
        public string request_id { get; set; }

        /// <summary>
        /// 回應代碼
        /// </summary>
        public string errorCode { get; set; }

        /// <summary>
        /// 回應說明
        /// </summary>
        public string errorMessage { get; set; }
    }
}
