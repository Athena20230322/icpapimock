using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.Payment
{
    public class AddBarcodeDBReq
    {
        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 條碼類型   IC：付款條碼　88：儲值條碼
        /// </summary>
        public string BarcodeType { get; set; }

        /// <summary>
        /// 時間戳 格式：2019/01/01 00:00:00
        /// </summary>
        public string Timestamp { get; set; }

        /// <summary>
        /// 金流類型   條碼類型為IC時 1: 愛金卡帳戶 2: 銀行快付 ; 條碼類型為88時 1:現金儲值 2:中奬發票儲值
        /// </summary>
        public int PaymentType { get; set; }

        /// <summary>
        /// 付款方式識別碼
        /// </summary>
        public string PayID { get; set; }

        /// <summary>
        /// 儲值金額
        /// </summary>
        public int Amount { get; set; }
    }
}
