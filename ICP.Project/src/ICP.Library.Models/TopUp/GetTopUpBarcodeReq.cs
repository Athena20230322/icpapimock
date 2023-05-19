using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.TopUp
{
    public class GetTopUpBarcodeReq
    {
        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 交易金額
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// 金流類型(儲值) 1:現金儲值 2:中奬發票儲值
        /// </summary>
        public int PaymentType { get; set; }
    }
}
