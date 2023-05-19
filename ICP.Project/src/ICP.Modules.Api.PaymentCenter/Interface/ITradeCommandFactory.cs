using ICP.Modules.Api.PaymentCenter.Enums;

namespace ICP.Modules.Api.PaymentCenter.Interface
{
    public interface ITradeCommandFactory
    {
        ITradeCommand Create(eTradeMode tradeMode);
    }
}
