using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.Payment
{
    public class TradeTypeModel
    {
        /// <summary>
        /// 付款方式代碼
        /// </summary>
        public int PaymentTypeID { get; set; }

        /// <summary>
        /// 付款方式子代碼(收單行編號)
        /// </summary>
        public int PaymentSubTypeID { get; set; }

        /// <summary>
        /// 收單行名稱
        /// </summary>
        public string PaymentSubTypeName { get; set; }

        /// <summary>
        /// 收單行註解
        /// </summary>
        public string PaymentSubTypeNotes { get; set; }

        /// <summary>
        /// 收單行狀態
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 銀行代碼
        /// </summary>
        public string BankCode { get; set; }

        /// <summary>
        /// 付款方式名稱
        /// </summary>
        public string PaymentName { get; set; }

        /// <summary>
        /// 付款方式註解
        /// </summary>
        public string PaymentNotes { get; set; }
    }
}
