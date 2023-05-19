using Autofac.Features.Metadata;
using ICP.Modules.Api.Payment.Interface;
using ICP.Modules.Api.Payment.Models.Payment;
using System.Collections.Generic;
using System.Linq;

namespace ICP.Modules.Api.Payment.Factory
{
    public class PaymentFactory : IPaymentFactory
    {
        private readonly IEnumerable<Meta<IPaymentType>> _paymentManagers = null;

        public PaymentFactory(IEnumerable<Meta<IPaymentType>> paymentManagers)
        {
            _paymentManagers = paymentManagers;
        }

        public IPaymentType Create(ePaymentType paymentType)
        {
            return _paymentManagers.FirstOrDefault(x => paymentType.Equals(x.Metadata[nameof(ePaymentType)]))?.Value;
        }
    }
}
