using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.OpenWalletApi.ClientApi
{
    public class BaseClientApiRequest
    {
        /// <summary>
        /// 廠商代碼
        /// </summary>
        public string client_id { get; set; }

        /// <summary>
        /// 廠商密碼
        /// </summary>
        public string client_mima { get; set; }

        private string _request_id;

        /// <summary>
        /// 識別值
        /// </summary>
        public string request_id
        {
            get
            {
                if (_request_id == null)
                {
                    _request_id = Guid.NewGuid().ToString();
                }
                return _request_id;
            }
        }

        /// <summary>
        /// 裝置號碼
        /// </summary>
        public string device_id { get; set; }

        private string _request_time;

        /// <summary>
        /// 呼叫時間
        /// </summary>
        public string request_time
        {
            get
            {
                if (_request_time == null)
                {
                    _request_time = DateTime.Now.ToString("yyyyMMddHHmmss");
                }
                return _request_time;
            }
        }
    }
}
