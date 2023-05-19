using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.OpenWalletApi.CustomReceiveApi
{
    public class BaseCustomReceiveApiRequest
    {
        /// <summary>
        /// 呼叫時間 yyyyMMddhhmmss	
        /// </summary>
        public string request_time { get; set; }

        /// <summary>
        /// 呼叫驗證碼
        /// </summary>
        public string mask { get; set; }
    }
}
