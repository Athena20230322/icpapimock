using Autofac.Features.Metadata;
using ICP.Modules.Api.PaymentCenter.Enums;
using ICP.Modules.Api.PaymentCenter.Interface;
using System;
using System.Collections.Generic;
using System.Linq;


namespace ICP.Modules.Api.PaymentCenter.Factory
{
    public class PaymentMethodFactory : IPaymentMethodFactory
    {
        private readonly IEnumerable<Meta<Func<IPaymentMethod>>> _paymentMethods = null;

        public PaymentMethodFactory(IEnumerable<Meta<Func<IPaymentMethod>>> paymentMethods)
        {
            _paymentMethods = paymentMethods;
        }

        public IPaymentMethod Create(ePaymentType paymentMethodType)
        {
            return _paymentMethods.FirstOrDefault(x => paymentMethodType.Equals(x.Metadata[nameof(ePaymentType)]))?.Value();
        }
    }
}
