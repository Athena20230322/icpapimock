using ICP.Modules.Api.Payment.Models.Payment;

namespace ICP.Modules.Api.Payment.Interface
{
    public interface IPaymentFactory
    {
        IPaymentType Create(ePaymentType paymentType);
    }
}
