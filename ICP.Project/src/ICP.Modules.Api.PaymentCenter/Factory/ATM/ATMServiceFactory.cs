using Autofac.Features.Metadata;
using ICP.Library.Models.PaymentCenterApi.Enums;
using ICP.Modules.Api.PaymentCenter.Interface.ATM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ICP.Modules.Api.PaymentCenter.Factory.ATM
{
    public class ATMServiceFactory : IATMServiceFactory
    {
        private readonly IEnumerable<Meta<Func<IATMService>>> _atmServices = null;

        public ATMServiceFactory(IEnumerable<Meta<Func<IATMService>>> atmServices)
        {
            _atmServices = atmServices;
        }

        public IATMService Create(PaymentSubType_ATM bankType)
        {
            return _atmServices.FirstOrDefault(x => bankType.Equals(x.Metadata[nameof(PaymentSubType_ATM)]))?.Value();
        }
    }
}
