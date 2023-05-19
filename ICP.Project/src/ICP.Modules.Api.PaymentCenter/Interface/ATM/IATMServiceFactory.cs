using ICP.Library.Models.PaymentCenterApi.Enums;

namespace ICP.Modules.Api.PaymentCenter.Interface.ATM
{
    public interface IATMServiceFactory
    {
        IATMService Create(PaymentSubType_ATM bankType);
    }
}
