using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.MemberModels
{
    public class MemberLawBasicModel
    {
        /// <summary>
        /// 每月付款+轉出額度
        /// </summary>
        public decimal PaymentLimitAMT { get; set; }

        /// <summary>
        /// 每日轉帳轉出額度
        /// </summary>
        public decimal DailyTransferOutLimitAMT { get; set; }

        /// <summary>
        /// 每月轉帳轉出額度
        /// </summary>
        public decimal MonthlyTransferOutLimitAMT { get; set; }

        /// <summary>
        /// 每月轉帳轉入額度
        /// </summary>
        public decimal TransferInLimitAMT { get; set; }

        /// <summary>
        /// 儲值額度
        /// </summary>
        public decimal TopUpLimitAMT { get; set; }
    }
}
