using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.PaymentCenter.Models.PaymentMethod.AccountLink
{
    public class VirtualAccountModel : BaseResult
    {
        public string VirtualAccount { get; set; }
    }
}
