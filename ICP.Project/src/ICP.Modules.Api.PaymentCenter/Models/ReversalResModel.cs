using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.PaymentCenter.Models
{
    public class ReversalResModel : BaseResult
    {
        /// <summary>
        /// 取消單號
        /// </summary>
        public long PaymentCenterTradeID { get; set; }

        /// <summary>
        /// 取消日期
        /// </summary>
        public DateTime TradeDate { get; set; }
    }
}
