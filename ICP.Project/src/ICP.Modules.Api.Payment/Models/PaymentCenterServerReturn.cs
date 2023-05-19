using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Payment.Models
{
    public class PaymentCenterServerReturn
    {
        /// <summary>
        /// 交易序號
        /// </summary>
        public string TradeID { get; set; }

        /// <summary>
        /// xml 格式交易資料
        /// </summary>
        public string XmlData { get; set; }

        /// <summary>
        /// json 格式交易資料
        /// </summary>
        public string JsonData { get; set; }
    }
}
