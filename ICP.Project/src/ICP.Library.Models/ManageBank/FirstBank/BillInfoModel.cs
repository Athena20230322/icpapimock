using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.ManageBank.FirstBank
{
    /// <summary>
    /// 單據資訊
    /// </summary>
    public class BillInfoModel
    {
        /// <summary>
        /// 單據類別
        /// </summary>
        public string BillType { get; set; }

        /// <summary>
        /// 單據號碼
        /// </summary>
        public string BillNO { get; set; }

        /// <summary>
        /// 單據金額
        /// </summary>
        public int BillAmount { get; set; }

        /// <summary>
        /// 單據日期
        /// </summary>
        public string BillDate { get; set; }

        /// <summary>
        /// 現金折扣
        /// </summary>
        public int Discount { get; set; }

        /// <summary>
        /// 實付金額
        /// </summary>
        public int NetAmount { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        public string Remark { get; set; }
    }
}
