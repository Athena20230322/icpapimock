using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.PaymentCenter.Models.PaymentMethod.AccountLink
{
    public class AccountLinkInfoModel : BaseResult
    {
        public string IDNO { get; set; }
        public string INDTAccount { get; set; }
    }
}
