using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.PaymentCenter.Models.TransactionMethod
{
    using Infrastructure.Core.Models;

    public class AddTradeResult : BaseResult
    {
        public long tradeID { get; set; }
    }
}
