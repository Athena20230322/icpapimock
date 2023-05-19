using ICP.Infrastructure.Core.Models;
using ICP.Modules.Api.PaymentCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.PaymentCenter.Interface
{
    public interface ITradeCommand
    {
        object Transaction(object requestModel);
    }
}
