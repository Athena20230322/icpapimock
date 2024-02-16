using ICP.Infrastructure.Core.Models;
using ICP.Modules.Api.PaymentCenter.Models;

namespace ICP.Modules.Api.PaymentCenter.Interface.ATM
{
    public interface IATMService
    {
        BaseResult NotifyBank(AtmNotifyModel atmNotifyModel);
    }
}
