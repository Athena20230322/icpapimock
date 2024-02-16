using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.Payment
{
    public class GetTradeInfoDbReq
    {
        /// <summary>
        /// 平台商編號
        /// </summary>
        public long PlatformID { get; set; }

        /// <summary>
        /// 廠商編號
        /// </summary>
        public long MerchantID { get; set; }

        /// <summary>
        /// 交易編號
        /// </summary>
        public string MerchantTradeNo { get; set; }
    }
}
