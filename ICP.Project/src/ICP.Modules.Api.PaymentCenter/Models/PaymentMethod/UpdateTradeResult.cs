using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.PaymentCenter.Models.PaymentMethod
{
    public class UpdateTradeResult : BaseResult
    {
        public long tradeID { get; set; }
    }
}
