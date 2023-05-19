using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.MerchantModels
{
    /// <summary>
    /// 特店撥款天數
    /// </summary>
    public class CustomerAllocateDayModel
    {
        /// <summary>
        /// 特店編號
        /// </summary>
        public long CustomerID { get; set; }

        /// <summary>
        /// 撥款方式
        /// </summary>
        public byte AllocateType { get; set; }

        /// <summary>
        /// 付款方式 0: 撥款方式為不分付款方式  1: 電支帳戶 2:連結扣款帳戶
        /// </summary>
        public short PaymentTypeID { get; set; }

        /// <summary>
        /// 撥款天數
        /// </summary>
        public int AllocateDay { get; set; }
    }
}
