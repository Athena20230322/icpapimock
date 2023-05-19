using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.OpenWalletApi.WebUIApi
{
    public class BaseWebUIApiRequest
    {
        /// <summary>
        /// 呼叫時間 yyyy/MM/dd HH:mm:ss
        /// </summary>
        public string TimeStamp { get; set; }

        /// <summary>
        /// 由字串{Md5加密前綴}+所有的INPUT欄位依序組合+{Md5加密後綴}轉成MD5
        /// </summary>
        public string Mask { get; set; }
    }
}
