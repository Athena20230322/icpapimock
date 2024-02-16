using ICP.Modules.Api.PaymentCenter.Enums;
using ICP.Modules.Api.PaymentCenter.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.PaymentCenter.Interface
{
    public interface IPaymentMethodFactory
    {
        IPaymentMethod Create(ePaymentType paymentMethodType);
    }
}
