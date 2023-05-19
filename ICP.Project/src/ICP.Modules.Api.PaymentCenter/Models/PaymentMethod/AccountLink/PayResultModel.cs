using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.PaymentCenter.Models.PaymentMethod.AccountLink
{
    public class PayResultModel : BaseResult
    {
        /// <summary>
        /// 銀行交易序號
        /// </summary>
        public string BankTradeNo { get; set; }
    }
}
