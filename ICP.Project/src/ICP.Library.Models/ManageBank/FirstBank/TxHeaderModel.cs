using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.ManageBank.FirstBank
{
    public class TxHeaderModel
    {
        /// <summary>
        /// 交易代號
        /// </summary>
        public string TxID { get; set; }

        /// <summary>
        /// 訊息唯一key
        /// CustId-YYYYMMDD-xxxxxx(xxxxxx:6碼序號)
        /// </summary>
        public string SvcRqId { get; set; }

        /// <summary>
        /// 客戶統編含重複續號
        /// 統一編號+空白+空白+重複續號12345678  0
        /// </summary>
        public string CustId { get; set; }

        /// <summary>
        /// 訊息類別
        /// RQ: request, RS: response
        /// </summary>
        public string MsgDirection { get; set; }
    }
}
