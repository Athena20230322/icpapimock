using ICP.Modules.Api.Payment.Interface;
using ICP.Modules.Api.Payment.Models.Payment;
using System;

namespace ICP.Modules.Api.Payment.Services
{
    public class PaymentProvider
    {
        private readonly IPaymentFactory _paymentTypeFactory = null;
        private IPaymentType payment = null;

        private readonly ITradeModeFactory _tradeModeFactory = null;
        private ITradeMode tradeMode = null;

        public PaymentProvider(IPaymentFactory paymentFactory, ITradeModeFactory tradeModeFactory)
        {
            _paymentTypeFactory = paymentFactory;
            _tradeModeFactory = tradeModeFactory;
        }

        public IPaymentType GetPaymentType(int paymentTypeID)
        {
            if (Enum.IsDefined(typeof(ePaymentType), paymentTypeID))
            {
                // 將字串轉換為列舉
                payment = _paymentTypeFactory.Create((ePaymentType)paymentTypeID);
            }

            return payment;
        }

        public ITradeMode GetTradeMode(int tradeModeInt)
        {
            if (Enum.IsDefined(typeof(eTradeMode), tradeModeInt))
            {
                // 將字串轉換為列舉
                tradeMode = _tradeModeFactory.Create((eTradeMode)tradeModeInt);
            }

            return tradeMode;
        }
    }
}
